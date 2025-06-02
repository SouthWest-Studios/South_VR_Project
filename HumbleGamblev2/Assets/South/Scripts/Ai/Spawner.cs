using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

    public List<GameObject> prefabList;


    public bool spawn = false;


    public Camera cameraToAssign;
    public Transform targetTransform;
    public Transform destination2Transform;

    public Vector3 spawnOffset = Vector3.zero;

    void Update()
    {
        if (spawn)
        {
            SpawnPrefab();
            spawn = false; 
        }
    }

    void SpawnPrefab()
    {
        if (prefabList == null || prefabList.Count == 0)
        {

            return;
        }

        int index = Random.Range(0, prefabList.Count);
        GameObject selectedPrefab = prefabList[index];


        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject instance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);


        ActiveRagdollWalker script = instance.GetComponent<ActiveRagdollWalker>();
        if (script != null)
        {
            script.target = targetTransform;
            script.destination2 = destination2Transform;
            script.SetCurrentTarget(script.target);
        }
        else
        {

        }
    }
}
