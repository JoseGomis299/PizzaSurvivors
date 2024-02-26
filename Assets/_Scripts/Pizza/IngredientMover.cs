using System.Collections.Generic;
using System.Linq;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientMover : MonoBehaviour
{
    private PizzaIngredient _selectedIngredient;
    private Image _selectedImage;
    private Vector3 _originalPosition;

    private IngredientPlacer _ingredientPlacer;
    
    private void Start()
    {
        _ingredientPlacer = GetComponent<IngredientPlacer>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _ingredientPlacer.CurrentIngredient == null && _ingredientPlacer.CurrentBaseIngredient == null)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            
            _selectedIngredient = results.FirstOrDefault(x => x.gameObject.TryGetComponent(out PizzaIngredient _)).gameObject != null ? results.FirstOrDefault(x => x.gameObject.TryGetComponent(out PizzaIngredient _)).gameObject.GetComponent<PizzaIngredient>() : null;
            if (_selectedIngredient == null) return;
                
            _originalPosition = _selectedIngredient.transform.position;
            _selectedImage = _selectedIngredient.GetComponent<Image>();
        }

        if (_selectedIngredient != null)
        {
            _selectedIngredient.transform.position = Input.mousePosition;

            if (!_ingredientPlacer.IsValid(Input.mousePosition, _selectedIngredient.gameObject))
                _selectedImage.color = Color.white * 0.5f;
            else
                _selectedImage.color = Color.white;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (_selectedIngredient == null) return;
            
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (_ingredientPlacer.IsValid(Input.mousePosition, _selectedIngredient.gameObject))
                _originalPosition = _selectedIngredient.transform.position;

            if (results.Any(x => x.gameObject.CompareTag("ThrashCan")))
            {
                _selectedIngredient.RemoveFromPizza();
                _selectedIngredient = null;
            }
            else _selectedIngredient.transform.DoMove(_originalPosition, 0.2f);

            if (_selectedIngredient != null)
            {
                _ingredientPlacer.ApplyCheeseAndSauceEffects(_selectedIngredient);
                _selectedIngredient = null;
            }
            
            _selectedImage.color = Color.white;
            _selectedImage = null;
        }
    }
    
    private void OnMouseDrag()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}