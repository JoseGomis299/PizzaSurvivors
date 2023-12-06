using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSelector : MonoBehaviour
{
    [SerializeField] private Paintable sauceCanvas;
    [SerializeField] private Paintable cheeseCanvas;

    private void Awake()
    {
        cheeseCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cheeseCanvas.enabled = true;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            sauceCanvas.enabled = true;
            cheeseCanvas.enabled = false;
        }
    }
}
