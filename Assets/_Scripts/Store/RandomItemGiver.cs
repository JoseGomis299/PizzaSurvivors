using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemGiver : MonoBehaviour
{
    [SerializeField] private List<StoreItem> items;

    private void Start()
    {
        items = items.OrderBy(x => x.rarity).ToList();
        RoundManager.OnRoundEnd += GiveRandomItem;
    }

    private void OnDestroy()
    {
        RoundManager.OnRoundEnd -= GiveRandomItem;
    }

    private void GiveRandomItem(int round)
    {
        int index = Random.Range(0, Mathf.Min(round+3, items.Count));
        IngredientInventory.AddIngredient((Ingredient) items[index]);
    }
}
