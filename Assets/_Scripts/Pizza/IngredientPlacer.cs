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
    
    public Action OnIngredientChanged;

    [Header("Pizza")]
    [SerializeField] private RectTransform pizzaBase;
    [SerializeField] private float borderLength = 100;

    [Header("Base Ingredients")]
    [SerializeField] private PizzaBaseIngredient sauce;
    [SerializeField] private PizzaBaseIngredient cheese;
    
    private PizzaBaseIngredient _currentBaseIngredient;
    private Ingredient _currentIngredient;
    public Ingredient CurrentIngredient => _instantiatedIngredient?.Ingredient;
    public PizzaBaseIngredient CurrentBaseIngredient => _currentBaseIngredient;
    
    private Image _instantiatedIngredientImage;
    private PizzaIngredient _instantiatedIngredient;
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
        
        if (_currentIngredient != null && _instantiatedIngredientImage != null)
        {
            _instantiatedIngredientImage.transform.position = Input.mousePosition;
            if(!IsValid(Input.mousePosition, _instantiatedIngredientImage.gameObject)) _instantiatedIngredientImage.color = Color.white*0.5f;
            else
            {
                _instantiatedIngredientImage.color = Color.white;
                PreviewModification(_instantiatedIngredient);
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
        _instantiatedIngredientImage = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)), pizzaBase.parent).GetComponent<Image>();
        _instantiatedIngredient = _instantiatedIngredientImage.GetComponent<PizzaIngredient>();
        _instantiatedIngredientImage.color = Color.white;
        _instantiatedIngredient.Initialize(_currentIngredient, _pizza);
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
        PlaceIngredient(_instantiatedIngredient);

        _instantiatedIngredientImage.GetComponent<Image>().color = Color.white;
        _instantiatedIngredientImage = null;
        
        if (IngredientInventory.GetIngredientQuantity(_currentIngredient) > 0) InstantiatePreview();
        else ClearSelection();
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
        _instantiatedIngredient = null;
        _instantiatedIngredientImage = null;
    }
    
    private void PlaceIngredient(PizzaIngredient ingredient)
    {
         _pizza.PlaceIngredient(ingredient.Ingredient);
         ApplyCheeseAndSauceEffects(ingredient);
    }
    
    public void ApplyCheeseAndSauceEffects(PizzaIngredient ingredient)
    {
        Ingredient prevIngredient = ingredient.Ingredient;
        ModifyIngredient(ingredient);
        
        _pizza.RemoveIngredient(prevIngredient);
        _pizza.PlaceIngredient(ingredient.Ingredient);
    }
    
    private void ModifyIngredient(PizzaIngredient ingredient)
    {
        bool hasSauce = sauce.IsPainted(ingredient.transform.position);
        bool hasCheese = cheese.IsPainted(ingredient.transform.position);
        
        Ingredient originalIngredient = ingredient.Ingredient.OriginalIngredient;
        if(originalIngredient != null) ingredient.Ingredient = originalIngredient;
            
        if(hasSauce)
            sauce.ModifyIngredient(ingredient);
        
        if(hasCheese)
            cheese.ModifyIngredient(ingredient);
    }
    
    public void PreviewModification(PizzaIngredient ingredient)
    {
        //bool hasSauce = sauce.IsPainted(ingredient.transform.position);
        bool hasCheese = cheese.IsPainted(ingredient.transform.position);
        
        Ingredient originalIngredient = ingredient.Ingredient.OriginalIngredient;
        if (originalIngredient != null && !hasCheese)
        {
            ingredient.Ingredient = originalIngredient;
            OnIngredientChanged?.Invoke();
        }
        else if (originalIngredient == null && hasCheese)
        {
            cheese.ModifyIngredient(ingredient);
            OnIngredientChanged?.Invoke();
        }

     
        //if(hasSauce)
        //   sauce.ModifyIngredient(ingredient);
    }
}