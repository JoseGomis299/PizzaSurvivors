using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StatsManager statsManager;

    private void Start()
    {
        statsManager = GetComponent<StatsManager>();
    }


}
