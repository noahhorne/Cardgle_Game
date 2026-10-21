using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardgleGame;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Internal;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public Image[] typeImages;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        
    }
}
