using UnityEngine;
using System.Collections.Generic;
using CardgleGame;

public class PileManager : MonoBehaviour
{
    [SerializeField] private PileView drawPileView;
    [SerializeField] private PileView discardPileView;
    [SerializeField] private HandView handView;

    [SerializeField] private List<Card> startingCards;

    private Pile drawPile;
    private Pile handPile;
    private Pile discardPile;
    
    private void Awake()
    {
        drawPile = CreatePile("Draw");
        handPile = CreatePile("Hand");
        discardPile = CreatePile("Discard");
    }

    private Pile CreatePile(string type)
    {
        Pile pile = ScriptableObject.CreateInstance<Pile>();
        pile.pileType = type;
        pile.cards = new List<Card>();
        return pile;
    }

    private void Start()
    {
        PopulateDrawPile(startingCards);
        RefreshPileViews();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card AddCard(Card card, Pile pile) {
        pile.cards.Add(card);
        return card;
    }
    
    public Card RemoveCard(Card card, Pile pile) {
        pile.cards.Remove(card);
        return card;
    }

    public void DrawCard()
    {
        if (drawPile.cardCount > 0)
        {
            Card drawnCard = drawPile.cards[0];
            drawPile.cards.RemoveAt(0);
            handPile.cards.Add(drawnCard);
            RefreshPileViews();
        }
    }

    public void DiscardCard(Card card)
    {
        if (handPile.cards.Contains(card))
        {
            handPile.cards.Remove(card);
            discardPile.cards.Add(card);
            RefreshPileViews();
        }
    }

    public void ClearPile(Pile pile) 
    {
        pile.cards.Clear();
        RefreshPileViews();
    }
    
    public void PopulateDrawPile(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            drawPile.cards.Add(cards[i]);
        }
        RefreshPileViews();
    }
    
    public void PopulateDrawPileFromDiscard()
    {
        for (int i = 0; i < discardPile.cardCount; i++)
        {
            drawPile.cards.Add(discardPile.cards[i]);
        }
        discardPile.cards.Clear();
        RefreshPileViews();
    }

    public void ShuffleDrawPile()
    {
        for (int i = 0; i < drawPile.cardCount; i++)
        {
            Card temp = drawPile.cards[i];
            int randomIndex = Random.Range(i, drawPile.cardCount);
            drawPile.cards[i] = drawPile.cards[randomIndex];
            drawPile.cards[randomIndex] = temp;
        }
        RefreshPileViews();
    }
    
        private void RefreshPileViews()
    {
        drawPileView.Display(drawPile);
        discardPileView.Display(discardPile);
        handView.Display(handPile);
    }


}
