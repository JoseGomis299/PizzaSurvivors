using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private float collectionRadius = 1f;
    [SerializeField] private float collectionSpeed = 100f;
    [SerializeField] private LayerMask collectableLayer;
    
    private void FixedUpdate()
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, collectionRadius, collectableLayer);

        foreach (var collectable in collisions)
        {
            Vector2 direction = (transform.position - collectable.transform.position).normalized;
            collectable.GetComponent<Rigidbody2D>().AddForce(direction*collectionSpeed);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Collect(gameObject);
            collision.gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
#endif
}