using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject camera;
    private Camera cam;
    public Stats playerStats;

    [Header("CameraInfo")] [SerializeField]
    private float maxSpeed = 0.5f;
    [SerializeField] private float camRange = 1f;
    [SerializeField] private float offsetDistMult = 0.25f;
    [SerializeField] private float distBuffer = 0.1f;
    
    [Header("CameraSettings")]
    [SerializeField] private bool useOffset = true;

    private float speed = 0f;
    private Vector3 lastPos;
    private Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camera.GetComponent<Camera>();
        playerStats = transform.GetComponentInParent<StatsManager>().Stats;

        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (useOffset)
        {
            UpdateCamOffset();
        }
        else
        {
            offset = Vector3.zero;
        }

        UpdateCamSpeed(transform.position + offset);
    }

    private void FixedUpdate()
    {
        MoveCam(transform.position + offset);
    }

    void UpdateCamSpeed(Vector3 targetPos)
    {
        float normalisedDistance = (targetPos - camera.transform.position).magnitude / camRange;

        speed = (normalisedDistance - 1) * (normalisedDistance - 1) * (normalisedDistance - 1) + 1;

        //speed = maxSpeed * (-Mathf.Exp(-10 * normalisedDistance) + 1);

        speed *= maxSpeed;
    }

    private void UpdateCamOffset()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, camera.transform.position.z);
        offset = Vector3.ClampMagnitude((mousePos - transform.position) * offsetDistMult, playerStats.BulletsMaxRange * offsetDistMult);
    }

    void MoveCam(Vector3 targetPos)
    {
        Vector3 camPos = camera.transform.position;

        camera.transform.position = camPos + (targetPos - camPos).normalized * (speed * Time.fixedDeltaTime);
    }
}