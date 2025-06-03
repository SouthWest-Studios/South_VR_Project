using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefab list (can add multiple)")]
    public List<GameObject> prefabList;

    [Header("Spawn control")]
    public bool spawn = false;

    [Header("Values to assign")]
    public Camera cameraToAssign;
    public List<Transform> targetList; // ← Nueva lista de objetivos

    [Header("Spawn offset")]
    public Vector3 spawnOffset = Vector3.zero;

    void Update()
    {
        if (spawn)
        {
            SpawnPrefab();
            spawn = false; // Spawn only once
        }
    }

    void SpawnPrefab()
    {
        if (prefabList == null || prefabList.Count == 0)
        {
            Debug.LogWarning("No prefab assigned!");
            return;
        }

        // Randomly select prefab
        int index = Random.Range(0, prefabList.Count);
        GameObject selectedPrefab = prefabList[index];

        // Spawn
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject instance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Assign references to the ActiveRagdollWalker
        ActiveRagdollWalker script = instance.GetComponent<ActiveRagdollWalker>();
        if (script != null)
        {
            script.playerCamera = cameraToAssign;

            // Asignar lista completa de targets
            script.targets = new List<Transform>(targetList);

            // Asignar el primer objetivo como objetivo actual si hay alguno
            if (script.targets.Count > 0)
            {
                script.SetCurrentTargetByIndex(0);

            }
        }
        else
        {
            Debug.LogWarning("Spawned object has no ActiveRagdollWalker script!");
        }
    }
}
