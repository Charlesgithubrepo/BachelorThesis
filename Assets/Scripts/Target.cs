using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameManager gameManager;

    public int pointValue;
    public int lifeValue;
    public GameObject[] explosionPrefabs; // Array to hold references to the explosion prefabs

    void Start()
    {
        gameManager = GameManager.gameManagerInstance;

        explosionPrefabs = Resources.LoadAll<GameObject>("Particles");
    }

    void Update()
    {
        DestroyAndLoseLife();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.AddScore(pointValue);
            Destroy(gameObject);// destroy the target (crates or bombs)
            CreateExplosion(); // create the explosion effect
        }
    }

    private void DestroyAndLoseLife()
    {
        if (transform.position.y <= -5)
        {
            gameManager.LoseLife(lifeValue);
            Destroy(gameObject); // destroy the target
        }
    }

    private void CreateExplosion()
    {
        Vector3 explosionPosition = transform.position;

        // Randomly select an explosion prefab from the array
        int randomIndex = Random.Range(0, explosionPrefabs.Length);
        GameObject randomExplosionPrefab = explosionPrefabs[randomIndex];

        // Instantiate the randomly selected explosion prefab
        Instantiate(randomExplosionPrefab, explosionPosition, Quaternion.identity);
    }
}
