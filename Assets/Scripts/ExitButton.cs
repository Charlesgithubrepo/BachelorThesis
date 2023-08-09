using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; // Reference to the button

    void Start()
    {
        exitButton.onClick.AddListener(OnExitButtonClick); // Add a listener for the button click
    }

    void OnExitButtonClick()
    {
        Application.Quit();

        // If you're in the editor, you can use the following line to exit play mode.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
