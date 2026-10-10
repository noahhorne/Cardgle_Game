using CardgleGame;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandView : MonoBehaviour
{
    [SerializeField] private TMP_Text pileType;
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;


    public void Display(Pile hand)
    {

        // Clear existing cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // Display cards in hand
        float spacing = (maxX - minX) / Mathf.Max(hand.cardCount - 1, 1);
        for (int i = 0; i < hand.cardCount; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardContainer);
            cardGO.transform.localPosition = new Vector3(minX + i * spacing, 0f, 0f);

            CardView cardView = cardGO.GetComponent<CardView>();
            if (cardView == null)
            {
                Debug.LogError("The card prefab must have a CardView component.", cardGO);
                continue;
            }

            cardView.Display(hand.cards[i]);
        }

    }
}