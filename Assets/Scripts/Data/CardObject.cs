using System;
using CardgleGame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

[RequireComponent(typeof(CardView))]
public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Hover / movement")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverLift = 60f;
    [SerializeField] private float moveSpeed = 12f;

    public Card Data { get; private set;}
    public int CurrentHealth { get; private set;}
    public int CurrentDamage { get; private set;}
    public int CurrentSpeed { get; private set;}

    public bool isHovered { get; private set;}
    public bool isDragging { get; private set;}

    public event Action<CardObject> Clicked;
    public event Action<CardObject> DragStarted;
    public event Action<CardObject, PointerEventData> DragEnded;
    public event Action<CardObject> Died;

    private CardView view;
    private RectTransform rectTransform;

    private Vector3 homePosition;
    private Quaternion homeRotation = Quaternion.identity;
    private int homeSiblingIndex;

    private void Awake()
    {
        view = GetComponent<CardView>();
        rectTransform = (RectTransform)transform;
        homePosition = transform.localPosition;
    }

    public void Intialize(Card data)
    {
        Data = data;
        CurrentHealth = data.health;
        CurrentDamage = data.damage;
        CurrentSpeed = data.speed;
        view.Display(data);
    }

    public void SetHome(Vector3 localPosition, Quaternion localRotation)
    {
        homePosition = localPosition;
        homeRotation = localRotation;
        homeSiblingIndex = transform.GetSiblingIndex();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        if (CurrentHealth == 0) Died?.Invoke(this);
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(Data.health, CurrentHealth + amount);
    }

    private void Update()
    {
        if (isDragging) return;

        Vector3 targetPos = homePosition;
        Quaternion targetRot = homeRotation;
        Vector3 targetScale = Vector3.one;

        if (isHovered)
        {
            targetPos += Vector3.up * hoverLift;
            targetRot = Quaternion.identity;
            targetScale = Vector3.one * hoverScale;
        }

        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, t);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, t);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;
        isHovered = true;
        homeSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;
        isHovered = false;
        transform.SetSiblingIndex(homeSiblingIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isHovered = false;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * hoverScale;
        transform.SetAsLastSibling();
        DragStarted?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            transform.position = worldPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        transform.SetSiblingIndex(homeSiblingIndex);
        DragEnded?.Invoke(this, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Clicked?.Invoke(this);
    }
}