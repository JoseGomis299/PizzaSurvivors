using System;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using ProjectUtils.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDropSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> drops;
    [SerializeField] private float maxDropForce = 5f;

    public void DropItem() {

        if(drops == null) return;
        
        for (int i = 0; i < drops.Count; i++)
        {
            for (int j = 0; j < Random.Range(1, GetDropCount()+1); j++)
            {
                Rigidbody2D itemRb = ObjectPool.Instance.InstantiateFromPool(drops[i], transform.position, Helpers.RandomRotation()).GetComponent<Rigidbody2D>();
                itemRb.AddForce(new Vector2(Random.Range(-1f, 1f) * Random.Range(3, maxDropForce), Random.Range(-1f, 1f)) * Random.Range(3, maxDropForce), ForceMode2D.Impulse);
            }
        }
    }
    
    private int GetDropCount()
    {
        float probability = Random.value;
        if (probability < 0.5f) return 1;
        if (probability < 0.8f) return 2;
        return 3;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (drops == null) return;
        for (var i = 0; i < drops.Count; i++)
        {
            if (drops[i].TryGetComponent<ICollectable>(out var collectable)) continue;
            
            Debug.LogError($"{drops[i].name} does not implement ICollectable interface");
            drops.RemoveAt(i);
        }
    }
#endif
}
