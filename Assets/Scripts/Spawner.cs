using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject[] objectsToSpawn; // Array of prefabs to spawn
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 10;
    private float ySpawnPos = -4;
    private float spawnRate = 1.0f;
    private float initialSpawnRate;

    private int objectCount = 0;

    public GameObject heartObject; // Prefab to spawn
    private int heartSpawnInterval = 30;

    public GameObject bombObject;
    private int nextBombSpawn = 0;  // This will store the next bomb spawn interval

    private void Start()
    {
        gameManager = GameManager.gameManagerInstance;
        initialSpawnRate = spawnRate;

        // Initialize nextBombSpawn to a random number between 3 and 5
        nextBombSpawn = Random.Range(3, 6);

        StartCoroutine(SpawnTarget());
    }
    private void Update()
    {
        
    }

    private IEnumerator SpawnTarget()
    {
        while (true)
        {
            if (gameManager.score % 10 == 0 && gameManager.score > 0 && spawnRate > initialSpawnRate / 5)
            {
                spawnRate /= 1.1f;
            }

            yield return new WaitForSeconds(spawnRate);

            Launch();
        }
    }

    public void Launch()
    {
        GameObject objectToSpawn;

        // Spawn a heart object every 'heartSpawnInterval' objects
        if (objectCount % heartSpawnInterval == 0)
        {
            Debug.Log("Spawning a heart!");
            objectToSpawn = heartObject;
        }
        else if (objectCount == nextBombSpawn)  // Compare objectCount to nextBombSpawn
        {

            Debug.Log("Spawning a bomb!");
            objectToSpawn = bombObject;

            // After spawning a bomb, set the next bomb spawn interval
            nextBombSpawn = objectCount + Random.Range(3, 6);


        }
        else
        {
            objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        }

        // Increment the object counter
        objectCount++;

        // Instantiate the object at the spawner's position with no rotation
        GameObject newObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        // Get the Rigidbody component from the new object
        Rigidbody rb = newObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Add a random upward force to the object
            rb.AddForce(RandomForce(), ForceMode.Impulse);

            // Add a random rotation to the object
            rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

            // Move the object to a random x position
            newObject.transform.position = RandomSpawnPos();
        }
    }


    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
