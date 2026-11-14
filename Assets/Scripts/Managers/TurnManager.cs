using UnityEngine;
using TMPro;
using System;
using Unity.Hierarchy.Editor;
using Unity.Tutorials.Editor;

public enum Player { PlayerOne, PlayerTwo }
public enum TurnPhase { PlayerOnePlace, PlayerTwoPlace, PlayerOneTurn, PlayerTwoTurn}
public class TurnManager : MonoBehaviour
{
    [Header("UI (optional)")]
    [SerializeField] private TMP_Text phaseLabel;
    [SerializeField] private Color playerOneColor = new Color32(0x51, 0x5A, 0xBA, 255);
    [SerializeField] private Color playerTwoColor = new Color32(0xF8, 0x8A, 0x16, 255);

    [Header("Rules")]
    [SerializeField] private int actionsPerTurn = 1;

    public TurnPhase CurrentPhase { get; private set; }

    public Player CurrentPlayer => (CurrentPhase == TurnPhase.PlayerOnePlace || CurrentPhase == TurnPhase.PlayerOneTurn) ? Player.PlayerOne : Player.PlayerTwo;
    public bool IsPlacePhase => CurrentPhase == TurnPhase.PlayerOnePlace || CurrentPhase == TurnPhase.PlayerTwoPlace;
    public bool IsTurnPhase => !IsPlacePhase;

    public event Action<TurnPhase> PhaseChanged;
    private int placedThisPhase;
    private int actionsLeft;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartPhase(TurnPhase.PlayerOnePlace);
    }

    public bool CanPlace(Player slotOwner)
    {
        return CurrentPlayer == slotOwner && (IsPlacePhase || actionsLeft > 0);
    }

    public void NotifyPlaced()
    {
        if (IsPlacePhase)
        {
            placedThisPhase++;
            UpdateLabel();
        }
        else
        {
            NotifyActed();
        }
    }

    public void NotifyActed()
    {
        actionsLeft--;
        if (actionsLeft <= 0)
        {
            EndPhase();
        } else
        {
            UpdateLabel();
        }
    }

    public void EndPhase()
    {
        StartPhase(NextPhase(CurrentPhase));
    }

    private TurnPhase NextPhase(TurnPhase phase)
    {
        switch (phase)
        {
            case TurnPhase.PlayerOnePlace: return TurnPhase.PlayerTwoPlace;
            case TurnPhase.PlayerTwoPlace: return TurnPhase.PlayerOneTurn;
            case TurnPhase.PlayerOneTurn: return TurnPhase.PlayerTwoTurn;
            default: return TurnPhase.PlayerOneTurn;
        }
    }

    private void StartPhase(TurnPhase phase)
    {
        CurrentPhase = phase;
        placedThisPhase = 0;
        actionsLeft = IsTurnPhase ? actionsPerTurn : 0;

        UpdateLabel();
        Debug.Log($"[Turn] {phase}");
        PhaseChanged?.Invoke(phase);
    }

    private void UpdateLabel()
    {
        if (phaseLabel == null)
        {
            return;
        }

        Color color = CurrentPlayer == Player.PlayerOne ? playerOneColor : playerTwoColor;
        string hex = ColorUtility.ToHtmlStringRGB(color);
        
        string who = CurrentPlayer == Player.PlayerOne ? "Player 1" : "Player 2";
        string what = IsPlacePhase ? $"Place ({placedThisPhase} placed)" : $"Turn ({actionsLeft} action left)";
        phaseLabel.text = $"<color=#{hex}>{who}</color> - {what}";
    }
}
