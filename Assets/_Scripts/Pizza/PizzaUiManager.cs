using ProjectUtils.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PizzaUiManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button cheeseButton;
    [SerializeField] private Button sauceButton;
    [SerializeField] private Button exitButton;
    
    [Header("Current Ingredient")]
    [SerializeField] private Sprite defaultIngredientSprite;
    [SerializeField] private Image currentIngredientImage;

    [Header("Inventory")]
    [SerializeField] private Transform inventory;
    [SerializeField] private GameObject inventoryItemPrefab;
    
    private IngredientPlacer _ingredientPlacer;
    
    private void Start()    
    {
        _ingredientPlacer = GetComponent<IngredientPlacer>();
        
        _ingredientPlacer.OnIngredientChanged += UpdateCurrentIngredientImage;
        
        Pizza.OnEnterPizzaView += UpdateInventory;
        IngredientInventory.OnInventoryChanged += UpdateInventory;
        
        cheeseButton.onClick.AddListener(_ingredientPlacer.StartPlacingCheese);
        sauceButton.onClick.AddListener(_ingredientPlacer.StartPlacingSauce);
        exitButton.onClick.AddListener(_ingredientPlacer.ExitPizzaView);
    }

    private void UpdateInventory()
    {
        if(!transform.GetChild(0).gameObject.activeInHierarchy) return;
        
        inventory.DeleteChildren();
        foreach (var ingredient in IngredientInventory.Ingredients)
        {
            GameObject instance = Instantiate(inventoryItemPrefab, inventory);
            
            instance.GetComponent<Image>().sprite = ingredient.Key.uiSprite;
            instance.GetComponentInChildren<TMP_Text>().text = ingredient.Value.ToString();
            
            instance.GetComponent<Button>().onClick.AddListener(() => _ingredientPlacer.SetIngredient(ingredient.Key));
        }
    }

    private void UpdateCurrentIngredientImage()
    {
        Sprite sprite = _ingredientPlacer.GetCurrentSprite();

        currentIngredientImage.sprite = sprite != null ? sprite : defaultIngredientSprite;
    }
    
    private void OnDestroy()
    {
        _ingredientPlacer.OnIngredientChanged -= UpdateCurrentIngredientImage;
        
        Pizza.OnEnterPizzaView -= UpdateInventory;
        IngredientInventory.OnInventoryChanged -= UpdateInventory;
    }
}