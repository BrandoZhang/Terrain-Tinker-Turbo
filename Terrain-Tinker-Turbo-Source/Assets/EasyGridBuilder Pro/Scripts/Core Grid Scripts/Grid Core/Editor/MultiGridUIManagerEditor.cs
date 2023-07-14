using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(MultiGridUIManager)), CanEditMultipleObjects]
    public class MultiGridUIManagerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty buildableObjectTypeCategorySO;

        SerializedProperty showInGridModeDefault;
        SerializedProperty showInGridModeBuild;
        SerializedProperty showInGridModeDestruction;
        SerializedProperty showInGridModeSelection;

        SerializedProperty inputGroupObject;
        SerializedProperty gridModeResetText;
        SerializedProperty gridHeightChangeText;
        SerializedProperty buildModeActiveText;
        SerializedProperty placementText;
        SerializedProperty listScrollText;
        SerializedProperty ghostRotateLText;
        SerializedProperty ghostRotateRText;
        SerializedProperty destructionModeActiveText;
        SerializedProperty destroyText;
        SerializedProperty selectionModeActiveText;
        SerializedProperty selectionText;
        SerializedProperty saveText;
        SerializedProperty loadText;

        SerializedProperty categorySection;
        SerializedProperty buildablesSection;
        SerializedProperty placeHolderCategory;
        SerializedProperty placeHolderBuildable;
        SerializedProperty placeHolderBuildableSectionCategory;
        SerializedProperty buildableListAnimator;

        SerializedProperty gridLevelUpButton;
        SerializedProperty gridLevelDownButton;
        #endregion

        private void OnEnable()
        {
            buildableObjectTypeCategorySO = serializedObject.FindProperty("buildableObjectTypeCategorySO");

            showInGridModeDefault = serializedObject.FindProperty("showInGridModeDefault");
            showInGridModeBuild = serializedObject.FindProperty("showInGridModeBuild");
            showInGridModeDestruction = serializedObject.FindProperty("showInGridModeDestruction");
            showInGridModeSelection = serializedObject.FindProperty("showInGridModeSelection");

            inputGroupObject = serializedObject.FindProperty("inputGroupObject");
            gridModeResetText = serializedObject.FindProperty("gridModeResetText");
            gridHeightChangeText = serializedObject.FindProperty("gridHeightChangeText");
            buildModeActiveText = serializedObject.FindProperty("buildModeActiveText");
            placementText = serializedObject.FindProperty("placementText");
            listScrollText = serializedObject.FindProperty("listScrollText");
            ghostRotateLText = serializedObject.FindProperty("ghostRotateLText");
            ghostRotateRText = serializedObject.FindProperty("ghostRotateRText");
            destructionModeActiveText = serializedObject.FindProperty("destructionModeActiveText");
            destroyText = serializedObject.FindProperty("destroyText");
            selectionModeActiveText = serializedObject.FindProperty("selectionModeActiveText");
            selectionText = serializedObject.FindProperty("selectionText");
            saveText = serializedObject.FindProperty("saveText");
            loadText = serializedObject.FindProperty("loadText");

            categorySection = serializedObject.FindProperty("categorySection");
            buildablesSection = serializedObject.FindProperty("buildablesSection");
            placeHolderCategory = serializedObject.FindProperty("placeHolderCategory");
            placeHolderBuildable = serializedObject.FindProperty("placeHolderBuildable");
            placeHolderBuildableSectionCategory = serializedObject.FindProperty("placeHolderBuildableSectionCategory");
            buildableListAnimator = serializedObject.FindProperty("buildableListAnimator");

            gridLevelUpButton = serializedObject.FindProperty("gridLevelUpButton");
            gridLevelDownButton = serializedObject.FindProperty("gridLevelDownButton");
        }

        public override void OnInspectorGUI()
        {
            MultiGridUIManager multiGridUIManager = (MultiGridUIManager)target;

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
            EditorGUILayout.LabelField("MULTI GRID UI MANAGER", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("", "GroupBox");
            

            EditorGUILayout.LabelField("Buildable Object Type Category SO Assets", style2);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(buildableObjectTypeCategorySO);

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.LabelField("Buildable List Menu Visibility", style2);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel++;
            multiGridUIManager.showBuildableListMenuData = EditorGUILayout.Foldout(multiGridUIManager.showBuildableListMenuData, "Buildables List Menu Data", true);

            if (multiGridUIManager.showBuildableListMenuData)
            {
                EditorGUILayout.PropertyField(showInGridModeDefault);
                EditorGUILayout.PropertyField(showInGridModeBuild);
                EditorGUILayout.PropertyField(showInGridModeDestruction);
                EditorGUILayout.PropertyField(showInGridModeSelection);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.LabelField("Help Menu Data (DO NOT CHANGE!)", style2);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel++;
            multiGridUIManager.showHelpMenuData = EditorGUILayout.Foldout(multiGridUIManager.showHelpMenuData, "Help Menu Data", true);

            if (multiGridUIManager.showHelpMenuData)
            {
                EditorGUILayout.PropertyField(inputGroupObject);
                EditorGUILayout.PropertyField(gridModeResetText);
                EditorGUILayout.PropertyField(gridHeightChangeText);
                EditorGUILayout.PropertyField(buildModeActiveText);
                EditorGUILayout.PropertyField(placementText);
                EditorGUILayout.PropertyField(listScrollText);
                EditorGUILayout.PropertyField(ghostRotateLText);
                EditorGUILayout.PropertyField(ghostRotateRText);
                EditorGUILayout.PropertyField(destructionModeActiveText);
                EditorGUILayout.PropertyField(destroyText);
                EditorGUILayout.PropertyField(selectionModeActiveText);
                EditorGUILayout.PropertyField(selectionText);
                EditorGUILayout.PropertyField(saveText);
                EditorGUILayout.PropertyField(loadText);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.LabelField("Buildables Menu Data (DO NOT CHANGE!)", style2);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel++;
            multiGridUIManager.showBuildablesMenuData = EditorGUILayout.Foldout(multiGridUIManager.showBuildablesMenuData, "Buildables Menu Data", true);

            if (multiGridUIManager.showBuildablesMenuData)
            {
                EditorGUILayout.PropertyField(categorySection);
                EditorGUILayout.PropertyField(buildablesSection);
                EditorGUILayout.PropertyField(placeHolderCategory);
                EditorGUILayout.PropertyField(placeHolderBuildable);
                EditorGUILayout.PropertyField(placeHolderBuildableSectionCategory);
                EditorGUILayout.PropertyField(buildableListAnimator);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            GUILayout.BeginVertical("", "GroupBox");

            EditorGUILayout.LabelField("Vertical Grid Menu Data (DO NOT CHANGE!)", style2);

            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel++;
            multiGridUIManager.showVerticalGridMenuData = EditorGUILayout.Foldout(multiGridUIManager.showVerticalGridMenuData, "Vertical Grid Menu Data", true);

            if (multiGridUIManager.showVerticalGridMenuData)
            {
                EditorGUILayout.PropertyField(gridLevelUpButton);
                EditorGUILayout.PropertyField(gridLevelDownButton);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
