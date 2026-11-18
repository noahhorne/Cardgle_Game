using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using Unity.InferenceEngine.Tokenization.Parsers.HuggingFace;
using Unity.VisualScripting;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
#endif

public class CombatManager : MonoBehaviour
{
    private enum CombatMode {None, Attack, Heal, Ability }
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private List<PlayerToken> tokens;
    
    [Header("Glow colours")] 
     [SerializeField] private Color neutralColor = new Color(1f, 0.9f, 0.3f, 1f);
     [SerializeField] private Color attackColor = new Color(1f, 0.25f, 0.25f, 1f);
     [SerializeField] private Color healColor = new Color(0.3f, 1f, 0.4f, 1f);
     [SerializeField] private Color abilityColor = new Color(0.7f, 0.4f, 1f, 1f);
    private CardObject selected;
    private CombatMode mode = CombatMode.None;

    private void OnEnable()
    {
        CardObject.Spawned += HandleCardSpawned;
        turnManager.PhaseChanged += HandlePhaseChanged;
        foreach (PlayerToken t in tokens)
        {
            t.Clicked += HandleTokenClicked;
            t.Defeated += HandleTokenDefeated;
        }
    }

    private void OnDisable()
    {
        CardObject.Spawned -= HandleCardSpawned;
    }

    private void HandleCardSpawned(CardObject card)
    {
        card.Clicked += HandleCardClicked;
        card.Died += HandleCardDied;
    }

    private void HandlePhaseChanged(TurnPhase phase)
    {
        Deselect();

        if (turnManager.IsTurnPhase && !turnManager.IsGameOver)
        {
            foreach (CardObject card in boardManager.GetCards(turnManager.CurrentPlayer))
            {
                Debug.Log($"[Tick] ticking {card.Data.cardName} ({card.Owner})");
                card.TickEffects();
            }
        }
    }

    private void Update()
    {
        if (selected == null)
        {
            return;
        }

        if (KeyPressed(KeyCode.A))
        {
            SetMode(CombatMode.Attack);
        } else if (KeyPressed(KeyCode.H)) {
            SetMode(CombatMode.Heal);
        } else if (KeyPressed(KeyCode.E)) {
            SetMode(CombatMode.Ability);
        } else if (KeyPressed(KeyCode.Escape)) {
            Deselect();
        }
    }

    private bool KeyPressed(KeyCode key)
    {
        #if ENABLE_INPUT_SYSTEM
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return false;
        }

