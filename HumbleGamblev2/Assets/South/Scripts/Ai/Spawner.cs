using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefab 列表（可添加多个）")]
    public List<GameObject> prefabList;

    [Header("是否生成？")]
    public bool spawn = false;

    [Header("赋值内容")]
    public Camera cameraToAssign;
    public Transform targetTransform;
    public Transform destination2Transform;

    [Header("生成位置偏移")]
    public Vector3 spawnOffset = Vector3.zero;

    void Update()
    {
        if (spawn)
        {
            SpawnPrefab();
            spawn = false; // 只生成一次
        }
    }

    void SpawnPrefab()
    {
        if (prefabList == null || prefabList.Count == 0)
        {
            Debug.LogWarning("没有设置 prefab！");
            return;
        }

        // 随机选择 prefab
        int index = Random.Range(0, prefabList.Count);
        GameObject selectedPrefab = prefabList[index];

        // 生成
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject instance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // 获取脚本并赋值
        ActiveRagdollWalker script = instance.GetComponent<ActiveRagdollWalker>();
        if (script != null)
        {
            script.playerCamera = cameraToAssign;
            script.target = targetTransform;
            script.destination2 = destination2Transform;
            script.setCurrenTarget(script.target);
        }
        else
        {
            Debug.LogWarning("生成的物体上没有 MyPrefabScript 脚本！");
        }
    }
}
