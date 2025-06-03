using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefab list (can add multiple)")]
    public List<GameObject> prefabList;

    [Header("Spawn timing")]
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    [Header("Values to assign")]
    public Camera cameraToAssign;
    public List<Transform> targetList;

    [Header("Spawn offset")]
    public Vector3 spawnOffset = Vector3.zero;

    public static Spawner instance;

    public Coroutine actualCoroutine;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Iniciar la corutina de spawn continuo
        startSpawnLoop();
    }

    public void startSpawnLoop()
    {
        SpawnPrefab();
        actualCoroutine = StartCoroutine(SpawnLoop());
    }

    public void stopSpawn()
    {
        StopCoroutine(actualCoroutine);
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnPrefab();
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

            script.targets = new List<Transform>(targetList);

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
