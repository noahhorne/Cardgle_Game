using UnityEngine;
using UnityEngine.UI;

public class DottedBorderGraphic : MaskableGraphic
{
    [SerializeField] private float dotSize = 4f;
    [SerializeField] private float dotSpacing = 10f;

    public float dotSizeValue
    {
        get => dotSize;
        set
        {
            dotSize = Mathf.Max(1f, value);
            SetVerticesDirty();
        }
    }

    public float dotSpacingValue
    {
        get => dotSpacing;
        set
        {
            dotSpacing = Mathf.Max(dotSize, value);
            SetVerticesDirty();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        raycastTarget = false;
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        float radius = Mathf.Max(1f, dotSize * 0.5f);
        float step = Mathf.Max(dotSize, dotSpacing);
        Rect rect = GetPixelAdjustedRect();
        float left = rect.xMin + radius;
        float right = rect.xMax - radius;
        float bottom = rect.yMin + radius;
        float top = rect.yMax - radius;

        AddHorizontalDots(vertexHelper, left, right, bottom, step, radius);
        AddHorizontalDots(vertexHelper, left, right, top, step, radius);
        AddVerticalDots(vertexHelper, bottom + step, top - step, left, step, radius);
        AddVerticalDots(vertexHelper, bottom + step, top - step, right, step, radius);
    }

    private void AddHorizontalDots(VertexHelper vertexHelper, float left, float right, float y, float step, float radius)
    {
        int count = Mathf.Max(1, Mathf.FloorToInt((right - left) / step) + 1);
        for (int i = 0; i < count; i++)
        {
            float x = count == 1 ? (left + right) * 0.5f : Mathf.Lerp(left, right, (float)i / (count - 1));
            AddDot(vertexHelper, new Vector2(x, y), radius);
        }
    }

    private void AddVerticalDots(VertexHelper vertexHelper, float start, float end, float x, float step, float radius)
    {
        if (end < start) return;

        int count = Mathf.Max(1, Mathf.FloorToInt((end - start) / step) + 1);
        for (int i = 0; i < count; i++)
        {
            float y = count == 1 ? (start + end) * 0.5f : Mathf.Lerp(start, end, (float)i / (count - 1));
            AddDot(vertexHelper, new Vector2(x, y), radius);
        }
    }

    private void AddDot(VertexHelper vertexHelper, Vector2 center, float radius)
    {
        const int segments = 8;
        int firstVertex = vertexHelper.currentVertCount;
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = center;
        vertexHelper.AddVert(vertex);

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            vertex.position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            vertexHelper.AddVert(vertex);
        }

        for (int i = 0; i < segments; i++)
        {
            vertexHelper.AddTriangle(firstVertex, firstVertex + 1 + i, firstVertex + 1 + (i + 1) % segments);
        }
    }
}
