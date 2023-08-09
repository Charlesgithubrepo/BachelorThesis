using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private GameManager gameManager;
    public int lifeValue;
    public GameObject explosionPrefab;

    void Start()
    {
        gameManager = GameManager.gameManagerInstance;
    }

    void Update()
    {
        HeartDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.AddLife(lifeValue);
            Destroy(gameObject);
            CreateExplosion(); // create the explosion effect
        }
    }

    private void HeartDestroy()
    {
        if (transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

    private void CreateExplosion()
    {
        Vector3 explosionPosition = transform.position;

        // Instantiate the selected explosion prefab
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
    }
}
