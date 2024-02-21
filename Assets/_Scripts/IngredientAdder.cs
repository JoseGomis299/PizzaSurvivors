#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

public class IngredientAdder : MonoBehaviour
{
    void Start()
    {
        string[] guids = AssetDatabase.FindAssets("t:Ingredient");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Ingredient ingredient = AssetDatabase.LoadAssetAtPath<Ingredient>(assetPath);
            IngredientInventory.AddIngredient(ingredient, 5);
        }
    }
}

#endif
