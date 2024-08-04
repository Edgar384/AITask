using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnHealthPacks());
    }

    private IEnumerator SpawnHealthPacks()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(healthPackPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
