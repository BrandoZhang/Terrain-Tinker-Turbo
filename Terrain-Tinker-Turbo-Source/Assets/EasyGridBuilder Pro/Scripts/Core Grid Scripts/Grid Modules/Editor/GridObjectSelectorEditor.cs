using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(GridObjectSelector)), CanEditMultipleObjects]
    public class GridObjectSelectorEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty deselectOnFalseSelect;
        SerializedProperty selectedObject;
        #endregion

        private void OnEnable()
        {
            deselectOnFalseSelect = serializedObject.FindProperty("deselectOnFalseSelect");
            selectedObject = serializedObject.FindProperty("selectedObject");
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
            EditorGUILayout.LabelField("GRID OBJECT SELECTOR", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
                                        
            EditorGUILayout.PropertyField(deselectOnFalseSelect);
            EditorGUILayout.PropertyField(selectedObject);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}