using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    private StatsManager _statsManager;
    public List<Effect> Buffs;

    private void Start()
    {
        _statsManager = GetComponent<StatsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (var buff in Buffs) _statsManager.ApplyEffect(buff.GetEffect(_statsManager));
        }
    }
}