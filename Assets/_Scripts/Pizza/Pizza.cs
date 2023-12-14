using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectUtils.Helpers;

public class Pizza : MonoBehaviour
{
    public static Action OnEnterPizzaView;
    public static Action OnExitPizzaView;

    private BulletSpawner _bulletSpawner;
    private StatsManager _statsManager;

    private readonly List<Ingredient> _placedIngredients = new  List<Ingredient> ();
    private List<StatsManagerEffect> _currentBuffs = new List<StatsManagerEffect>();

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
        _currentBuffs = new List<StatsManagerEffect>();
        
        foreach (var ingredient in _placedIngredients)
        {
            bulletModifiers.AddRange(ingredient.BulletModifiers);
            for (var i = 0; i < ingredient.Buffs.Length; i++)
            {
                _currentBuffs.Add(ingredient.Buffs[i].GetEffect(_statsManager));
                _statsManager.ApplyEffect(_currentBuffs[^1]);
            }
        }
        
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
        
        OnEnterPizzaView?.Invoke();
    }

    public void PlaceIngredient(Ingredient ingredient)
    {
        _placedIngredients.Add(ingredient);
    }
}