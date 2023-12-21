using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiResizer : MonoBehaviour
{
    private GridLayoutGroup _gridLayoutGroup;
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    private VerticalLayoutGroup _verticalLayoutGroup;
    
    private RectTransform _rectTransform;
    
    private void OnEnable()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void Resize()
    {
        float width = 0;
        float height = 0;

        if (_gridLayoutGroup != null)
        {
            width = (_gridLayoutGroup.cellSize.x+_gridLayoutGroup.spacing.x) * _gridLayoutGroup.constraintCount;
            height = (_gridLayoutGroup.cellSize.y+_gridLayoutGroup.spacing.y) * Mathf.CeilToInt(_rectTransform.childCount / (float)_gridLayoutGroup.constraintCount);
        }
        else if (_horizontalLayoutGroup != null)
        {
            foreach (RectTransform child in _rectTransform)
                width += child.rect.width + _horizontalLayoutGroup.spacing;

            width = Math.Max(width, _rectTransform.rect.width);
            height = _rectTransform.rect.height;
        }
        else if (_verticalLayoutGroup != null)
        {
            foreach (RectTransform child in _rectTransform)
                height += child.rect.height + _verticalLayoutGroup.spacing;

            width = _rectTransform.rect.width;
            height = Math.Max(height, _rectTransform.rect.height);
        }
        
        _rectTransform.sizeDelta = new Vector2(width, height);
        
        _rectTransform.localPosition = new Vector3(width/2, -height/2, 0);
    }
}
