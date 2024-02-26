using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IngredientPlacer : MonoBehaviour
{
    private Pizza _pizza;
    
    public event Action OnIngredientChanged;

    [Header("Pizza")]
    [SerializeField] private RectTransform pizzaBase;
    [SerializeField] private float borderLength = 100;

    [Header("Base Ingredients")]
    [SerializeField] private PizzaBaseIngredient sauce;
    [SerializeField] private PizzaBaseIngredient cheese;
    
    private PizzaBaseIngredient _currentBaseIngredient;
    private Ingredient _currentIngredient;
    public Ingredient CurrentIngredient => _currentIngredient;
    public PizzaBaseIngredient CurrentBaseIngredient => _currentBaseIngredient;
    
    private Image _instantiatedIngredient;
    [SerializeField] private GameObject ingredientPrefab;

    private void Start()
    {
        _pizza = GetComponent<Pizza>();
    }
    
    private void Update()
    {
        if(!transform.GetChild(0).gameObject.activeInHierarchy) return;
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {        
            ExitPizzaView();
            return;
        }
        
        if (_currentIngredient != null && _instantiatedIngredient != null)
        {
            _instantiatedIngredient.transform.position = Input.mousePosition;
            if(!IsValid(Input.mousePosition, _instantiatedIngredient.gameObject)) _instantiatedIngredient.color = Color.white*0.5f;
            else
            {
                _instantiatedIngredient.color = Color.white;
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceIngredient();
                }
            }
        }
        else if (_currentBaseIngredient != null && Input.GetMouseButton(0)) 
            _currentBaseIngredient.Draw(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
            ClearSelection();
    }
    
    public void SetIngredient(Ingredient ingredient)
    {
        ClearCurrentIngredient();

        _currentBaseIngredient = null;
        _currentIngredient = ingredient;
        
        InstantiatePreview();
        OnIngredientChanged?.Invoke();
    }

    private void InstantiatePreview()
    {
        _instantiatedIngredient = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)), pizzaBase.parent).GetComponent<Image>();
        _instantiatedIngredient.color = Color.white;
        _instantiatedIngredient.GetComponent<PizzaIngredient>().Initialize(_currentIngredient, _pizza);
    }

    public void StartPlacingCheese()
    {
        ClearCurrentIngredient();
        
        _currentBaseIngredient = cheese;
        OnIngredientChanged?.Invoke();
    }
    
    public void StartPlacingSauce()
    {
        ClearCurrentIngredient();
        
        _currentBaseIngredient = sauce;
        OnIngredientChanged?.Invoke();
    }
    
    public Sprite GetCurrentSprite()
    {
        if(_currentIngredient != null) return _currentIngredient.icon;
        return _currentBaseIngredient != null ? _currentBaseIngredient.GetSprite() : null;
    }
    
    public void ExitPizzaView()
    {
        foreach (var placedIngredient in _pizza.transform.GetComponentsInChildren<PizzaIngredient>())
        {
            ApplyCheeseAndSauceEffects(placedIngredient);
        }
        ClearSelection();
        
        _pizza.ExitPizzaView();
    }
    
    private void ClearSelection()
    {
        ClearCurrentIngredient();
        _currentBaseIngredient = null;
        
        OnIngredientChanged?.Invoke();
    }
    
    private void PlaceIngredient()
    {
        if (IngredientInventory.GetIngredientQuantity(_currentIngredient) <= 0) return;

        IngredientInventory.RemoveIngredient(_currentIngredient);
        PlaceIngredient(_instantiatedIngredient.GetComponent<PizzaIngredient>());

        _instantiatedIngredient.GetComponent<Image>().color = Color.white;
        _instantiatedIngredient = null;
        
        if (IngredientInventory.GetIngredientQuantity(_currentIngredient) > 0) InstantiatePreview();
    }
    
    public bool IsValid(Vector2 position, GameObject selectedIngredient)
    {
        float limit = pizzaBase.rect.width * pizzaBase.localScale.x / 2f - borderLength;
        float distance = Vector2.Distance(position, pizzaBase.position);
        
        bool isInside = distance <= limit;
        bool hasSauce = sauce.IsPainted(Input.mousePosition);
        bool isOnIngredient = false;
        
        var results = new List<RaycastResult>();
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition};
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if(selectedIngredient != null) results.RemoveAll(x => x.gameObject == selectedIngredient);
        if(results.FirstOrDefault(x => x.gameObject.TryGetComponent<PizzaIngredient>(out _)).gameObject != null) isOnIngredient = true;
        
        return isInside && hasSauce && !isOnIngredient;
    }
    
    private void ClearCurrentIngredient()
    {
        _currentIngredient = null;
        if(_instantiatedIngredient != null) Destroy(_instantiatedIngredient.gameObject);
    }
    
    private void PlaceIngredient(PizzaIngredient ingredient)
    {
        if(!ApplyCheeseAndSauceEffects(ingredient))
         _pizza.PlaceIngredient(ingredient.Ingredient);
    }
    
    public bool ApplyCheeseAndSauceEffects(PizzaIngredient ingredient)
    {
        bool hasSauce = sauce.IsPainted(ingredient.transform.position);
        bool hasCheese = cheese.IsPainted(ingredient.transform.position);
        
        Ingredient prevIngredient = ingredient.Ingredient;
        if(ingredient.Ingredient.OriginalIngredient != null) ingredient.Ingredient = ingredient.Ingredient.OriginalIngredient;

        if(hasSauce)
            sauce.ModifyIngredient(ingredient);
        
        if(hasCheese)
            cheese.ModifyIngredient(ingredient);
        
        if (prevIngredient != ingredient.Ingredient)
        {
            _pizza.RemoveIngredient(prevIngredient);
            _pizza.PlaceIngredient(ingredient.Ingredient);
            return true;
        }
        return false;
    }
}