using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScipt : MonoBehaviour
{
    private int value = 0;
    [SerializeField] private string type;
    // Start is called before the first frame update
    void Start()
    {
        if(type == "COIN")
        {
            value = Random.Range(0, 9);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("1");
        if(collision.CompareTag("Player")) {
            CharacterCurrency cc = collision.GetComponent<CharacterCurrency>();

            cc.AddValue(value);
            Destroy(gameObject);
        }
    }
}
