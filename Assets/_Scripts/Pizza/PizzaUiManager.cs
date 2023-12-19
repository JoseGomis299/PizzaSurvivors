using System;
using System.Linq;
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
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TMP_Text currentIngredientName;
    [SerializeField] private Image currentIngredientImage;
    
    [Serializable]
    struct StatIcon
    {
        public BuffType type;
        public Sprite icon;
    }
    
    [Space(10)]
    [SerializeField] private Transform statsContainer;
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private StatIcon[] statIcons;
    
    [Space(10)]
    [SerializeField] private Transform modificationsContainer;
    [SerializeField] private GameObject modificationPrefab;

    [Header("Inventory")]
    [SerializeField] private Transform inventory;
    [SerializeField] private GameObject inventoryItemPrefab;
    
    private IngredientPlacer _ingredientPlacer;
    
    private void Start()    
    {
        _ingredientPlacer = GetComponent<IngredientPlacer>();
        
        _ingredientPlacer.OnIngredientChanged += UpdateCurrentIngredient;
        
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

    private void UpdateCurrentIngredient()
    {
        Sprite sprite = _ingredientPlacer.GetCurrentSprite();
        if (sprite == null || _ingredientPlacer.CurrentIngredient == null)
        {
            statsPanel.SetActive(false);
            return;
        }
        
        statsPanel.SetActive(true);
        currentIngredientImage.sprite = sprite;
        currentIngredientName.text = _ingredientPlacer.CurrentIngredient.name;
        
        statsContainer.DeleteChildren();
        modificationsContainer.DeleteChildren();

        foreach (var statBuff in _ingredientPlacer.CurrentIngredient.Buffs)
        {
            GameObject instance = Instantiate(statPrefab, statsContainer);
            instance.GetComponentInChildren<Image>().sprite = statIcons.FirstOrDefault(x => x.type == statBuff.Type).icon;
            instance.GetComponentInChildren<TMP_Text>().text = statBuff.ToString();
        }

        foreach (var modifier in _ingredientPlacer.CurrentIngredient.BulletModifiers)
        {
            foreach (var description in modifier.GetDescriptions())
            {
                GameObject instance = Instantiate(modificationPrefab, modificationsContainer);
                instance.GetComponent<TMP_Text>().text = description;
            }
        }
        
        statsContainer.GetComponent<UiResizer>().Resize();
        modificationsContainer.GetComponent<UiResizer>().Resize();
    }
    
    private void OnDestroy()
    {
        _ingredientPlacer.OnIngredientChanged -= UpdateCurrentIngredient;
        
        Pizza.OnEnterPizzaView -= UpdateInventory;
        IngredientInventory.OnInventoryChanged -= UpdateInventory;
    }
}