using System.Collections;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public GameObject logPrefab; 
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    private bool isGameActive = true; // Control log spawning

    void Start()
    {
        StartCoroutine(SpawnLogs());
    }

    IEnumerator SpawnLogs()
    {
        while (isGameActive)
        {
            Instantiate(logPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Function to stop spawning when time runs out
    public void StopSpawning()
    {
        isGameActive = false;
    }
}
