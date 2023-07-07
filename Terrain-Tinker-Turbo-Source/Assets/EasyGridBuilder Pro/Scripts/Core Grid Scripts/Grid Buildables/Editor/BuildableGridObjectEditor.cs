using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(BuildableGridObject)), CanEditMultipleObjects]
    public class BuildableGridObjectEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty buildableGridObjectTypeSO;

        SerializedProperty rotateObjectForXY;
        SerializedProperty rotateObjectForXZ;
        SerializedProperty rotateForXY;
        SerializedProperty objectScale;
        SerializedProperty objectCenter;
        SerializedProperty objectCustomPivot;
        #endregion

        private void OnEnable()
        {
            buildableGridObjectTypeSO = serializedObject.FindProperty("buildableGridObjectTypeSO");
            
            rotateObjectForXY = serializedObject.FindProperty("rotateObjectForXY");
            rotateObjectForXZ = serializedObject.FindProperty("rotateObjectForXZ");
            rotateForXY = serializedObject.FindProperty("rotateForXY");
            objectScale = serializedObject.FindProperty("objectScale");
            objectCenter = serializedObject.FindProperty("objectCenter");
            objectCustomPivot = serializedObject.FindProperty("objectCustomPivot");
        }

        public override void OnInspectorGUI()
        {
            BuildableGridObject buildableGridObject = (BuildableGridObject)target;

            var style = new GUIStyle(GUI.skin.label)
            {
                fixedHeight = 25f,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 20,
            };
            var style2 = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
            };

            serializedObject.Update();

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginVertical("textArea", GUILayout.Height(30));
            EditorGUILayout.LabelField("BUILDABLE GRID OBJECT", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(buildableGridObjectTypeSO);
            EditorGUILayout.PropertyField(rotateObjectForXY);
            if (buildableGridObject.rotateObjectForXY) buildableGridObject.rotateObjectForXZ = false;
            EditorGUILayout.PropertyField(rotateObjectForXZ);
            if (buildableGridObject.rotateObjectForXZ) buildableGridObject.rotateObjectForXY = false;

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Auto Pivot & Size Calculator", style2);
            EditorGUILayout.Separator();
            
            if (GUILayout.Button("Calculate Size & Pivot", GUILayout.Height(20)))
            {
                buildableGridObject.AutoCalculatePivotAndSize();
            } 
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(rotateForXY);
            EditorGUILayout.PropertyField(objectScale);
            EditorGUILayout.PropertyField(objectCenter);
            EditorGUILayout.PropertyField(objectCustomPivot);

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}