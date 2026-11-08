using CardgleGame;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PileView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text pileCount;
    [SerializeField] private TMP_Text pileType;
    [SerializeField] private Image artwork;

    private RectTransform rect;

    private void Awake()
    {
        rect = (RectTransform)transform;
    }

    public bool Contains(Vector2 screenPoint, Camera cam)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, cam);
    }

    public void Display(Pile pile)
    {
        pileCount.text = pile.cardCount.ToString();
        pileType.text = pile.pileType;
    }

    public event Action Clicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[Pile] clicked {name}");
        Clicked?.Invoke();
    }
}