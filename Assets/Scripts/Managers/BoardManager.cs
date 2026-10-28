using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private List<CardSlot> slots;
    [SerializeField] private PileManager pileManager;

    private void OnEnable()
    {
        CardObject.Spawned += HandleCardSpawned;
    }

    private void OnDisable()
    {
        CardObject.Spawned -= HandleCardSpawned;
    }

    private void HandleCardSpawned(CardObject card)
    {
        card.DragEnded += HandleDragEnded;
    }

    public void SetSlots(List<CardSlot> newSlots)
    {
        if (newSlots == null)
        {
            throw new System.ArgumentNullException(nameof(newSlots));
        }

        slots = newSlots;
    }
    
    private void HandleDragEnded(CardObject card, PointerEventData eventData)
    {
        CardSlot slot = FindSlotAt(eventData.position, eventData.pressEventCamera);

        if (slot == null || !slot.isEmpty) return;

        slot.Place(card);
        pileManager.PlayCard(card.Data);
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
