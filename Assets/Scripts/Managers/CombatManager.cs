using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using Unity.VisualScripting;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CombatManager : MonoBehaviour
{
    private enum CombatMode {None, Attack, Heal}
    
    [Header("Glow colours")] 
     [SerializeField] private Color neutralColor = new Color(1f, 0.9f, 0.3f, 1f);
     [SerializeField] private Color attackColor = new Color(1f, 0.25f, 0.25f, 1f);
     [SerializeField] private Color healColor = new Color(0.3f, 1f, 0.4f, 1f);
    private CardObject selected;
    private CombatMode mode = CombatMode.None;

    private void OnEnable()
    {
        CardObject.Spawned += HandleCardSpawned;
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

    private void Update()
    {
        if (selected == null)
        {
            return;
        }

        if (KeyPressed(KeyCode.A))
        {
            SetMode(CombatMode.Attack);
        } else if (KeyPressed(KeyCode.H)){
            SetMode(CombatMode.Heal);
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
            case KeyCode.Escape: return kb.escapeKey.wasPressedThisFrame;
        }
        return false;
        #else
        return Input.GetKeyDown(key);
        #endif
    }

    private void SetMode(CombatMode newMode)
    {
        mode = (mode == newMode) ? CombatMode.None : newMode;
        UpdateGlow();
        Debug.Log($"[Combat] mode: {mode}");
    }
    private void HandleCardClicked(CardObject card)
    {
        if (!card.isPlaced)
        {
            return;
        }

        if (selected == null)
        {
            Select(card);
            return;
        }
        
        if (selected == card)
        {
            Deselect();
            return;
        }

        switch (mode)
        {
            case CombatMode.Attack:
            Attack(selected, card);
            Deselect();
            break;

            case CombatMode.Heal:
            Heal(selected, card);
            Deselect();
            break;

            default:
            Select(card);
            break;
        }
    }

    private void Select(CardObject card)
    {
        selected = card;
        card.SetSelected(true);
    }

    private Color ColorForMode()
    {
        switch (mode)
        {
            case CombatMode.Attack: return attackColor;
            case CombatMode.Heal: return healColor;
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
        Debug.Log($"{attacker.Data.cardName} attacks {target.Data.cardName} for {damage}");
        target.TakeDamage(damage);
    }

    private void Heal(CardObject healer, CardObject target)
    {
        int amount = healer.CurrentHealthAttack;
        if (amount <= 0)
        {
            return;
        }
        Debug.Log($"{healer.Data.cardName} heals {target.Data.cardName} for {amount}");
        target.Heal(amount);
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
