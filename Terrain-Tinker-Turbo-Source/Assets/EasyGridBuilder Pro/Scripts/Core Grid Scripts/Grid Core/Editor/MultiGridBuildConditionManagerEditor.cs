using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(MultiGridBuildConditionManager)), CanEditMultipleObjects]
    public class MultiGridBuildConditionManagerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty _buildableGridObjectTypeSOList;
        SerializedProperty _buildableEdgeObjectTypeSOList;
        SerializedProperty _buildableFreeObjectTypeSOList;
        #endregion

        private void OnEnable()
        {
            _buildableGridObjectTypeSOList = serializedObject.FindProperty("_buildableGridObjectTypeSOList");
            _buildableEdgeObjectTypeSOList = serializedObject.FindProperty("_buildableEdgeObjectTypeSOList");
            _buildableFreeObjectTypeSOList = serializedObject.FindProperty("_buildableFreeObjectTypeSOList");
        }

        public override void OnInspectorGUI()
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fixedHeight = 25f,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 20,
            };

            serializedObject.Update();

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginVertical("textArea", GUILayout.Height(30));
            EditorGUILayout.LabelField("MULTI GRID BUILD CONDITION MANAGER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_buildableGridObjectTypeSOList);
            EditorGUILayout.PropertyField(_buildableEdgeObjectTypeSOList);
            EditorGUILayout.PropertyField(_buildableFreeObjectTypeSOList);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}