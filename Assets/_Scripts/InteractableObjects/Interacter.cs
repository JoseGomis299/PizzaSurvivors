using System;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    public static event Action OnInteract;
    
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private LayerMask interactableLayer;
    
    private IInteractable _interactable;
    
    private void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);
        if (collider == null)
        {
            if(_interactable == null) return;
            
            _interactable.OnInteractRangeExit();
            _interactable = null;
            return;
        }

        if (!collider.TryGetComponent(out IInteractable interactable)) return;
        
        if(interactable != _interactable)
        {
            _interactable?.OnInteractRangeExit();
            _interactable = interactable;
            interactable.OnInteractRangeEnter();
        }
            
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
            OnInteract?.Invoke();
        }
    }
}