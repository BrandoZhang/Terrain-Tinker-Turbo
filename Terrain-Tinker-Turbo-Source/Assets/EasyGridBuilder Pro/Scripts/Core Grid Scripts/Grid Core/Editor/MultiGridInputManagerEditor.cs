using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(MultiGridInputManager)), CanEditMultipleObjects]
    public class MultiGridInputManagerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty easyGridBuilderProInputsSO;
        SerializedProperty gridObjectSelector;
        #endregion

        private void OnEnable()
        {
            easyGridBuilderProInputsSO = serializedObject.FindProperty("easyGridBuilderProInputsSO");
            gridObjectSelector = serializedObject.FindProperty("gridObjectSelector");
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
            EditorGUILayout.LabelField("MULTI GRID INPUT MANAGER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(easyGridBuilderProInputsSO);
            EditorGUILayout.PropertyField(gridObjectSelector);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
