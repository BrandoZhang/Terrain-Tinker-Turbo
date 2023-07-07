using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(BuildableFreeObject)), CanEditMultipleObjects]
    public class BuildableFreeObjectEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty buildableFreeObjectTypeSO;

        SerializedProperty rotateObjectForXY;
        SerializedProperty rotateObjectForXZ;
        SerializedProperty rotateForXY;
        SerializedProperty objectScale;
        SerializedProperty objectCenter;
        SerializedProperty objectCustomPivot;
        #endregion

        private void OnEnable()
        {
            buildableFreeObjectTypeSO = serializedObject.FindProperty("buildableFreeObjectTypeSO");

            rotateObjectForXY = serializedObject.FindProperty("rotateObjectForXY");
            rotateObjectForXZ = serializedObject.FindProperty("rotateObjectForXZ");
            rotateForXY = serializedObject.FindProperty("rotateForXY");
            objectScale = serializedObject.FindProperty("objectScale");
            objectCenter = serializedObject.FindProperty("objectCenter");
            objectCustomPivot = serializedObject.FindProperty("objectCustomPivot");
        }

        public override void OnInspectorGUI()
        {
            BuildableFreeObject buildableFreeObject = (BuildableFreeObject)target;

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
            EditorGUILayout.LabelField("BUILDABLE FREE OBJECT", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(buildableFreeObjectTypeSO);
            EditorGUILayout.PropertyField(rotateObjectForXY);
            if (buildableFreeObject.rotateObjectForXY) buildableFreeObject.rotateObjectForXZ = false;
            EditorGUILayout.PropertyField(rotateObjectForXZ);
            if (buildableFreeObject.rotateObjectForXZ) buildableFreeObject.rotateObjectForXY = false;

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Auto Pivot Calculator", style2);
            EditorGUILayout.Separator();

            if (GUILayout.Button("Calculate Pivot", GUILayout.Height(20)))
            {
                buildableFreeObject.AutoCalculatePivotAndSize();
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