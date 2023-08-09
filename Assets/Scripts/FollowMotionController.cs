using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMotionController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0.5f, 0.5f, 10f); // Adjust the offset as needed

    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.SetActive(true);
    }

    public void ActivateSphereFollowMotionController()
    {
        gameObject.SetActive(true); // Enable the cubeFollowMouse object when called
    }
}
