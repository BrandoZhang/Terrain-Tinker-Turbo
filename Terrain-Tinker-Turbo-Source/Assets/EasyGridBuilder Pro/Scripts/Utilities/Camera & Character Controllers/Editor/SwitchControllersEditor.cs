using UnityEditor;
using UnityEngine;

namespace SoulGames.Utilities
{
    [CustomEditor(typeof(SwitchControllers)), CanEditMultipleObjects]
    public class SwitchControllersEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty startingActiveObjects;
        SerializedProperty mainCamStartingActiveParent;
        SerializedProperty switchingObjects;
        SerializedProperty mainCamSwitchingParent;
        SerializedProperty switchToggleKey;
        #endregion

        private void OnEnable()
        {
            startingActiveObjects = serializedObject.FindProperty("startingActiveObjects");
            mainCamStartingActiveParent = serializedObject.FindProperty("mainCamStartingActiveParent");
            switchingObjects = serializedObject.FindProperty("switchingObjects");
            mainCamSwitchingParent = serializedObject.FindProperty("mainCamSwitchingParent");
            switchToggleKey = serializedObject.FindProperty("switchToggleKey");
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
            EditorGUILayout.LabelField("SWITCH CONTROLLERS", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Start Active Objects", style2);

            EditorGUILayout.PropertyField(startingActiveObjects);
            EditorGUILayout.PropertyField(mainCamStartingActiveParent);
            
            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Switching Objects", style2);
            

            EditorGUILayout.PropertyField(switchingObjects);
            EditorGUILayout.PropertyField(mainCamSwitchingParent);
            
            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Inputs", style2);
            

            EditorGUILayout.PropertyField(switchToggleKey);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}