using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectUtils.Helpers;

public class IngredientPlacer : MonoBehaviour
{
    [SerializeField] private Paintable sauceCanvas;
    [SerializeField] private Paintable cheeseCanvas;
    private Paintable _currentCanvas;

    [SerializeField] private float borderLength = 0.35f;
    private Ingredient _currentIngredient;

    public GameObject CurrentIngredient
    {
        get => _currentIngredient.gameObject;
        set => _currentIngredient = value.TryGetComponent(out Ingredient ingredient) ? ingredient : null;
    }
    
    public readonly Dictionary<IngredientInfo, int> PlacedIngredients = new Dictionary<IngredientInfo, int>();

    private void Awake()
    {
        _currentCanvas = sauceCanvas;
        cheeseCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentCanvas = cheeseCanvas;
            cheeseCanvas.enabled = true;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentCanvas = sauceCanvas;
            sauceCanvas.enabled = true;
            cheeseCanvas.enabled = false;
        }

        if (_currentIngredient != null)
        {
            if (Input.GetMouseButtonDown(0)) PlaceIngredient();
        }
        else if (Input.GetMouseButton(0)) _currentCanvas.Paint(Helpers.Camera.ScreenToWorldPoint(Input.mousePosition));
        
        if(Input.GetMouseButtonDown(1)) _currentIngredient = null;
    }

    private void PlaceIngredient()
    {
        Vector2 mousePos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (mousePos.magnitude <= transform.localScale.x/2f - borderLength - _currentIngredient.transform.localScale.x/2f)
        {
             IngredientInfo info = _currentIngredient.Info;
             if (PlacedIngredients.ContainsKey(info)) PlacedIngredients[info]++;
             else PlacedIngredients.Add(info, 1);
            
             Instantiate(CurrentIngredient, mousePos,  Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }
}