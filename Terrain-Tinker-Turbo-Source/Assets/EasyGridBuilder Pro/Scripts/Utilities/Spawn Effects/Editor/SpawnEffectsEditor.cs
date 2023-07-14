using UnityEditor;
using UnityEngine;

namespace SoulGames.Utilities
{
    [CustomEditor(typeof(SpawnEffects)), CanEditMultipleObjects]
    public class SpawnEffectsEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty useScalingEffect;
        SerializedProperty useScaleEffectX;
        SerializedProperty useScaleEffectY;
        SerializedProperty useScaleEffectZ;
        SerializedProperty ScaleEffectAnimationCurve;

        SerializedProperty useSpawnGameObjects;
        SerializedProperty spawnGameObjects;
        SerializedProperty spawnRandomOneFromList;
        SerializedProperty spawnStartDelay;
        SerializedProperty spawnLocalPosition;
        SerializedProperty spawnLocalRotation;
        SerializedProperty spawnLocalScale;
        SerializedProperty destroySpawnnedAfterDelay;
        SerializedProperty destroyDelay;
        #endregion

        private void OnEnable()
        {
            useScalingEffect = serializedObject.FindProperty("useScalingEffect");
            useScaleEffectX = serializedObject.FindProperty("useScaleEffectX");
            useScaleEffectY = serializedObject.FindProperty("useScaleEffectY");
            useScaleEffectZ = serializedObject.FindProperty("useScaleEffectZ");
            ScaleEffectAnimationCurve = serializedObject.FindProperty("ScaleEffectAnimationCurve");

            useSpawnGameObjects = serializedObject.FindProperty("useSpawnGameObjects");
            spawnGameObjects = serializedObject.FindProperty("spawnGameObjects");
            spawnRandomOneFromList = serializedObject.FindProperty("spawnRandomOneFromList");
            spawnStartDelay = serializedObject.FindProperty("spawnStartDelay");
            spawnLocalPosition = serializedObject.FindProperty("spawnLocalPosition");
            spawnLocalRotation = serializedObject.FindProperty("spawnLocalRotation");
            spawnLocalScale = serializedObject.FindProperty("spawnLocalScale");
            destroySpawnnedAfterDelay = serializedObject.FindProperty("destroySpawnnedAfterDelay");
            destroyDelay = serializedObject.FindProperty("destroyDelay");
        }

        public override void OnInspectorGUI()
        {
            SpawnEffects spawnEffects = (SpawnEffects)target;

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
            EditorGUILayout.LabelField("SPAWN EFFECTS", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Scalling Effect", style2);

            EditorGUILayout.PropertyField(useScalingEffect);
            
            if (spawnEffects.useScalingEffect)
            {
                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                spawnEffects.showObjectScallingData = EditorGUILayout.Foldout(spawnEffects.showObjectScallingData, "Object Scalling Data", true);
                                        
                if (spawnEffects.showObjectScallingData)
                {
                    EditorGUILayout.PropertyField(useScaleEffectX);
                    EditorGUILayout.PropertyField(useScaleEffectY);
                    EditorGUILayout.PropertyField(useScaleEffectZ);
                    EditorGUILayout.PropertyField(ScaleEffectAnimationCurve);
                    
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Objects Spawn", style2);

            EditorGUILayout.PropertyField(useSpawnGameObjects);

            if (spawnEffects.useSpawnGameObjects)
            {
                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                spawnEffects.showObjectSpawnData = EditorGUILayout.Foldout(spawnEffects.showObjectSpawnData, "Objects Spawn Data", true);
                                        
                if (spawnEffects.showObjectSpawnData)
                {
                    EditorGUILayout.PropertyField(spawnGameObjects);
                    EditorGUILayout.PropertyField(spawnRandomOneFromList);
                    EditorGUILayout.PropertyField(spawnStartDelay);
                    EditorGUILayout.PropertyField(spawnLocalPosition);
                    EditorGUILayout.PropertyField(spawnLocalRotation);
                    EditorGUILayout.PropertyField(spawnLocalScale);
                    EditorGUILayout.PropertyField(destroySpawnnedAfterDelay);
                    if (spawnEffects.destroySpawnnedAfterDelay)
                    {
                        EditorGUILayout.PropertyField(destroyDelay);
                    }
                }
            
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}