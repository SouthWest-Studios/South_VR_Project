//using System;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;

//[AttributeUsage(AttributeTargets.Field, Inherited = true)]
//public class DescriptionAttribute : PropertyAttribute
//{
//    public string text;

//    public DescriptionAttribute(string text)
//    {
//        this.text = text;
//    }
//}

//[CustomPropertyDrawer(typeof(DescriptionAttribute))]
//public class DescriptionRightDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        DescriptionAttribute descAttr = (DescriptionAttribute)attribute;

//        // Crea el texto combinado
//        string combinedLabel = label.text + " ";

//        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
//        GUIStyle italicStyle = new GUIStyle(EditorStyles.label);
//        italicStyle.fontStyle = FontStyle.Italic;
//        italicStyle.fontSize -= 2;
//        italicStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f); // Gris clarito

//        // Mide las partes
//        Vector2 labelSize = labelStyle.CalcSize(new GUIContent(label.text));
//        Vector2 descSize = italicStyle.CalcSize(new GUIContent("[" + descAttr.text + "]"));

//        // Rect para la etiqueta principal
//        Rect labelRect = new Rect(position.x, position.y, labelSize.x, position.height);
//        // Rect para la descripción
//        Rect descRect = new Rect(labelRect.xMax + 4, position.y, descSize.x, position.height);
//        // Rect para el campo
//        float fieldWidth = position.width - labelSize.x - descSize.x - 12;
//        Rect fieldRect = new Rect(position.x + position.width - fieldWidth, position.y, fieldWidth, position.height);

//        EditorGUI.LabelField(labelRect, label.text, labelStyle);
//        EditorGUI.LabelField(descRect, "[" + descAttr.text + "]", italicStyle);
//        EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
//    }
//}