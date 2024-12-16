using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    public Transform targetObject;
    public Camera MiniMapCamera;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 targetPosition = targetObject.position;
        Vector3 screenPos = MiniMapCamera.WorldToScreenPoint(targetPosition);

        rectTransform.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
    }
}