        switch (key)
        {
            case KeyCode.A: return kb.aKey.wasPressedThisFrame;
            case KeyCode.H: return kb.hKey.wasPressedThisFrame;
            case KeyCode.E: return kb.eKey.wasPressedThisFrame;
            case KeyCode.Escape: return kb.escapeKey.wasPressedThisFrame;
        }
        return false;
        #else
        return Input.GetKeyDown(key);
        #endif
    }

    private void SetMode(CombatMode newMode)
    {
        Debug.Log($"[Combat] SetMode({newMode}) | card={selected.Data.cardName} | ability={(selected.Data.activeAbility == null ? "NONE ASSIGNED" : selected.Data.activeAbility.name + ", type=" + selected.Data.activeAbility.type)}");
        if (newMode == CombatMode.Ability && !HasAbility(selected))
        {
            Debug.Log("[Combat] this card has no active ability");
            return;
        }
        mode = (mode == newMode) ? CombatMode.None : newMode;
        UpdateGlow();
        Debug.Log($"[Combat] mode: {mode}");
    }

    private bool HasAbility(CardObject card)
    {
        return card.Data.activeAbility != null && card.Data.activeAbility.type != AbilityType.None;
    }
    private void HandleCardClicked(CardObject card)
    {
        if (!card.isPlaced)
        {
            return;
        }

        if (turnManager.IsGameOver)
        {
            return;
        }

        if (!turnManager.IsTurnPhase) return;

        Player current = turnManager.CurrentPlayer;

        if (selected == null)
        {
            if (card.Owner == current) Select(card);
            return;
        }
        
        if (selected == card)
        {
            if (mode == CombatMode.Ability && TryUseAbility(selected, card)) FinishAction();
            else Deselect();
            return;
        }

        switch (mode)
        {
            case CombatMode.Attack:
            Attack(selected, card);
            FinishAction();
            break;

            case CombatMode.Heal:
            if (card.Owner != selected.Owner) return;
            if (Heal(selected, card)) FinishAction();
            break;

            case CombatMode.Ability:
            if (TryUseAbility(selected, card)) FinishAction();
            break;
            

            default:
            if (card.Owner == current) Select(card);;
            break;
        }
    }

    private void HandleTokenClicked(PlayerToken token)
    {
        if (turnManager.IsGameOver) { Debug.Log("[Token] blocked: game is over"); return; }
        if (!turnManager.IsTurnPhase) { Debug.Log($"[Token] blocked: not a Turn phase ({turnManager.CurrentPhase})"); return; }
        if (selected == null) { Debug.Log("[Token] blocked: no card selected"); return; }
        if (mode != CombatMode.Attack) { Debug.Log($"[Token] blocked: mode is {mode}, press A first"); return; }
        if (token.Owner == selected.Owner) { Debug.Log($"[Token] blocked: that token belongs to the same player as the selected card ({token.Owner})"); return; }
        
        if (turnManager.IsGameOver || !turnManager.IsTurnPhase)
        {
            return;
        }
        if (selected == null || mode != CombatMode.Attack)
        {
            return;
        }
        if (token.Owner == selected.Owner)
        {
            return;
        }

        if (!boardManager.IsLaneOpen(token.Owner, selected.Lane))
        {
            Debug.Log($"[Combat] lane {selected.Lane} is blocked, can't reach the token");
            return;
        }

        Debug.Log($"{selected.Data.cardName} attacks the {token.Owner} token for {selected.CurrentDamage}");
        token.TakeDamage(selected.CurrentDamage);
        FinishAction();
    }

    private void HandleTokenDefeated(PlayerToken token)
    {
        Player winner = token.Owner == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;
        turnManager.EndGame(winner);
    }

    private void FinishAction()
    {
        Deselect();
        turnManager.NotifyActed();
    }
    private void Select(CardObject card)
    {
        if (selected != null) selected.SetSelected(false);
        selected = card;
        mode = CombatMode.None;
        card.SetSelected(true);
        UpdateGlow();
    }

    private Color ColorForMode()
    {
        switch (mode)
        {
            case CombatMode.Attack: return attackColor;
            case CombatMode.Heal: return healColor;
            case CombatMode.Ability: return abilityColor;
            default: return neutralColor;
        }
    }

    private void UpdateGlow()
    {
        if (selected != null)
        {
            selected.SetGlowColor(ColorForMode());
        }
    }

    private void Deselect()
    {
        if (selected != null)
        {
            selected.SetSelected(false);
        }
        selected = null;
        mode = CombatMode.None;
    }
    
    private void Attack(CardObject attacker, CardObject target)
    {
        int damage = attacker.CurrentDamage;
        StatusEffect thorns = target.GetEffect(AbilityType.Thorns);
        Debug.Log($"[Thorns] target has thorns: {thorns != null}");

        Debug.Log($"{attacker.Data.cardName} attacks {target.Data.cardName} for {damage}");
        target.TakeDamage(damage);

        if (thorns != null)
        {
            Debug.Log($"{attacker.Data.cardName} is hurt by thorns for {thorns.Value}");
            attacker.TakeDamage(thorns.Value);
        }
    }

    private bool Heal(CardObject healer, CardObject target)
    {
        int amount = healer.CurrentHealthAttack;
        if (amount <= 0)
        {
            return false;
        }
        Debug.Log($"{healer.Data.cardName} heals {target.Data.cardName} for {amount}");
        target.Heal(amount);
        return true;
    }

    private bool TryUseAbility(CardObject user, CardObject target)
    {
        CardAbility ability = user.Data.activeAbility;
        if (ability == null || ability.type == AbilityType.None)
        {
            return false;
        }

        if (ability.TargetAllies)
        {
            if (target.Owner != user.Owner) return false;
        }
        else
        {
            if (target.Owner == user.Owner) return false;
        }

        target.AddEffect(ability.type, ability.value, ability.duration);
        Debug.Log($"{user.Data.cardName} uses {ability.type} on {target.Data.cardName} ({ability.value} for {ability.duration} turns)");
        return true;
    }
    
    private void HandleCardDied(CardObject card)
    {
        if (selected == card)
        {
            Deselect();
        }
        Destroy(card.gameObject);
    }
}
