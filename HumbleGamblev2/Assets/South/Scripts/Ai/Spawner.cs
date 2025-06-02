using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Prefab �б�����Ӷ����")]
    public List<GameObject> prefabList;

    [Header("�Ƿ����ɣ�")]
    public bool spawn = false;

    [Header("��ֵ����")]
    public Camera cameraToAssign;
    public Transform targetTransform;
    public Transform destination2Transform;

    [Header("����λ��ƫ��")]
    public Vector3 spawnOffset = Vector3.zero;

    void Update()
    {
        if (spawn)
        {
            SpawnPrefab();
            spawn = false; // ֻ����һ��
        }
    }

    void SpawnPrefab()
    {
        if (prefabList == null || prefabList.Count == 0)
        {
            Debug.LogWarning("û������ prefab��");
            return;
        }

        // ���ѡ�� prefab
        int index = Random.Range(0, prefabList.Count);
        GameObject selectedPrefab = prefabList[index];

        // ����
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject instance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // ��ȡ�ű�����ֵ
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
            Debug.LogWarning("���ɵ�������û�� MyPrefabScript �ű���");
        }
    }
}
