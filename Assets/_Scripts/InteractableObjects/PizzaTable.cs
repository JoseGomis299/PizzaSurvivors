using System;
using UnityEngine;

public class PizzaTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject pizzaUI;
    private Pizza _pizza;
    
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;


    private void Awake()
    {
        SpawningSystem.OnRoundStart += OnRoundStart;
        SpawningSystem.OnRoundEnd += OnRoundEnd;
    }

    private void Start()
    {
        _pizza = pizzaUI.GetComponent<Pizza>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    private void OnDestroy()
    {
        SpawningSystem.OnRoundStart -= OnRoundStart;
        SpawningSystem.OnRoundEnd -= OnRoundEnd;
    }

    private void OnRoundStart(int round)
    {
        gameObject.SetActive(false);
    }
    
    private void OnRoundEnd(int round)
    {
        gameObject.SetActive(true);
    }


    public void OnInteractRangeEnter()
    {
        _spriteRenderer.color = Color.green;
    }
    
    public void OnInteractRangeExit()
    {
        _spriteRenderer.color = _defaultColor;
    }
    
    public void Interact()
    {
        _pizza.EnterPizzaView();
    }
}