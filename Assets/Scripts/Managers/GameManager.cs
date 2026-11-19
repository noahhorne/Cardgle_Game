using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;

    // ADDED: Reference to the countdown text.
    [SerializeField] private TMP_Text countdownText;

    private bool gameEnded = false;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.GameOver += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.GameOver -= HandleGameOver;
        }
    }

    private void Start()
    {
        countdownText.gameObject.SetActive(false);
    }

    private void HandleGameOver(Player winner)
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        Debug.Log("Game over! Restarting in 10 seconds.");

        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        countdownText.gameObject.SetActive(true);

        for (int seconds = 10; seconds > 0; seconds--)
        {
            countdownText.text = "Restarting in " + seconds + "...";

            yield return new WaitForSecondsRealtime(1f);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
