using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(BuildableFreeObjectSnapper)), CanEditMultipleObjects]
    public class BuildableFreeObjectSnapperEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty snappingTriggerSize;
        #endregion

        private void OnEnable()
        {
            snappingTriggerSize = serializedObject.FindProperty("snappingTriggerSize");
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
            EditorGUILayout.LabelField("BUILDABLE FREE OBJECT SNAPPER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(snappingTriggerSize);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}