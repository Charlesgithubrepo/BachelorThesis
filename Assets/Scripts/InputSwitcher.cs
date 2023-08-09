using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputSwitcher : MonoBehaviour
{
    public GameObject mouseControlObject; // Drag your GameObject with the FollowMouse script attached here
    public GameObject leapMotionControlObject; // Drag your Leap Motion Controller here
    public TextMeshProUGUI mouseText;
    public TextMeshProUGUI motionControllerText;
    void Update()
    {
        // Listen for the spacebar key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If mouse control is currently active, disable it and enable Leap Motion control
            if (mouseControlObject.activeInHierarchy)
            {
                mouseControlObject.SetActive(false);
                mouseText.gameObject.SetActive(false);
                leapMotionControlObject.SetActive(true);
                motionControllerText.gameObject.SetActive(true);
                Debug.Log("Switched to Leap Motion control"); // This line prints to the console when Leap Motion control is activated
            }
            else // If Leap Motion control is currently active, disable it and enable mouse control
            {
                mouseControlObject.SetActive(true);
                mouseText.gameObject.SetActive(true);
                leapMotionControlObject.SetActive(false);
                motionControllerText.gameObject.SetActive(false);
                Debug.Log("Switched to mouse control"); // This line prints to the console when mouse control is activated
            }
        }
    }
}
