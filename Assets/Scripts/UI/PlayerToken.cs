using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PlayerToken : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Player owner;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private TMP_Text healthText;

    public Player Owner => owner;
    public int CurrentHealth { get; private set; }

    public event Action<PlayerToken> Clicked;
    public event Action<PlayerToken> Defeated;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        UpdateText();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        UpdateText();
        if (CurrentHealth == 0)
        {
            Defeated?.Invoke(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

    private void UpdateText()
    {
        if (healthText != null)
        {
            healthText.text = CurrentHealth.ToString();
        }
    }
}
