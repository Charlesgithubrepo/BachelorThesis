using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.SceneManagement;*/

public class Bomb : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManagerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        BombDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Game Over");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    private void BombDestroy()
    {
        if (transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

}
