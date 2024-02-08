using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleBehaviour : MonoBehaviour
{
    public RectTransform reticle;
    private BulletSpawner _bulletSpawner;
    private Stats charStats;
    private Camera mainCam;

    private Vector3 spawnPoint;
    private Vector3 mousePos;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        charStats = GetComponent<StatsManager>().Stats;
        
        _bulletSpawner = GetComponent<BulletSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateReticlePos();
    }

    void UpdateReticlePos()
    {
        spawnPoint = _bulletSpawner.GetFirePoint();
        
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        mousePos.Set(mousePos.x, mousePos.y, 0f);
        spawnPoint.Set(spawnPoint.x, spawnPoint.y, 0f);

        Vector3 directionVector = Vector3.ClampMagnitude(mousePos - spawnPoint, charStats.BulletsMaxRange);

        reticle.position = mainCam.WorldToScreenPoint(spawnPoint + directionVector);
    }
}
