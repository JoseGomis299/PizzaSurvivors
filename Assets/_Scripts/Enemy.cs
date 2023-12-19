using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StatsManager _statsManager;
    private CharacterMovement _characterMovement;

    private void Start()
    {
        _statsManager = GetComponent<StatsManager>();
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void FixedUpdate()
    {
        _characterMovement.UpdateMovement(Vector3.zero, Time.fixedDeltaTime);
    }
}
