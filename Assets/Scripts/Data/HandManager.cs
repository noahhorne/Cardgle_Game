using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardgleGame;
using System;
using Unity.VisualScripting;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab; // Assigning the car prefab
    public Transform handTransform; // Root of the hand
    public float fanSpread = 5f;
    public float cardSpacing = 5f;
    public List<GameObject> cardsInHand = new List<GameObject>();

    void Start()
    {
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
    }

    public void AddCardToHand()
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);
        UpdateHandVisuals();
    }

    void Update()
    {
        UpdateHandVisuals();
    }
    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1)/2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = i * cardSpacing;
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, 0f, 0f);
        }
    }
}
