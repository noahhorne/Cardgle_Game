using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardSlot : MonoBehaviour
{
    [SerializeField] private Player owner;
    public Player Owner => owner; 
    public CardObject Occupant {get; private set;}
    public bool isEmpty => Occupant == null;

    private RectTransform rect;

    private void Awake()
    {
        rect = (RectTransform)transform;
    }

    public bool Contains(Vector2 screenPoint, Camera cam)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, cam);
    }

    public void Place(CardObject card)
    {
        Occupant = card;
        card.SetOwner(owner);
        card.PlaceInSlot(rect);
    }

    public void Clear()
    {
        Occupant = null;
    }
}
