using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropSystem : MonoBehaviour
{
    public GameObject[] itemDrop;
    void Start()
    {
    }

    void Update()
    {
    }

    public void DropItem() {

        for (int i = 0; i < itemDrop.Length; i++)
        {
            Instantiate(itemDrop[i], transform.position, Quaternion.identity);
        }
    }
}
