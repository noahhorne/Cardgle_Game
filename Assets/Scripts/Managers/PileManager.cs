using UnityEngine;
using System.Collections.Generic;
using CardgleGame;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.Design;
using System.Security.Cryptography;

public class PileManager : MonoBehaviour
{
    [SerializeField] private PileView drawPileView;
    [SerializeField] private PileView discardPileView;
    [SerializeField] private HandView handView;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private List<Card> startingCards;
    [SerializeField] private int openingHandSize = 6; 
    [SerializeField] private int maxHandSizeToDraw = 6;

    private class PlayerPiles 
    {
        public Pile draw;
        public Pile hand;
        public Pile discard;
    }

    private PlayerPiles Active => piles[turnManager.CurrentPlayer];
    private Dictionary<Player, PlayerPiles> piles;
    private void Awake()
    {
        piles = new Dictionary<Player, PlayerPiles>
        {
            { Player.PlayerOne, CreatePlayerPiles() },
            { Player.PlayerTwo, CreatePlayerPiles() }
        };
    }

    private PlayerPiles CreatePlayerPiles()
    {
        return new PlayerPiles
        {
            draw = CreatePile("Draw"),
            hand = CreatePile("Hand"),
            discard = CreatePile("Discard")
        };
    }

    private Pile CreatePile(string type)
    {
        Pile pile = ScriptableObject.CreateInstance<Pile>();
        pile.pileType = type;
        pile.cards = new List<Card>();
        return pile;
    }
    private void OnEnable()
    {
        drawPileView.Clicked += TryDrawCard;
        turnManager.PhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        drawPileView.Clicked -= TryDrawCard;
        turnManager.PhaseChanged += HandlePhaseChanged;
    }

    private void HandlePhaseChanged(TurnPhase phase)
    {
        RefreshPileViews();
    }

    private void Start()
    {
        foreach (PlayerPiles p in piles.Values)
        {
            p.draw.cards.AddRange(startingCards);
            Shuffle(p.draw);

            for (int i = 0; i < openingHandSize; i++)
            {
                DrawInto(p);
            }
        }

        RefreshPileViews();
    }

    private void TryDrawCard()
    {
        if (Active.hand.cardCount <= maxHandSizeToDraw)
        {
            DrawCard();
        }
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

    private bool DrawInto(PlayerPiles p)
    {
        if (p.draw.cardCount == 0)
        {
            return false;
        }

        Card drawnCard = p.draw.cards[0];
        p.draw.cards.RemoveAt(0);
        p.hand.cards.Add(drawnCard);
        return true;
    }


    public void DrawCard()
    {
        if (DrawInto(Active))
        {
            RefreshPileViews();
        }
    }

    public void PlayCard(Card card)
    {
        if (Active.hand.cards.Remove(card))
        {
            RefreshPileViews();
        }
    }

    public void DiscardCard(Card card)
    {
        PlayerPiles p = Active;
        if (p.hand.cards.Contains(card))
        {
            p.hand.cards.Remove(card);
            p.discard.cards.Add(card);
            RefreshPileViews();
        }
    }

    public void ClearPile(Pile pile) 
    {
        pile.cards.Clear();
        RefreshPileViews();
    }
    
    public void PopulateDrawPileFromDiscard()
    {
        PlayerPiles p = Active;
        p.draw.cards.AddRange(p.discard.cards);
        p.discard.cards.Clear();
        RefreshPileViews();
    }

    public void ShuffleDrawPile()
    {
        Shuffle(Active.draw);
        RefreshPileViews();
    }

    private void Shuffle(Pile pile)
    {
        for (int i = 0; i < pile.cardCount; i++)
        {
            Card temp = pile.cards[i];
            int randomIndex = Random.Range(i, pile.cardCount);
            pile.cards[i] = pile.cards[randomIndex];
            pile.cards[randomIndex] = temp;
        }
    }
    
    private void RefreshPileViews()
    {
        PlayerPiles p = Active;
        drawPileView.Display(p.draw);
        discardPileView.Display(p.discard);
        handView.Display(p.hand);
    }

}
