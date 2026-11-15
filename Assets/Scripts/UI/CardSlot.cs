using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardSlot : MonoBehaviour
{
    [SerializeField] private Player owner;
    public Player Owner => owner; 

    [SerializeField] private int lane;
    public int Lane => lane;
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
        card.SetSlot(this);
        card.SetOwner(owner);
        card.PlaceInSlot(rect);
    }

    public void Clear()
    {
        Occupant = null;
    }
}
