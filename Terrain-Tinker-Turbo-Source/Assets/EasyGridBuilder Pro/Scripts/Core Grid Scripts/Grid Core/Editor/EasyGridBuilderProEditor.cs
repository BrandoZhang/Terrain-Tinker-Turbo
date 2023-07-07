using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CustomEditor(typeof(EasyGridBuilderPro)), CanEditMultipleObjects]
    public class EasyGridBuilderProEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty buildableGridObjectTypeSOList;
        SerializedProperty buildableEdgeObjectTypeSOList;
        SerializedProperty buildableFreeObjectTypeSOList;

        SerializedProperty gridAxis;
        SerializedProperty gridWidth;
        SerializedProperty gridLength;
        SerializedProperty cellSize;

        SerializedProperty verticalGridsCount; //Pro Feature
        SerializedProperty gridHeight; //Pro Feature
        SerializedProperty changeHeightWithInput; //Pro Feature
        SerializedProperty autoDetectHeight; //Pro Feature
        SerializedProperty autoDetectHeightLayerMask; //Pro Feature
        SerializedProperty gridOriginXZ;
        SerializedProperty gridOriginXY;
        SerializedProperty useHolderPositionAsOrigin;
        SerializedProperty useBuildableDistance;
        SerializedProperty distanceCheckObject;
        SerializedProperty distanceMin;
        SerializedProperty distanceMax;
        SerializedProperty mouseColliderLayerMask;
        SerializedProperty freeObjectCollidingLayerMask; //Pro Feature
        SerializedProperty colliderSizeMultiplier;
        SerializedProperty lockColliderOnHeightChange; //Pro Feature

        SerializedProperty showEditorAndRuntimeCanvasGrid;
        SerializedProperty gridCanvasPrefab;
        SerializedProperty gridImageSprite;
        SerializedProperty showColor;
        SerializedProperty hideColor;
        SerializedProperty colorTransitionSpeed;
        SerializedProperty showOnDefaultMode;
        SerializedProperty showOnBuildMode;
        SerializedProperty showOnDestructMode;
        SerializedProperty showOnSelectedMode;
        //SerializedProperty showOnMoveMode;
        SerializedProperty lockCanvasGridOnHeightChange; //Pro Feature

        SerializedProperty showEditorAndRuntimeDebugGrid;
        SerializedProperty editorGridLineColor;
        SerializedProperty lockDebugGridOnHeightChange; //Pro Feature

        SerializedProperty showRuntimeNodeGrid;
        SerializedProperty gridNodePrefab;
        SerializedProperty gridNodeMarginPercentage;
        SerializedProperty gridNodeLocalOffset;

        SerializedProperty showRuntimeGridText;
        SerializedProperty gridTextColor;
        SerializedProperty gridTextSizeMultiplier;
        SerializedProperty showCellValueText;
        SerializedProperty gridTextPrefix;
        SerializedProperty gridTextSuffix;
        SerializedProperty gridTextLocalOffset;

        SerializedProperty enableSaveAndLoad;
        SerializedProperty uniqueSaveName;
        SerializedProperty saveLocation;

        SerializedProperty showConsoleText;
        SerializedProperty objectPlacement;
        SerializedProperty objectDestruction;
        SerializedProperty objectSelected;
        //SerializedProperty objectMoving;
        SerializedProperty gridLevelChange; //Pro Feature
        SerializedProperty saveAndLoad;

        SerializedProperty enableUnityEvents;
        SerializedProperty OnSelectedBuildableChangedUnityEvent;
        SerializedProperty OnGridCellChangedUnityEvent;
        SerializedProperty OnActiveGridLevelChangedUnityEvent; //Pro Feature
        SerializedProperty OnObjectPlacedUnityEvent;
        SerializedProperty OnObjectRemovedUnityEvent;
        SerializedProperty OnObjectSelectedUnityEvent;
        SerializedProperty OnObjectDeselectedUnityEvent;
        //SerializedProperty OnObjectStartMovingUnityEvent;
        //SerializedProperty OnObjectStoppedMovingUnityEvent;
        
        #endregion

        private void OnEnable()
        {
            buildableGridObjectTypeSOList = serializedObject.FindProperty("buildableGridObjectTypeSOList");
            buildableEdgeObjectTypeSOList = serializedObject.FindProperty("buildableEdgeObjectTypeSOList"); //Pro Feature
            buildableFreeObjectTypeSOList = serializedObject.FindProperty("buildableFreeObjectTypeSOList"); //Pro Feature

            gridAxis = serializedObject.FindProperty("gridAxis");
            gridWidth = serializedObject.FindProperty("gridWidth");
            gridLength = serializedObject.FindProperty("gridLength");
            cellSize = serializedObject.FindProperty("cellSize");
            verticalGridsCount = serializedObject.FindProperty("verticalGridsCount"); //Pro Feature
            gridHeight = serializedObject.FindProperty("gridHeight"); //Pro Feature
            changeHeightWithInput = serializedObject.FindProperty("changeHeightWithInput"); //Pro Feature
            autoDetectHeight = serializedObject.FindProperty("autoDetectHeight"); //Pro Feature
            autoDetectHeightLayerMask = serializedObject.FindProperty("autoDetectHeightLayerMask"); //Pro Feature
            gridOriginXZ = serializedObject.FindProperty("gridOriginXZ");
            gridOriginXY = serializedObject.FindProperty("gridOriginXY");
            useHolderPositionAsOrigin = serializedObject.FindProperty("useHolderPositionAsOrigin");
            useBuildableDistance = serializedObject.FindProperty("useBuildableDistance");
            distanceCheckObject = serializedObject.FindProperty("distanceCheckObject");
            distanceMin = serializedObject.FindProperty("distanceMin");
            distanceMax = serializedObject.FindProperty("distanceMax");
            mouseColliderLayerMask = serializedObject.FindProperty("mouseColliderLayerMask");
            freeObjectCollidingLayerMask = serializedObject.FindProperty("freeObjectCollidingLayerMask"); //Pro Feature
            colliderSizeMultiplier = serializedObject.FindProperty("colliderSizeMultiplier");
            lockColliderOnHeightChange = serializedObject.FindProperty("lockColliderOnHeightChange");  //Pro Feature
            
            showEditorAndRuntimeCanvasGrid = serializedObject.FindProperty("showEditorAndRuntimeCanvasGrid");
            gridCanvasPrefab = serializedObject.FindProperty("gridCanvasPrefab");
            gridImageSprite = serializedObject.FindProperty("gridImageSprite");
            showColor = serializedObject.FindProperty("showColor");
            hideColor = serializedObject.FindProperty("hideColor");
            colorTransitionSpeed = serializedObject.FindProperty("colorTransitionSpeed");
            showOnDefaultMode = serializedObject.FindProperty("showOnDefaultMode");
            showOnBuildMode = serializedObject.FindProperty("showOnBuildMode");
            showOnDestructMode = serializedObject.FindProperty("showOnDestructMode");
            showOnSelectedMode = serializedObject.FindProperty("showOnSelectedMode");
            //showOnMoveMode = serializedObject.FindProperty("showOnMoveMode");
            lockCanvasGridOnHeightChange = serializedObject.FindProperty("lockCanvasGridOnHeightChange");  //Pro Feature

            showEditorAndRuntimeDebugGrid = serializedObject.FindProperty("showEditorAndRuntimeDebugGrid");
            editorGridLineColor = serializedObject.FindProperty("editorGridLineColor");
            lockDebugGridOnHeightChange = serializedObject.FindProperty("lockDebugGridOnHeightChange");  //Pro Feature

            showRuntimeNodeGrid = serializedObject.FindProperty("showRuntimeNodeGrid");
            gridNodePrefab = serializedObject.FindProperty("gridNodePrefab");
            gridNodeMarginPercentage = serializedObject.FindProperty("gridNodeMarginPercentage");
            gridNodeLocalOffset = serializedObject.FindProperty("gridNodeLocalOffset");

            showRuntimeGridText = serializedObject.FindProperty("showRuntimeGridText");
            gridTextColor = serializedObject.FindProperty("gridTextColor");
            gridTextSizeMultiplier = serializedObject.FindProperty("gridTextSizeMultiplier");
            showCellValueText = serializedObject.FindProperty("showCellValueText");
            gridTextPrefix = serializedObject.FindProperty("gridTextPrefix");
            gridTextSuffix = serializedObject.FindProperty("gridTextSuffix");
            gridTextLocalOffset = serializedObject.FindProperty("gridTextLocalOffset");

            enableSaveAndLoad = serializedObject.FindProperty("enableSaveAndLoad");
            uniqueSaveName = serializedObject.FindProperty("uniqueSaveName");
            saveLocation = serializedObject.FindProperty("saveLocation");

            showConsoleText = serializedObject.FindProperty("showConsoleText");
            objectPlacement = serializedObject.FindProperty("objectPlacement");
            objectDestruction = serializedObject.FindProperty("objectDestruction");
            objectSelected = serializedObject.FindProperty("objectSelected");
            //objectMoving = serializedObject.FindProperty("objectMoving");
            gridLevelChange = serializedObject.FindProperty("gridLevelChange"); //Pro Feature
            saveAndLoad = serializedObject.FindProperty("saveAndLoad");

            enableUnityEvents = serializedObject.FindProperty("enableUnityEvents");
            OnSelectedBuildableChangedUnityEvent = serializedObject.FindProperty("OnSelectedBuildableChangedUnityEvent");
            OnGridCellChangedUnityEvent = serializedObject.FindProperty("OnGridCellChangedUnityEvent");
            OnActiveGridLevelChangedUnityEvent = serializedObject.FindProperty("OnActiveGridLevelChangedUnityEvent"); //Pro Feature
            OnObjectPlacedUnityEvent = serializedObject.FindProperty("OnObjectPlacedUnityEvent");
            OnObjectRemovedUnityEvent = serializedObject.FindProperty("OnObjectRemovedUnityEvent");
            OnObjectSelectedUnityEvent = serializedObject.FindProperty("OnObjectSelectedUnityEvent");
            OnObjectDeselectedUnityEvent = serializedObject.FindProperty("OnObjectDeselectedUnityEvent");
            //OnObjectStartMovingUnityEvent = serializedObject.FindProperty("OnObjectStartMovingUnityEvent");
            //OnObjectStoppedMovingUnityEvent = serializedObject.FindProperty("OnObjectStoppedMovingUnityEvent");
            
        }

        public override void OnInspectorGUI()
        {
            EasyGridBuilderPro easyGridBuilderPro = (EasyGridBuilderPro)target;
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
            EditorGUILayout.LabelField("EASY GRID BUILDER PRO", style, GUILayout.ExpandWidth(true));
            //EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            //EditorGUILayout.BeginHorizontal("textArea");
            if (GUILayout.Button("GRID LITE", GUILayout.Height(20))) easyGridBuilderPro.gridEditorMode = GridEditorMode.GridLite;
            if (GUILayout.Button("GRID PRO", GUILayout.Height(20))) easyGridBuilderPro.gridEditorMode = GridEditorMode.GridPro;
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            if (easyGridBuilderPro.gridEditorMode == GridEditorMode.None)
            {
            }
            else
            {
                GUILayout.BeginVertical("GroupBox");
                GUILayout.BeginVertical("textArea", GUILayout.Height(30));
                if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridLite)
                {
                    EditorGUILayout.LabelField("CORE GRID - LITE", style, GUILayout.ExpandWidth(true));
                }
                else if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                {
                    EditorGUILayout.LabelField("CORE GRID - PRO", style, GUILayout.ExpandWidth(true));
                }
                GUILayout.EndVertical();
                
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(buildableGridObjectTypeSOList);
                EditorGUI.indentLevel--;

                if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                {
                    EditorGUI.indentLevel++;
                    if (easyGridBuilderPro.gridAxis == GridAxis.XZ)
                    {
                        EditorGUILayout.PropertyField(buildableEdgeObjectTypeSOList);
                    }
                    EditorGUILayout.PropertyField(buildableFreeObjectTypeSOList);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(gridAxis);
                EditorGUILayout.PropertyField(gridWidth);
                EditorGUILayout.PropertyField(gridLength);
                EditorGUILayout.PropertyField(cellSize);
                if (easyGridBuilderPro.gridAxis == GridAxis.XZ)
                {
                    EditorGUILayout.PropertyField(gridOriginXZ);
                }
                else
                {
                    EditorGUILayout.PropertyField(gridOriginXY);
                }
                EditorGUILayout.PropertyField(useHolderPositionAsOrigin);

                if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                {
                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel++;
                    easyGridBuilderPro.showVerticalGridData = EditorGUILayout.Foldout(easyGridBuilderPro.showVerticalGridData, "Vertical Grid Data", true);

                    if (easyGridBuilderPro.showVerticalGridData)
                    {
                        EditorGUILayout.PropertyField(verticalGridsCount);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(gridHeight);
                        if (GUILayout.Button("Reset", GUILayout.Width(60))) easyGridBuilderPro.SetGridHeight(2.5f);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.PropertyField(changeHeightWithInput);
                        EditorGUILayout.PropertyField(autoDetectHeight);
                        if (easyGridBuilderPro.autoDetectHeight)
                        {
                            EditorGUILayout.PropertyField(autoDetectHeightLayerMask);
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndVertical();
                }

                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showBuildableDistanceData = EditorGUILayout.Foldout(easyGridBuilderPro.showBuildableDistanceData, "Buildable Distance Data", true);
                                    
                if (easyGridBuilderPro.showBuildableDistanceData)
                {
                    EditorGUILayout.PropertyField(useBuildableDistance);
                    if (easyGridBuilderPro.useBuildableDistance)
                    {
                        EditorGUILayout.PropertyField(distanceCheckObject);
                        EditorGUILayout.PropertyField(distanceMin);
                        EditorGUILayout.PropertyField(distanceMax);
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showGridObjectCollisionData = EditorGUILayout.Foldout(easyGridBuilderPro.showGridObjectCollisionData, "Grid Object Collision Data", true);

                if (easyGridBuilderPro.showGridObjectCollisionData)
                {
                    EditorGUILayout.PropertyField(mouseColliderLayerMask);
                    if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                    {
                        EditorGUILayout.PropertyField(freeObjectCollidingLayerMask);
                    }
                    EditorGUILayout.PropertyField(colliderSizeMultiplier);
                    if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                    {
                        EditorGUILayout.PropertyField(lockColliderOnHeightChange);
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                GUILayout.BeginVertical("textArea", GUILayout.Height(30));
                EditorGUILayout.LabelField("VISUALS", style, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showCanvasGridData = EditorGUILayout.Foldout(easyGridBuilderPro.showCanvasGridData, "Canvas Grid Data", true);

                if (easyGridBuilderPro.showCanvasGridData)
                {
                    EditorGUILayout.PropertyField(showEditorAndRuntimeCanvasGrid);
                    if (easyGridBuilderPro.showEditorAndRuntimeCanvasGrid)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(gridCanvasPrefab);
                        if (GUILayout.Button("Auto Set", GUILayout.Width(60)))
                        {
                            easyGridBuilderPro.gridCanvasPrefab = (Canvas)AssetDatabase.LoadAssetAtPath("Assets/EasyGridBuilder Pro/Prefabs/Canvas Grid Samples/Grid Canvas.prefab", typeof(Canvas));
                        } 
                        EditorGUILayout.EndHorizontal();
                        
                        EditorGUILayout.PropertyField(gridImageSprite);
                        EditorGUILayout.PropertyField(showColor);
                        EditorGUILayout.PropertyField(hideColor);
                        EditorGUILayout.PropertyField(colorTransitionSpeed);
                        EditorGUILayout.PropertyField(showOnDefaultMode);
                        EditorGUILayout.PropertyField(showOnBuildMode);
                        EditorGUILayout.PropertyField(showOnDestructMode);
                        EditorGUILayout.PropertyField(showOnSelectedMode);
                        //EditorGUILayout.PropertyField(showOnMoveMode);
                        if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                        {
                            EditorGUILayout.PropertyField(lockCanvasGridOnHeightChange);
                        }
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showDebugGridData = EditorGUILayout.Foldout(easyGridBuilderPro.showDebugGridData, "Dubug Grid Data", true);

                if (easyGridBuilderPro.showDebugGridData)
                {
                    EditorGUILayout.PropertyField(showEditorAndRuntimeDebugGrid);
                    if (easyGridBuilderPro.showEditorAndRuntimeDebugGrid)
                    {
                        EditorGUILayout.PropertyField(editorGridLineColor);
                        if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                        {
                            EditorGUILayout.PropertyField(lockDebugGridOnHeightChange);
                        }
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                
                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showNodeGridData = EditorGUILayout.Foldout(easyGridBuilderPro.showNodeGridData, "Node Grid Data", true);

                if (easyGridBuilderPro.showNodeGridData)
                {
                    EditorGUILayout.PropertyField(showRuntimeNodeGrid);
                    if (easyGridBuilderPro.showRuntimeNodeGrid)
                    {
                        EditorGUILayout.PropertyField(gridNodePrefab);
                        EditorGUILayout.PropertyField(gridNodeMarginPercentage);
                        EditorGUILayout.PropertyField(gridNodeLocalOffset);
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                
                GUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                easyGridBuilderPro.showTextGridData = EditorGUILayout.Foldout(easyGridBuilderPro.showTextGridData, "Text Grid Data", true);

                if (easyGridBuilderPro.showTextGridData)
                {
                    EditorGUILayout.PropertyField(showRuntimeGridText);
                    if (easyGridBuilderPro.showRuntimeGridText)
                    {
                        EditorGUILayout.PropertyField(gridTextColor);
                        EditorGUILayout.PropertyField(gridTextSizeMultiplier);
                        EditorGUILayout.PropertyField(showCellValueText);
                        EditorGUILayout.PropertyField(gridTextPrefix);
                        EditorGUILayout.PropertyField(gridTextSuffix);
                        EditorGUILayout.PropertyField(gridTextLocalOffset);
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();

                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                GUILayout.BeginVertical("textArea", GUILayout.Height(30));
                EditorGUILayout.LabelField("SAVE & LOAD", style, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                EditorGUILayout.PropertyField(enableSaveAndLoad);
                if (easyGridBuilderPro.enableSaveAndLoad)
                {
                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel++;
                    easyGridBuilderPro.showSaveAndLoadData = EditorGUILayout.Foldout(easyGridBuilderPro.showSaveAndLoadData, "Save & Load Data", true);

                    if (easyGridBuilderPro.showSaveAndLoadData)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(uniqueSaveName);
                        if (GUILayout.Button("Reset", GUILayout.Width(60))) easyGridBuilderPro.uniqueSaveName = "EasyGridBuilder_SaveData_";
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(saveLocation);
                        if (GUILayout.Button("Reset", GUILayout.Width(60))) easyGridBuilderPro.saveLocation = "/EasyGridBuilder Pro/LocalSaves/";
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                GUILayout.BeginVertical("textArea", GUILayout.Height(30));
                EditorGUILayout.LabelField("DEBUG", style, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                EditorGUILayout.PropertyField(showConsoleText);
                if (easyGridBuilderPro.showConsoleText)
                {
                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel++;
                    easyGridBuilderPro.showConsoleData = EditorGUILayout.Foldout(easyGridBuilderPro.showConsoleData, "Console Data", true);

                    if (easyGridBuilderPro.showConsoleData)
                    {
                        EditorGUILayout.PropertyField(objectPlacement);
                        EditorGUILayout.PropertyField(objectDestruction);
                        EditorGUILayout.PropertyField(objectSelected);
                        //EditorGUILayout.PropertyField(objectMoving);
                        if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                        {
                            EditorGUILayout.PropertyField(gridLevelChange);
                        }
                        EditorGUILayout.PropertyField(saveAndLoad);
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndVertical();
                GUILayout.BeginVertical("GroupBox");
                GUILayout.BeginVertical("textArea", GUILayout.Height(30));
                EditorGUILayout.LabelField("UNITY EVENTS", style, GUILayout.ExpandWidth(true));
                GUILayout.EndVertical();

                EditorGUILayout.PropertyField(enableUnityEvents);
                if (easyGridBuilderPro.enableUnityEvents)
                {
                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel++;
                    easyGridBuilderPro.showBaseEvent = EditorGUILayout.Foldout(easyGridBuilderPro.showBaseEvent, "Base Events", true);

                    if (easyGridBuilderPro.showBaseEvent)
                    {
                        EditorGUILayout.PropertyField(OnSelectedBuildableChangedUnityEvent);
                        EditorGUILayout.PropertyField(OnGridCellChangedUnityEvent);
                        if (easyGridBuilderPro.gridEditorMode == GridEditorMode.GridPro) //Pro Feature
                        {
                            EditorGUILayout.PropertyField(OnActiveGridLevelChangedUnityEvent);
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");
                    EditorGUI.indentLevel++;
                    easyGridBuilderPro.showObjectInteractEvents = EditorGUILayout.Foldout(easyGridBuilderPro.showObjectInteractEvents, "Object Interact Events", true);

                    if (easyGridBuilderPro.showObjectInteractEvents)
                    {
                        EditorGUILayout.PropertyField(OnObjectPlacedUnityEvent);
                        EditorGUILayout.PropertyField(OnObjectRemovedUnityEvent);  
                        EditorGUILayout.PropertyField(OnObjectSelectedUnityEvent);
                        EditorGUILayout.PropertyField(OnObjectDeselectedUnityEvent);
                        //EditorGUILayout.PropertyField(OnObjectStartMovingUnityEvent);
                        //EditorGUILayout.PropertyField(OnObjectStoppedMovingUnityEvent);
                    }
                    
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}