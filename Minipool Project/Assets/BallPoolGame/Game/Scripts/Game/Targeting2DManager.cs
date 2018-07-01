using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BallPool;

/// <summary>
/// Manager for targeting on cue ball in 2D mode.
/// </summary>
public class Targeting2DManager : MonoBehaviour, IPointerDownHandler
{
    public static bool isSelected = false;
    [SerializeField] private RectTransform point;
    [SerializeField] private ShotController shotController;
    private RectTransform rectTransform;
    private float radius;
    private float currentRadius;
    private Vector3 localPosition;
    private Vector3 checkLocalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        radius = 0.5f * (rectTransform.sizeDelta.x - point.sizeDelta.x);
    }
    void OnEnable()
    {
        localPosition = point.localPosition;
        InputOutput.OnMouseState += InputOutput_OnMouseState;
    }
    void OnDisable()
    {
        InputOutput.OnMouseState -= InputOutput_OnMouseState;
    }
    void InputOutput_OnMouseState (MouseState mouseState)
    {
        if (isSelected && mouseState == MouseState.Up)
        {
            isSelected = false;
            shotController.RessetCueAfterTargeting();
        }
        if (isSelected && mouseState == MouseState.PressAndMove)
        {
            localPosition -= 0.01f * InputOutput.mouseScreenSpeed;
            currentRadius = Mathf.Sqrt(localPosition.x * localPosition.x + localPosition.y * localPosition.y);
            if (currentRadius < radius)
            {
                checkLocalPosition = localPosition;
                point.localPosition = localPosition;
            }
            else
            {
                localPosition = checkLocalPosition;
            }
            SetCuePosition(-localPosition / radius);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
        shotController.RessetCueForTargeting();
    }
    private void SetCuePosition(Vector3 normalizedPosition)
    {
        shotController.SetCueTargetingPosition(normalizedPosition);
    }
    public void Resset()
    {
        point.localPosition = localPosition = Vector3.zero;
        isSelected = false;
    }
    public void SetPointTargetingPosition(Vector3 normalizedPosition)
    {
        point.localPosition = normalizedPosition * radius;
    }
}
