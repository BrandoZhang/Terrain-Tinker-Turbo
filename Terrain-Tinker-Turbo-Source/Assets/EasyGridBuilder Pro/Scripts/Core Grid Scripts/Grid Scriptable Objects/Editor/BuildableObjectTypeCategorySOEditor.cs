using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(BuildableObjectTypeCategorySO)), CanEditMultipleObjects]
    public class BuildableObjectTypeCategorySOEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty categoryName;
        SerializedProperty categoryIcon;
        #endregion

        private void OnEnable()
        {
            categoryName = serializedObject.FindProperty("categoryName");
            categoryIcon = serializedObject.FindProperty("categoryIcon");
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
            var style2 = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
            };

            serializedObject.Update();

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.BeginVertical("textArea", GUILayout.Height(30));
            EditorGUILayout.LabelField("BUILDABLE OBJECT TYPE CATEGORY SO", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Category Data", style2);

            EditorGUILayout.PropertyField(categoryName);
            EditorGUILayout.PropertyField(categoryIcon);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}