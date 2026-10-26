using CardgleGame;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HandView : MonoBehaviour
{
    [SerializeField] private TMP_Text pileType;
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;
    [SerializeField] private float fanSpread = 5f;
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
            
            CardObject cardObject = cardGO.GetComponent<CardObject>();
            if (cardObject == null)
            {
                Debug.LogError("The card prefab must have a CardObject component.", cardGO);
                continue;
            }

            Vector3 pos = new Vector3(minX + i * spacing, 0f, 0f);
            Quaternion rot = Quaternion.Euler(0f, 0f, -fanSpread * (i - (hand.cardCount - 1) / 2f));

            cardGO.transform.localPosition = pos;
            cardGO.transform.localRotation = rot;

            cardObject.Intialize(hand.cards[i]);
            cardObject.SetHome(pos, rot);
        }
    }
}