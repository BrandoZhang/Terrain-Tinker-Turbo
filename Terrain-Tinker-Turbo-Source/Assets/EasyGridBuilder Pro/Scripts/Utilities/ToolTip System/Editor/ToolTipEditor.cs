using UnityEditor;
using UnityEngine;

namespace SoulGames.Utilities
{
    [CustomEditor(typeof(ToolTip)), CanEditMultipleObjects]
    public class TooltipEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty header;
        SerializedProperty content;
        SerializedProperty objectNameAsHeader;
        SerializedProperty objectBuildSOToolTipAsContent;
        #endregion

        private void OnEnable()
        {
            header = serializedObject.FindProperty("header");
            content = serializedObject.FindProperty("content");
            objectNameAsHeader = serializedObject.FindProperty("objectNameAsHeader");
            objectBuildSOToolTipAsContent = serializedObject.FindProperty("objectBuildSOToolTipAsContent");
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
            EditorGUILayout.LabelField("TOOLTIP", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(header);
            EditorGUILayout.PropertyField(content);
            EditorGUILayout.PropertyField(objectNameAsHeader);
            EditorGUILayout.PropertyField(objectBuildSOToolTipAsContent);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}