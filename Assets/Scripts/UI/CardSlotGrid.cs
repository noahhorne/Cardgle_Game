using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardSlotGrid : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Vector2 spacing = new Vector2(24f, 24f);
    [SerializeField] private Color borderColor = new Color(1f, 1f, 1f, 0.7f);
    [SerializeField] private float dotSize = 4f;
    [SerializeField] private float dotSpacing = 10f;

    private const int Columns = 3;
    private const int Rows = 2;
    private const float CardAspectRatio = 300f / 420f;

    private void Start()
    {
        if (boardManager == null)
        {
            Debug.LogError("Assign a BoardManager to the CardSlotGrid component.", this);
            enabled = false;
            return;
        }

        RectTransform rect = (RectTransform)transform;
        float maxCellWidth = (rect.rect.width - spacing.x * (Columns - 1)) / Columns;
        float maxCellHeight = (rect.rect.height - spacing.y * (Rows - 1)) / Rows;
        float cellWidth = Mathf.Min(maxCellWidth, maxCellHeight * CardAspectRatio);
        float cellHeight = cellWidth / CardAspectRatio;
        if (cellWidth <= 0f || cellHeight <= 0f)
        {
            Debug.LogError("The CardSlotGrid RectTransform must be large enough for its spacing and card aspect ratio.", this);
            enabled = false;
            return;
        }

        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
        if (layout == null)
        {
            layout = gameObject.AddComponent<GridLayoutGroup>();
        }

        layout.cellSize = new Vector2(cellWidth, cellHeight);
        layout.spacing = spacing;
        layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis = GridLayoutGroup.Axis.Horizontal;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = Columns;

        List<CardSlot> slots = new List<CardSlot>(Columns * Rows);
        for (int i = 0; i < Columns * Rows; i++)
        {
            GameObject slotObject = new GameObject(
                $"CardSlot_{i + 1}",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(DottedBorderGraphic),
                typeof(CardSlot));
            slotObject.transform.SetParent(transform, false);

            DottedBorderGraphic border = slotObject.GetComponent<DottedBorderGraphic>();
            border.color = borderColor;
            border.dotSizeValue = dotSize;
            border.dotSpacingValue = dotSpacing;

            slots.Add(slotObject.GetComponent<CardSlot>());
        }

        boardManager.SetSlots(slots);
    }
}
