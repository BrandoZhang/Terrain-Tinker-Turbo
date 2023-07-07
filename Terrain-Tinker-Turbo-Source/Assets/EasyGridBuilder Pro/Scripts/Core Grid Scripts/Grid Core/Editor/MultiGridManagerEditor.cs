using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(MultiGridManager)), CanEditMultipleObjects]
    public class MultiGridManagerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty mouseColliderLayerMask;
        #endregion

        private void OnEnable()
        {
            mouseColliderLayerMask = serializedObject.FindProperty("mouseColliderLayerMask");
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
            EditorGUILayout.LabelField("MULTI GRID MANAGER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.PropertyField(mouseColliderLayerMask);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}