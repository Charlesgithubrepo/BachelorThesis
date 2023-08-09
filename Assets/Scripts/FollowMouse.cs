using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // Reference to the camera
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0.5f, 0.5f, 10f);

    // This flag is checked in Update to see if mouse control should be active
    public bool mouseControlActive = true;

    void Start()
    {
        // Get the camera component
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mouseControlActive)
        {
            // Get the mouse position in screen coordinates
            Vector3 mouseScreenPos = Input.mousePosition;

            // Convert the screen coordinates to world coordinates
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos + offset);

            // Set the position of the cube to the mouse position
            transform.position = mouseWorldPos;
        }
    }
}
