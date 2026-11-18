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
    [SerializeField] private TMP_Text healText;
    [SerializeField] private TMP_Text abilityText;
    [SerializeField] private TMP_Text statusText;

    public void Display(Card card)
    {
        nameText.text = card.cardName;
        healthText.text = card.health.ToString();
        damageText.text = card.damage.ToString();
        costText.text = card.cost.ToString();
        artwork.sprite = card.cardArt;

        if (healText != null)
        {
            healText.text = card.healthAttack > 0 ? "+" + card.healthAttack : "";
        }

        if (abilityText != null)
        {
            abilityText.text = card.activeAbility != null ? card.activeAbility.abilityName : "";
        }
    }

    public void Refresh(int health, int damage, int healAmount)
    {
        healthText.text = health.ToString();
        damageText.text = damage.ToString();

        if (healText != null)
        {
            healText.text = healAmount > 0 ? "+" + healAmount : "";
        }
    }
}
