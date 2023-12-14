using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiResizer : MonoBehaviour
{
    [SerializeField] private bool scaleWidth;
    [SerializeField] private bool scaleHeight;
    private void OnEnable()
    {
        Resize();
    }

    private void Start()
    {
        Resize();
    }

    private void Resize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        float width = 0;
        float height = 0;

        foreach (RectTransform child in rectTransform)
        {
            width += child.rect.width;
            height += child.rect.height;
        }
        
        width = scaleWidth ? Math.Max(width, rectTransform.rect.width) : rectTransform.rect.width;
        height = scaleHeight ? Math.Max(height, rectTransform.rect.height) : rectTransform.rect.height;
        
        rectTransform.sizeDelta = new Vector2(width, height);
        
        rectTransform.localPosition = new Vector3(width/2, -height/2, 0);
    }
}
