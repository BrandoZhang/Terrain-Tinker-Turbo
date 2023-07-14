using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(UITriggerAnimation)), CanEditMultipleObjects]
    public class UITriggerAnimationEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty animator;
        SerializedProperty firstTriggerParameter;
        SerializedProperty secondTriggerParameter;


        SerializedProperty firstObject;
        SerializedProperty secondObject;

        SerializedProperty toggleObject;
        #endregion

        private void OnEnable()
        {
            animator = serializedObject.FindProperty("animator");
            firstTriggerParameter = serializedObject.FindProperty("firstTriggerParameter");
            secondTriggerParameter = serializedObject.FindProperty("secondTriggerParameter");

            firstObject = serializedObject.FindProperty("firstObject");
            secondObject = serializedObject.FindProperty("secondObject");

            toggleObject = serializedObject.FindProperty("toggleObject");
        }

        public override void OnInspectorGUI()
        {
            UITriggerAnimation uITriggerAnimation = (UITriggerAnimation)target;

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
            EditorGUILayout.LabelField("UI TRIGGER ANIMATION", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Animator Trigger", style2);
            EditorGUI.indentLevel++;
                                        
            EditorGUILayout.PropertyField(animator);
            EditorGUILayout.PropertyField(firstTriggerParameter);
            EditorGUILayout.PropertyField(secondTriggerParameter);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Objects Enable / Disable", style2);
            EditorGUI.indentLevel++;
                                        
            EditorGUILayout.PropertyField(firstObject);
            EditorGUILayout.PropertyField(secondObject);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Toggle", style2);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(toggleObject);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}