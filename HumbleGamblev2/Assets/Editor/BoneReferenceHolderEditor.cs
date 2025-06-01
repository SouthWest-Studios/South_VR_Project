using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoneReferenceHolder))]
public class BoneReferenceHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); 

        BoneReferenceHolder holder = (BoneReferenceHolder)target;

        if (GUILayout.Button("📌 Select All Bones"))
        {
            
            Transform[] allBones = new Transform[]
            {
                holder.pelvis,
                holder.leftHips,
                holder.leftKnee,
                holder.leftFoot,
                holder.rightHips,
                holder.rightKnee,
                holder.rightFoot,
                holder.leftArm,
                holder.leftElbow,
                holder.rightArm,
                holder.rightElbow,
                holder.spine,
                holder.head
            };

            
            List<Object> validBones = new List<Object>();
            foreach (Transform t in allBones)
            {
                if (t != null) validBones.Add(t.gameObject);
            }

            
            Selection.objects = validBones.ToArray();
        }
    }
}
