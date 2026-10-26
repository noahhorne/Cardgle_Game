using System.Collections;
using System.Collections.Generic;
using CardgleGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image artwork;

    public void Display(Card card)
    {
        nameText.text = card.cardName;
        healthText.text = card.health.ToString();
        damageText.text = card.damage.ToString();
        costText.text = card.cost.ToString();
        artwork.sprite = card.cardArt;
    }

    public void Refresh(int health, int damage)
    {
        healthText.text = health.ToString();
        damageText.text = damage.ToString();
    }
}
