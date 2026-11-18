using UnityEngine;
using TMPro;
using System;
using Unity.Hierarchy.Editor;
using Unity.Tutorials.Editor;
using UnityEngine.WSA;

public enum Player { PlayerOne, PlayerTwo }
public enum TurnPhase { PlayerOnePlace, PlayerTwoPlace, PlayerOneTurn, PlayerTwoTurn}
public class TurnManager : MonoBehaviour
{
    [Header("UI (optional)")]
    [SerializeField] private TMP_Text phaseLabel;
    [SerializeField] private string winText = "wins!";
    [SerializeField] private Color playerOneColor = new Color32(0x51, 0x5A, 0xBA, 255);
    [SerializeField] private Color playerTwoColor = new Color32(0xF8, 0x8A, 0x16, 255);

    [Header("Rules")]
    [SerializeField] private int actionsPerTurn = 1;

    public TurnPhase CurrentPhase { get; private set; }
    public bool IsGameOver { get; private set; }
    private Player winner;
    public Player CurrentPlayer => (CurrentPhase == TurnPhase.PlayerOnePlace || CurrentPhase == TurnPhase.PlayerOneTurn) ? Player.PlayerOne : Player.PlayerTwo;
    public bool IsPlacePhase => CurrentPhase == TurnPhase.PlayerOnePlace || CurrentPhase == TurnPhase.PlayerTwoPlace;
    public bool IsTurnPhase => !IsPlacePhase;

    public event Action<TurnPhase> PhaseChanged;
    public event Action<Player> GameOver;
    private int placedThisPhase;
    private int actionsLeft;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartPhase(TurnPhase.PlayerOnePlace);
    }

    public bool CanPlace(Player slotOwner)
    {
        return !IsGameOver && CurrentPlayer == slotOwner && (IsPlacePhase || actionsLeft > 0);
    }

    public void NotifyPlaced()
    {
        if (IsGameOver)
        {
            return;
        }
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
        if (IsGameOver)
        {
            return;
        }
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
        if (IsGameOver)
        {
            return;
        }
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

    public void EndGame(Player winningPlayer)
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        winner = winningPlayer;
        UpdateLabel();
        Debug.Log($"[Turn] game over: {winningPlayer} wins");
        GameOver?.Invoke(winningPlayer);
    }

    public void ResetGame()
    {
        IsGameOver = false;
        StartPhase(TurnPhase.PlayerOnePlace);
    }

    private void UpdateLabel()
    {
        if (phaseLabel == null)
        {
            return;
        }

        Player shown = IsGameOver ? winner : CurrentPlayer;

        Color color = shown == Player.PlayerOne ? playerOneColor : playerTwoColor;
        string hex = ColorUtility.ToHtmlStringRGB(color);
        
        string who = shown == Player.PlayerOne ? "Player 1" : "Player 2";

        if (IsGameOver)
        {
            phaseLabel.text = $"<color=#{hex}>{who}</color> - {winText}";
            return;
        }
        string what = IsPlacePhase ? $"Place ({placedThisPhase} placed)" : $"Turn ({actionsLeft} action left)";
        phaseLabel.text = $"<color=#{hex}>{who}</color> - {what}";
    }
}
