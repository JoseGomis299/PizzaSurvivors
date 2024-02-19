using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject camera;
    private Camera cam;
    public Stats playerStats;
    public BulletSpawner bulletSpawner;

    [Header("CameraInfo")] [SerializeField]
    private float maxSpeed = 0.5f;
    [SerializeField] private float camRange = 1f;
    [SerializeField] private float rangeMult = 0.25f;
    [SerializeField, Range(0, 1)] private float offsetDistMult = 0.25f;
    [SerializeField] private float distBuffer = 0.1f;
    
    [Header("CameraSettings")]
    [SerializeField] private bool useOffset = true;
    [SerializeField, Range(0, 0.499f)] private float playerMarginMult = 0.2f;

    private Vector3 targetPos;
    private float speed = 0f;
    private Vector3 lastPos;
    private Vector3 offset = Vector3.zero;
    private float targetCamSize;

    private Vector2 lastMousePos;
    private bool aimingMode = false;
    private float lastMovedMouse = 0f;
    [SerializeField] private float stopAimTime = 5f;
    [SerializeField] private float reelbackTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camera.GetComponent<Camera>();
        playerStats = transform.GetComponentInParent<StatsManager>().Stats;
        bulletSpawner = transform.GetComponentInParent<BulletSpawner>();

        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        targetPos = transform.position;
        lastPos = transform.position;
        targetCamSize = cam.orthographicSize;

        lastMovedMouse = Time.time;
        lastMousePos = Input.mousePosition;
        aimingMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetCamSize();
        SetCamRange();
        
        if (useOffset)
        {
            UpdateCamOffset();
        }
        else
        {
            offset = Vector3.zero;
        }

        if (!aimingMode)
        {
            targetPos = Vector3.Lerp(offset, Vector3.zero, (Time.time - lastMovedMouse - stopAimTime) / reelbackTime) + transform.position;
        }
        else
        {
            targetPos = transform.position + offset;
        }

        //Debug.Log(targetPos);

        UpdateCamSpeed(targetPos);
    }

    private void FixedUpdate()
    {
        MoveCam(targetPos);
        ChangeCamSize();
    }

    void UpdateCamSpeed(Vector3 targetPos)
    {
        float normalisedDistance = (targetPos - camera.transform.position).magnitude / camRange;

        //normalisedDistance = Mathf.Clamp(normalisedDistance, 0, 1);

        speed = (normalisedDistance - 1) * (normalisedDistance - 1) * (normalisedDistance - 1) + 1;

        //speed = maxSpeed * (-Mathf.Exp(-10 * normalisedDistance) + 1);

        speed *= maxSpeed;

        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }

    private void UpdateCamOffset()
    {
        Vector2 mouse = Input.mousePosition;

        if (mouse != lastMousePos || Input.GetMouseButton(0))
        {
            lastMovedMouse = Time.time;
            aimingMode = true;
            lastMousePos = mouse;
        }
        else if (Time.time - lastMovedMouse >= stopAimTime)
        {
            aimingMode = false;
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(mouse);
        mousePos = new Vector3(mousePos.x, mousePos.y, camera.transform.position.z);
        offset = Vector3.ClampMagnitude((mousePos - transform.position), playerStats.BulletsMaxRange);
        offset *= offsetDistMult;
    }

    void MoveCam(Vector3 targetPos)
    {
        Vector3 camPos = camera.transform.position;
        
        //Debug.Log(speed);
        
        Vector3 newPos = camPos + (targetPos - camPos).normalized * (speed * Time.fixedDeltaTime);

        camera.transform.position = newPos;
    }

    void SetCamSize()
    {
        //targetCamSize = (playerStats.BulletsMaxRange + bulletSpawner.GetFirePointDistance()) / (2 - 1.5f * playerMarginMult);

        targetCamSize = Mathf.Max(
            (playerStats.BulletsMaxRange * (1 - offsetDistMult) + bulletSpawner.GetFirePointDistance()) /
            (1 - playerMarginMult * 2f),
            (playerStats.BulletsMaxRange * (1 - offsetDistMult)) / (1 - playerMarginMult * 2f));

        //targetCamSize = (playerStats.BulletsMaxRange * offsetDistMult + bulletSpawner.GetFirePointDistance()) / (1 - playerMarginMult);
    }

    void ChangeCamSize()
    {
        if (Mathf.Abs(cam.orthographicSize - targetCamSize) <= 0.25f)
        {
            cam.orthographicSize = targetCamSize;
            return;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCamSize, 0.5f);
    }

    void SetCamRange()
    {
        camRange = playerStats.BulletsMaxRange * (1 + rangeMult);
    }

    public bool getIsAiming()
    {
        return aimingMode;
    }

    public float getMouseInactivityTime()
    {
        return stopAimTime;
    }

    public float getReelbackTime()
    {
        return reelbackTime;
    }

    public float getLastMovedMouse()
    {
        return lastMovedMouse;
    }
}