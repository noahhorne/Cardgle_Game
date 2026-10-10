using CardgleGame;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PileView : MonoBehaviour
{
    [SerializeField] private TMP_Text pileCount;
    [SerializeField] private TMP_Text pileType;
    [SerializeField] private Image artwork;

    public void Display(Pile pile)
    {
        pileCount.text = pile.cardCount.ToString();
        pileType.text = pile.pileType;
        if (pileType.text == "Draw")
        {
            artwork.sprite = Resources.Load<Sprite>("Images/Card Art/missing_content"); // Placeholder for back of draw pile
        } else if (pileType.text == "Discard")
        {
            if (pile.cardCount > 0)
            {
                artwork.sprite = pile.cards[0].cardArt;
            } else
            {
                artwork.sprite = Resources.Load<Sprite>("Images/Card Art/missing_content"); // Placeholder for empty discard pile
            }
        }
    }
}