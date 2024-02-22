using UnityEngine;

public class PizzaTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject pizzaUI;
    private Pizza _pizza;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _pizza = pizzaUI.GetComponent<Pizza>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void OnInteractRangeEnter()
    {
        _spriteRenderer.color = Color.green;
    }
    
    public void OnInteractRangeExit()
    {
        _spriteRenderer.color = Color.cyan;
    }
    
    public void Interact()
    {
        _pizza.EnterPizzaView();
    }
}