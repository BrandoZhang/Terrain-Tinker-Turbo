using UnityEditor;
using UnityEngine;

namespace SoulGames.Utilities
{
    [CustomEditor(typeof(WASDCameraController)), CanEditMultipleObjects]
    public class WASDCameraControllerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty cameraTransform;

        SerializedProperty normalSpeed;
        SerializedProperty fastSpeed;
        SerializedProperty movementLerpTime;

        SerializedProperty rotationAmount;
        SerializedProperty rotationLerpTime;

        SerializedProperty zoomAmount;
        SerializedProperty minZoom;
        SerializedProperty maxZoom;
        SerializedProperty zoomLerpTime;

        SerializedProperty setHeigthByRaycast;
        SerializedProperty raycastLayerMask;

        SerializedProperty upKey;
        SerializedProperty downKey;
        SerializedProperty leftKey;
        SerializedProperty rightKey;
        SerializedProperty speedUpKey;
        SerializedProperty rotateLeftKey;
        SerializedProperty rotateRightKey;
        SerializedProperty zoomInKey;
        SerializedProperty zoomOutKey;
        #endregion

        private void OnEnable()
        {
            cameraTransform = serializedObject.FindProperty("cameraTransform");

            normalSpeed = serializedObject.FindProperty("normalSpeed");
            fastSpeed = serializedObject.FindProperty("fastSpeed");
            movementLerpTime = serializedObject.FindProperty("movementLerpTime");

            rotationAmount = serializedObject.FindProperty("rotationAmount");
            rotationLerpTime = serializedObject.FindProperty("rotationLerpTime");

            zoomAmount = serializedObject.FindProperty("zoomAmount");
            minZoom = serializedObject.FindProperty("minZoom");
            maxZoom = serializedObject.FindProperty("maxZoom");
            zoomLerpTime = serializedObject.FindProperty("zoomLerpTime");

            setHeigthByRaycast = serializedObject.FindProperty("setHeigthByRaycast");
            raycastLayerMask = serializedObject.FindProperty("raycastLayerMask");

            upKey = serializedObject.FindProperty("upKey");
            downKey = serializedObject.FindProperty("downKey");
            leftKey = serializedObject.FindProperty("leftKey");
            rightKey = serializedObject.FindProperty("rightKey");
            speedUpKey = serializedObject.FindProperty("speedUpKey");
            rotateLeftKey = serializedObject.FindProperty("rotateLeftKey");
            rotateRightKey = serializedObject.FindProperty("rotateRightKey");
            zoomInKey = serializedObject.FindProperty("zoomInKey");
            zoomOutKey = serializedObject.FindProperty("zoomOutKey");
        }

        public override void OnInspectorGUI()
        {
            WASDCameraController wASDCameraController = (WASDCameraController)target;
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
            EditorGUILayout.LabelField("WASD CAMERA CONTROLLER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Camera Reference", style2);

            EditorGUILayout.PropertyField(cameraTransform);

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Camera Movement", style2);

            EditorGUILayout.PropertyField(normalSpeed);
            EditorGUILayout.PropertyField(fastSpeed);
            EditorGUILayout.PropertyField(movementLerpTime);

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Camera Rotation", style2);

            EditorGUILayout.PropertyField(rotationAmount);
            EditorGUILayout.PropertyField(rotationLerpTime);

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Camera Zoom", style2);

            EditorGUILayout.PropertyField(zoomAmount);
            EditorGUILayout.PropertyField(minZoom);
            EditorGUILayout.PropertyField(maxZoom);
            EditorGUILayout.PropertyField(zoomLerpTime);

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Camera Height", style2);

            EditorGUILayout.PropertyField(setHeigthByRaycast);
            if (wASDCameraController.setHeigthByRaycast)
            {
                EditorGUILayout.PropertyField(raycastLayerMask);
            }
            
            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Inputs", style2);

            EditorGUILayout.PropertyField(upKey);
            EditorGUILayout.PropertyField(downKey);
            EditorGUILayout.PropertyField(leftKey);
            EditorGUILayout.PropertyField(rightKey);
            EditorGUILayout.PropertyField(speedUpKey);
            EditorGUILayout.PropertyField(rotateLeftKey);
            EditorGUILayout.PropertyField(rotateRightKey);
            EditorGUILayout.PropertyField(zoomInKey);
            EditorGUILayout.PropertyField(zoomOutKey);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}