using System;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public static event Action OnEnterPizzaView;
    public static event Action OnExitPizzaView;
    public static event Action OnIngredientPlaced;
    public static event Action OnIngredientRemoved;

    private BulletSpawner _bulletSpawner;
    private StatsManager _statsManager;

    private readonly List<Ingredient> _placedIngredients = new  List<Ingredient> ();
    private List<IncrementalEffect> _currentBuffs = new List<IncrementalEffect>();

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _bulletSpawner = player.GetComponent<BulletSpawner>();
        _statsManager = player.GetComponent<StatsManager>();
    }

    public void ExitPizzaView()
    {
        Time.timeScale = 1f;
        transform.GetChild(0).gameObject.SetActive(false);
        
        List<BulletModifierInfo> bulletModifiers = new List<BulletModifierInfo>();
        _currentBuffs = new List<IncrementalEffect>();
        
        foreach (var ingredient in _placedIngredients)
        {
            bulletModifiers.AddRange(ingredient.BulletModifiers);
            for (var i = 0; i < ingredient.Buffs.Length; i++)
            {
                _currentBuffs.Add(ingredient.Buffs[i].GetEffect(_statsManager));
                _statsManager.ApplyEffect(_currentBuffs[^1]);
            }
        }
        
        Cursor.visible = false;
        _bulletSpawner.Initialize(bulletModifiers);
        OnExitPizzaView?.Invoke();
    }
    
    public void EnterPizzaView()
    {
        Time.timeScale = 0f;
        transform.GetChild(0).gameObject.SetActive(true);
        
        foreach (var buff in _currentBuffs)
        {
            buff.DeApply();
        }
        
        Cursor.visible = true;
        OnEnterPizzaView?.Invoke();
    }

    public void PlaceIngredient(Ingredient ingredient)
    {
        _placedIngredients.Add(ingredient);
        OnIngredientPlaced?.Invoke();
    }
    
    public void RemoveIngredient(Ingredient ingredient)
    {
        _placedIngredients.Remove(ingredient);
        OnIngredientRemoved?.Invoke();
    }
    
    public int GetModifierStacks(BulletModifierInfo modifier)
    {
        int stacks = 0;
        foreach (var ingredient in _placedIngredients)
        {
            stacks += ingredient.GetModifierCount(modifier);
        }
        return stacks;
    }
}