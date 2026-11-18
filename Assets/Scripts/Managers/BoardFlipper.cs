using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.Tilemaps;

public class BoardFlipper : MonoBehaviour
{

    [SerializeField] private TurnManager turnManager;
    [SerializeField] private List<RectTransform> flipTargets;
    [SerializeField] private Player playerAtTheBottom = Player.PlayerOne;
    [SerializeField] private float flipDuration = 0.4f;
    private Vector2[] basePositions;
    private Vector2[] fromPositions;
    private Vector2[] toPositions;
    private float axisY;
    private bool flipped;
    private bool initialized;
    private float progress = 1f;

    private void OnEnable()
    {
        turnManager.PhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        turnManager.PhaseChanged -= HandlePhaseChanged;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        int count = flipTargets.Count;
        basePositions = new Vector2[count];
        fromPositions = new Vector2[count];
        toPositions = new Vector2[count];

        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (int i = 0; i < count; i++)
        {
            basePositions[i] = flipTargets[i].anchoredPosition;
            minY = Mathf.Min(minY, basePositions[i].y);
            maxY = Mathf.Max(maxY, basePositions[i].y);
        }

        axisY = (minY + maxY) / 2f;
        initialized = true;

        ApplyFlip(animate: false);
    }

    private void HandlePhaseChanged(TurnPhase phase)
    {
        if (!initialized)
        {
            return;
        }
        ApplyFlip(animate: true);
    }

    private void ApplyFlip(bool animate)
    {
        bool shouldFlip = turnManager.CurrentPlayer != playerAtTheBottom;
        if (animate && shouldFlip == flipped)
        {
            return;
        }

        flipped = shouldFlip;

        for (int i = 0; i < flipTargets.Count; i++)
        {
            if (flipTargets[i] == null)
            {
                continue;
            }

            Vector2 basePos = basePositions[i];
            fromPositions[i] = flipTargets[i].anchoredPosition;
            toPositions[i] = flipped ? new Vector2(basePos.x, 2f * axisY - basePos.y) : basePos;
        }

        if (!animate || flipDuration <= 0f)
        {
            progress = 1f;
            SetPositions(1f);
        }
        else
        {
            progress = 0f;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (progress >= 1f)
        {
            return;
        }
        progress = Mathf.Min(1f, progress + Time.deltaTime / flipDuration);
        SetPositions(Mathf.SmoothStep(0f, 1f, progress));
    }

    private void SetPositions(float t)  
    {
        for (int i = 0; i < flipTargets.Count; i++)
        {
            if (flipTargets[i] == null)
            {
                continue;
            }
            flipTargets[i].anchoredPosition = Vector2.Lerp(fromPositions[i], toPositions[i], t);
        }
    }
}
