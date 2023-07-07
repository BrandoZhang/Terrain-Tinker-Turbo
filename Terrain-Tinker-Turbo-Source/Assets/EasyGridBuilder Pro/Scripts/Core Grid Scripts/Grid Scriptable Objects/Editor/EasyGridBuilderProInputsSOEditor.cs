using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(EasyGridBuilderProInputsSO)), CanEditMultipleObjects]
    public class EasyGridBuilderProInputsSOEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty gridModeResetKey;

        SerializedProperty gridHeightChangeKey;

        SerializedProperty useBuildModeActivationKey;
        SerializedProperty buildModeActivationKey;
        SerializedProperty buildablePlacementKey;
        SerializedProperty buildableListScrollKey;
        SerializedProperty ghostRotateLeftKey;
        SerializedProperty ghostRotateRightKey;

        SerializedProperty useDestructionModeActivationKey;
        SerializedProperty destructionModeActivationKey;
        SerializedProperty buildableDestroyKey;

        SerializedProperty useSelectionModeActivationKey;
        SerializedProperty selectionModeActivationKey;
        SerializedProperty buildableSelectionKey;

        // SerializedProperty useMoveModeActivationKey;
        // SerializedProperty moveModeActivationKey;
        // SerializedProperty buildableMoveKey;

        SerializedProperty gridSaveKey;
        SerializedProperty gridLoadKey;
        #endregion

        private void OnEnable()
        {
            gridModeResetKey = serializedObject.FindProperty("gridModeResetKey");

            gridHeightChangeKey = serializedObject.FindProperty("gridHeightChangeKey");

            useBuildModeActivationKey = serializedObject.FindProperty("useBuildModeActivationKey");
            buildModeActivationKey = serializedObject.FindProperty("buildModeActivationKey");
            buildablePlacementKey = serializedObject.FindProperty("buildablePlacementKey");
            buildableListScrollKey = serializedObject.FindProperty("buildableListScrollKey");
            ghostRotateLeftKey = serializedObject.FindProperty("ghostRotateLeftKey");
            ghostRotateRightKey = serializedObject.FindProperty("ghostRotateRightKey");

            useDestructionModeActivationKey = serializedObject.FindProperty("useDestructionModeActivationKey");
            destructionModeActivationKey = serializedObject.FindProperty("destructionModeActivationKey");
            buildableDestroyKey = serializedObject.FindProperty("buildableDestroyKey");

            useSelectionModeActivationKey = serializedObject.FindProperty("useSelectionModeActivationKey");
            selectionModeActivationKey = serializedObject.FindProperty("selectionModeActivationKey");
            buildableSelectionKey = serializedObject.FindProperty("buildableSelectionKey");

            // useMoveModeActivationKey = serializedObject.FindProperty("useMoveModeActivationKey");
            // moveModeActivationKey = serializedObject.FindProperty("moveModeActivationKey");
            // buildableMoveKey = serializedObject.FindProperty("buildableMoveKey");

            gridSaveKey = serializedObject.FindProperty("gridSaveKey");
            gridLoadKey = serializedObject.FindProperty("gridLoadKey");
        }

        public override void OnInspectorGUI()
        {
            EasyGridBuilderProInputsSO easyGridBuilderProInputsSO = (EasyGridBuilderProInputsSO)target;
            var style = new GUIStyle(GUI.skin.label)
            {
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
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("EASY GRID BUILDER PRO INPUTS SO", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Grid Mode Reset", style2);

            EditorGUILayout.PropertyField(gridModeResetKey);

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Grid Pro Heigth Change", style2);

            EditorGUILayout.PropertyField(gridHeightChangeKey);

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Placement", style2);

            EditorGUILayout.PropertyField(useBuildModeActivationKey);
            if (easyGridBuilderProInputsSO.useBuildModeActivationKey)
            {
                EditorGUILayout.PropertyField(buildModeActivationKey);
            }
            EditorGUILayout.PropertyField(buildablePlacementKey);
            EditorGUILayout.PropertyField(buildableListScrollKey);
            EditorGUILayout.PropertyField(ghostRotateLeftKey);
            EditorGUILayout.PropertyField(ghostRotateRightKey);

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Destruction", style2);

            EditorGUILayout.PropertyField(useDestructionModeActivationKey);
            if (easyGridBuilderProInputsSO.useDestructionModeActivationKey)
            {
                EditorGUILayout.PropertyField(destructionModeActivationKey);
            }
            EditorGUILayout.PropertyField(buildableDestroyKey);

           EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Selection", style2);

            EditorGUILayout.PropertyField(useSelectionModeActivationKey);
            if (easyGridBuilderProInputsSO.useSelectionModeActivationKey)
            {
                EditorGUILayout.PropertyField(selectionModeActivationKey);
            }
            EditorGUILayout.PropertyField(buildableSelectionKey);

            EditorGUILayout.EndVertical();

            //GUILayout.BeginVertical("", "GroupBox");
            //EditorGUILayout.LabelField("Object Moving", style2);

            //EditorGUILayout.PropertyField(useMoveModeActivationKey);
            // if (easyGridBuilderProInputsSO.useMoveModeActivationKey)
            // {
            //     EditorGUILayout.PropertyField(moveModeActivationKey);
            // }
            //EditorGUILayout.PropertyField(buildableMoveKey);

            //EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Grid Save & Load", style2);

            EditorGUILayout.PropertyField(gridSaveKey);
            EditorGUILayout.PropertyField(gridLoadKey);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}