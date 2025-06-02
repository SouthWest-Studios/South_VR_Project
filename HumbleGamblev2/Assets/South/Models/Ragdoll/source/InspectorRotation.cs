using UnityEngine;

[ExecuteAlways] 
public class InspectorRotation : MonoBehaviour
{

    public Vector3 rotationEuler;

    private void Update()
    {

        transform.rotation = Quaternion.Euler(rotationEuler);
    }


    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(rotationEuler);
    }
}
