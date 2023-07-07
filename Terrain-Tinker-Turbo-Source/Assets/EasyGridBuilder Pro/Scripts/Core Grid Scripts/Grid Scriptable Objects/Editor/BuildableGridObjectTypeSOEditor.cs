using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(BuildableGridObjectTypeSO)), CanEditMultipleObjects]
    public class BuildableGridObjectTypeSOEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty objectName;
        SerializedProperty objectDescription;
        SerializedProperty objectToolTipDescription;
        SerializedProperty buildableCategorySO;
        SerializedProperty objectIcon;

        SerializedProperty objectPrefab;
        SerializedProperty ghostPrefab;
        SerializedProperty placeableGhostMaterial;
        SerializedProperty notPlaceableGhostMaterial;
        SerializedProperty setBuiltObjectLayer;
        SerializedProperty builtObjectLayer;

        SerializedProperty showGridBelowObject;
        SerializedProperty objectGridCanvas;
        SerializedProperty gridImageSprite;
        SerializedProperty gridImagePlaceableColor;
        SerializedProperty gridImageNotPlaceableColor;

        SerializedProperty holdToPlace;
        SerializedProperty placeAndDeselect;
        //SerializedProperty objectMovable;

        SerializedProperty enableBuildCondition;
        SerializedProperty buildConditionSO;
        #endregion

        private void OnEnable()
        {
            objectName = serializedObject.FindProperty("objectName");
            objectDescription = serializedObject.FindProperty("objectDescription");
            objectToolTipDescription = serializedObject.FindProperty("objectToolTipDescription");
            buildableCategorySO = serializedObject.FindProperty("buildableCategorySO");
            objectIcon = serializedObject.FindProperty("objectIcon");

            objectPrefab = serializedObject.FindProperty("objectPrefab");
            ghostPrefab = serializedObject.FindProperty("ghostPrefab");
            placeableGhostMaterial = serializedObject.FindProperty("placeableGhostMaterial");
            notPlaceableGhostMaterial = serializedObject.FindProperty("notPlaceableGhostMaterial");
            setBuiltObjectLayer = serializedObject.FindProperty("setBuiltObjectLayer");
            builtObjectLayer = serializedObject.FindProperty("builtObjectLayer");

            showGridBelowObject = serializedObject.FindProperty("showGridBelowObject");
            objectGridCanvas = serializedObject.FindProperty("objectGridCanvas");
            gridImageSprite = serializedObject.FindProperty("gridImageSprite");
            gridImagePlaceableColor = serializedObject.FindProperty("gridImagePlaceableColor");
            gridImageNotPlaceableColor = serializedObject.FindProperty("gridImageNotPlaceableColor");

            holdToPlace = serializedObject.FindProperty("holdToPlace");
            placeAndDeselect = serializedObject.FindProperty("placeAndDeselect");
            //objectMovable = serializedObject.FindProperty("objectMovable");

            enableBuildCondition = serializedObject.FindProperty("enableBuildCondition");
            buildConditionSO = serializedObject.FindProperty("buildConditionSO");
        }

        public override void OnInspectorGUI()
        {
            BuildableGridObjectTypeSO buildableGridObjectTypeSO = (BuildableGridObjectTypeSO)target;
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
            EditorGUILayout.LabelField("BUILDABLE GRID OBJECT TYPE SO", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Basic Data", style2);

            EditorGUILayout.PropertyField(objectName);
            EditorGUILayout.PropertyField(objectDescription);
            EditorGUILayout.PropertyField(objectToolTipDescription);
            EditorGUILayout.PropertyField(buildableCategorySO);
            EditorGUILayout.PropertyField(objectIcon);

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Data & Visual", style2);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(objectPrefab);

            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(ghostPrefab);
            EditorGUILayout.PropertyField(placeableGhostMaterial);
            EditorGUILayout.PropertyField(notPlaceableGhostMaterial);
            EditorGUILayout.PropertyField(setBuiltObjectLayer);
            if (buildableGridObjectTypeSO.setBuiltObjectLayer)
            {
                EditorGUILayout.PropertyField(builtObjectLayer);
            }

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Underline Grid", style2);

            EditorGUILayout.PropertyField(showGridBelowObject);
            if (buildableGridObjectTypeSO.showGridBelowObject)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(objectGridCanvas);
                if (GUILayout.Button("Auto Set", GUILayout.Width(60)))
                {
                    buildableGridObjectTypeSO.objectGridCanvas = (Canvas)AssetDatabase.LoadAssetAtPath("Assets/EasyGridBuilder Pro/Prefabs/Canvas Grid Samples/Grid Canvas.prefab", typeof(Canvas));
                } 
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(gridImageSprite);
                EditorGUILayout.PropertyField(gridImagePlaceableColor);
                EditorGUILayout.PropertyField(gridImageNotPlaceableColor);
            }

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            EditorGUILayout.LabelField("Object Placement & Other", style2);

            EditorGUILayout.PropertyField(holdToPlace);
            EditorGUILayout.PropertyField(placeAndDeselect);
            //EditorGUILayout.PropertyField(objectMovable);

            EditorGUILayout.PropertyField(enableBuildCondition);
            if (buildableGridObjectTypeSO.enableBuildCondition)
            {
                EditorGUILayout.PropertyField(buildConditionSO);
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}