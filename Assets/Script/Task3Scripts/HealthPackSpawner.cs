using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab;
    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnHealthPacks();
    }

    private void SpawnHealthPacks()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(healthPackPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
