using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private List<CardSlot> slots;
    [SerializeField] private PileManager pileManager;
    [SerializeField] private PileView discardPileView;
    [SerializeField] private TurnManager turnManager;

    private void OnEnable()
    {
        CardObject.Spawned += HandleCardSpawned;
        Debug.Log("[Board] OnEnable");

    }

    private void OnDisable()
    {
        CardObject.Spawned -= HandleCardSpawned;
        
    }

    private void HandleCardSpawned(CardObject card)
    {
        card.DragEnded += HandleDragEnded;
        Debug.Log("[Board] card spawned");

    }
    
    private void HandleDragEnded(CardObject card, PointerEventData eventData)
    {

        if (discardPileView.Contains(eventData.position, eventData.pressEventCamera))
        {
            pileManager.DiscardCard(card.Data);
            Destroy(card.gameObject);
            return;
        }
        Debug.Log("DROP RECEIVED");
        CardSlot slot = FindSlotAt(eventData.position, eventData.pressEventCamera);

        if (slot == null || !slot.isEmpty) return;
        
        if (!turnManager.CanPlace(slot.Owner))
        {
            Debug.Log($"[Board] can't place in a {slot.Owner} slot during {turnManager.CurrentPhase}");
            return;
        }

        slot.Place(card);
        pileManager.PlayCard(card.Data);
        turnManager.NotifyPlaced();
    }

    public List<CardObject> GetCards(Player owner)
    {
        List<CardObject> result = new List<CardObject>();
        foreach (CardSlot slot in slots)
        {
            if (slot.Owner == owner && !slot.isEmpty)
            {
                result.Add(slot.Occupant);
            }
        }
        return result;
    }

    public bool IsLaneOpen(Player owner, int lane)
    {
        foreach (CardSlot slot in slots)
        {
            if (slot.Owner == owner && slot.Lane == lane)
            {
                return slot.isEmpty;
            }
        }
        return false;
    }

    private CardSlot FindSlotAt(Vector2 screenPoint, Camera cam)
    {
        foreach (CardSlot slot in slots)
        {
            if (slot.Contains(screenPoint, cam)) return slot;
        }
        return null;
    }
}
