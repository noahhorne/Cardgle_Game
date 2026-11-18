using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameReset : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private PileManager pileManager;
    [SerializeField] private List<PlayerToken> tokens;

    public void Surrender()
    {
        Debug.Log($"[Game] {turnManager.CurrentPlayer} surrendered, restarting");
        ResetGame();
    }

    public void ResetGame()
    {
        foreach (PlayerToken token in tokens) token.ResetHealth();
        boardManager.ClearBoard();
        pileManager.ResetPiles();
        turnManager.ResetGame(); 
    }
}
