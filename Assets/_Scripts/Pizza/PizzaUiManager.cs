using System;
using System.Collections.Generic;
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
    [SerializeField] private Transform buffsContainer;
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private StatIcon[] statIcons;
    
    [Space(10)]
    [SerializeField] private Transform modificationsContainer;
    [SerializeField] private GameObject modificationPrefab;

    [Header("Inventory")]
    [SerializeField] private Transform inventory;
    [SerializeField] private GameObject inventoryItemPrefab;
    
    private IngredientPlacer _ingredientPlacer;
    private Pizza _pizza;
    
    private void Start()    
    {
        _ingredientPlacer = GetComponent<IngredientPlacer>();
        _pizza = FindObjectOfType<Pizza>();
        
        _ingredientPlacer.OnIngredientChanged += UpdateCurrentIngredient;
        
        Pizza.OnEnterPizzaView += UpdateInventory;
        Pizza.OnEnterPizzaView += ResizeAndMoveInventory;
        Pizza.OnIngredientPlaced += UpdateCurrentIngredient;
        
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
            
            instance.GetComponent<Image>().sprite = ingredient.Key.icon;
            instance.GetComponentInChildren<TMP_Text>().text = ingredient.Value.ToString();
            
            instance.GetComponent<Button>().onClick.AddListener(() => _ingredientPlacer.SetIngredient(ingredient.Key));
        }
        
        inventory.GetComponent<UiResizer>().Resize(false);
    }
    
    private void ResizeAndMoveInventory()
    {
        inventory.GetComponent<UiResizer>().Resize(true);
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
        
        buffsContainer.DeleteChildren();
        modificationsContainer.DeleteChildren();

        foreach (var statBuff in _ingredientPlacer.CurrentIngredient.Buffs)
        {
            GameObject instance = Instantiate(statPrefab, buffsContainer);
            instance.GetComponentInChildren<Image>().sprite = statIcons.FirstOrDefault(x => x.type == statBuff.Type).icon;
            instance.GetComponentInChildren<TMP_Text>().text = statBuff.ToString();
        }

        List<BulletModifierInfo> seenModifiers = new List<BulletModifierInfo>();
        GameObject textContainer = Instantiate(modificationPrefab, modificationsContainer);
        TMP_Text modifiersText = textContainer.GetComponent<TMP_Text>();
        modifiersText.text = "";
        modifiersText.color = Color.green;
        
        foreach (var modifier in _ingredientPlacer.CurrentIngredient.BulletModifiers)
        {
            if(seenModifiers.Contains(modifier)) continue;
            
            int i = 1;
            int currentLevel = _pizza.GetModifierStacks(modifier);
            modifiersText.text += $"{modifier.name} +{_ingredientPlacer.CurrentIngredient.GetModifierCount(modifier)}\n";
            seenModifiers.Add(modifier);
            foreach (var description in modifier.GetDescriptions())
            {
                GameObject instance = Instantiate(modificationPrefab, modificationsContainer);
                TMP_Text text = instance.GetComponent<TMP_Text>();
                text.color = currentLevel >= i++ ? Color.white : Color.grey;
                text.text = description;
            }
        }
        
        buffsContainer.GetComponent<UiResizer>().Resize();
        modificationsContainer.GetComponent<UiResizer>().Resize();
        statsContainer.GetComponent<UiResizer>().Resize();
    }
    
    private void OnDestroy()
    {
        _ingredientPlacer.OnIngredientChanged -= UpdateCurrentIngredient;
        
        Pizza.OnEnterPizzaView -= UpdateInventory;
        Pizza.OnEnterPizzaView -= ResizeAndMoveInventory;
        Pizza.OnIngredientPlaced -= UpdateCurrentIngredient;
        IngredientInventory.OnInventoryChanged -= UpdateInventory;
        
        
    }
}