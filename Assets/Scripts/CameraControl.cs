﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 1;
    public float cameraSpeed = 10;
    public float clampAngle = 80;
    public Camera mainCamera;

    public GameObject ballPrefab;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // FixedUpdate for the move
    void Update()
    {
        // Grab inputs first
        // Keyboard 
        Vector3 translation = new Vector3(0, 0, 0);

        translation.z = Input.GetAxisRaw("Vertical");
        translation.x = Input.GetAxisRaw("Horizontal");
        translation.y = Input.GetAxisRaw("Jump");

        // Mouse 
        float horz = 0;
        float vert = 0;


        horz = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        vert = -Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        //  Relative translation because it depends on the normal
        translation *= Time.deltaTime * cameraSpeed;
        transform.Translate(translation);

        // Rotation
        if (Cursor.lockState != CursorLockMode.None)
        {
            transform.RotateAround(transform.position, Vector3.up, horz);
            Vector3 rot = transform.localRotation.eulerAngles;
            rot.x = (transform.localRotation.eulerAngles.x + 180) % 360 - 180 + vert;
            rot.x = Mathf.Clamp(rot.x, -clampAngle, clampAngle);
            transform.localRotation = Quaternion.Euler(rot);
        }


        // Mouse control
        if (Input.GetMouseButton(1)) // Right click
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Mouse control
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            RaycastHit hit;
            Ray ballRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ballRay, out hit))
            {
                Instantiate(ballPrefab, hit.point + Vector3.up, transform.rotation);
            }
        }
    }
}
