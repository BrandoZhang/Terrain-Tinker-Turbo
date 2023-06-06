/// <summary>
/// Fatty Ground Utility (Alpha0.0.1 / 2020.4.2, id3644@gmail.com)
/// !!Caution!!
///- The slot value save function is not yet supported.
///- Multiple selection is not yet supported.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class FattyGroundTool : EditorWindow
{
    //private float snap = 50;
    public float snap = 54;
    public float snapHeight = 18;
    public float snapDegrees = 90;
    public float groundHeight = 0;  // Replaced by snapHeight, Used for CheckPosition(), but CheckPosition() has also been replaced.
        Transform objPos;
    Vector3 lastPrefabPos;

    //Path
    public DefaultAsset targetFolder = null;
    public string targetFolderPath = "";
    //Ground list
    public int groundSample;
    public List<Object> groundSamples = new List<Object>();
    public List<Texture2D> groundSamplesTex = new List<Texture2D>();
    public string autoFillInfoTex = "";
    public string removeNameFilter = "Remove String Here.";

    //Selection
    GameObject selectedObj;
    string selectedObjString = "Select an object to move it.";
    //public bool projectObj = false;

    //UI
    Vector2 scrollPos = Vector2.zero;
    Vector2 scrollThumPos = Vector2.zero;
    //Vector2 scrollThumPosA = Vector2.zero;
    int row = 2;
    int col;

    public bool groundObjFold = true;
    bool autoFillFold = true;
    bool autoFillInfoFold = true;
    bool adescriptionFold = true;
    string description = " - If you select a GameObject in Hierarchy, the selection status window will turn green. \n" +
        " - Move the prefab with the direction pad. \n" +
        " - Rotate the prefab with the rotating pad. \n" +
        " - The slot value save function is not yet supported. \n" +
        " - Multiple selection is not yet supported. \n" +
        "[ !!CAUTION!! ] \n" +
        " - The snap value is reset when the window is closed. \n" +
        "[ 2020/6/3 ] Changes. \n" +
        " - Added lift function. (Direction pad) \n" +
        " - Added thumbnail name filter function. \n" +
        " - Create near the screen or on the final track (near the selected track) (Add Track Prefab button). \n" +
        " - Fixed snap error. \n" +
        "";

    //test
    
    public Vector3 copyPos;
    public Quaternion copyRot;
    public Vector3 copyScale;
    public bool copyOption = false;

    public bool toggleSetActive = false;


    [MenuItem("Window/FattyGroundTool")]

    static void Init()
    {
        FattyGroundTool groundUtility = (FattyGroundTool)EditorWindow.GetWindow(typeof(FattyGroundTool));
        groundUtility.Show();
    }

    private void TestFunction()
    {
        Debug.Log("Hi~ i am TestFunction()");
    }
    
    bool IntOrFloat(float _Mum)
    {
        if (_Mum - (int)_Mum == 0)
        {
            return  true;
        }
        else
        {
            return false;
        }
    }

    static bool PrefabFilter()
    {
        GameObject _go = Selection.activeObject as GameObject;
        if (_go == null)
            return false;

        return PrefabUtility.GetPrefabType(_go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(_go) == PrefabType.ModelPrefab;
    }

    public void CopyTransform()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go != null)
        {
            copyPos = go.transform.position;
            copyRot = go.transform.rotation;
            copyScale = go.transform.localScale;
            if (copyOption && go.activeSelf)
            {
                go.SetActive(false);
            }
        }

    }

    public void PasteTransform()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go != null)
        {
            go.transform.position = copyPos;
            go.transform.rotation = copyRot;
            go.transform.localScale = copyScale;
        }

    }

    public void RemoveSelected()
    {
        /* Undo is not supported.
        if (Selection.activeGameObject && Selection.activeGameObject.activeInHierarchy)
        {
            GameObject selectedObject = Selection.activeGameObject;
            Debug.Log("Delete gameobject = " + selectedObject.name);
            DestroyImmediate(selectedObject);
        }
        */
        SceneView.lastActiveSceneView.Focus();
        EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Delete"));
    }

    public static void DuplicateSelected()
    {
        /*
        Object prefabRoot = PrefabUtility.GetPrefabParent(Selection.activeGameObject);
        if (prefabRoot != null)
        {
            PrefabUtility.InstantiatePrefab(prefabRoot);
            Debug.Log("prefabRoot");
        }
        else
        {
            Instantiate(Selection.activeGameObject);
            Debug.Log("not prefabRoot");
        }
        */
        SceneView.lastActiveSceneView.Focus();
        EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
    }

    void CreatePrefab(Object _go)
    {
        GameObject clone = PrefabUtility.InstantiatePrefab(_go) as GameObject;

        Vector3 CreatePrefabPos;
        float lastPrefabDist = Vector3.Distance(lastPrefabPos, SceneView.lastActiveSceneView.pivot);

        if (Selection.activeGameObject != null)
        {
            CreatePrefabPos = Selection.activeGameObject.transform.position + Vector3.forward * snap;
            Debug.Log("<color=green>Selection.activeGameObject = </color>" + Selection.activeGameObject);
        }
        else if (lastPrefabDist < 1000)
        {
            CreatePrefabPos = lastPrefabPos + Vector3.forward * snap;
            lastPrefabPos = CreatePrefabPos;
            Debug.Log("<color=blue>Selection.activeGameObject = </color>" + Selection.activeGameObject);
        }
        else
        {
            CreatePrefabPos = SceneView.lastActiveSceneView.pivot;
            CreatePrefabPos = new Vector3(Mathf.Round(CreatePrefabPos.x / snap) * snap, Mathf.Round(CreatePrefabPos.y / snapHeight) * snapHeight, Mathf.Round(CreatePrefabPos.z / snap) * snap);
        }
        
        clone.transform.position = CreatePrefabPos;
        Selection.activeGameObject = clone;
    }

    static void AddPrefab(Object _go)
    {
        // not used - This has been replaced by "CreatePrefab".
        Object prefabRoot = PrefabUtility.GetPrefabParent(_go);
        if (prefabRoot != null)
        {
            GameObject addedPrefab;
            addedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
            addedPrefab.transform.position = SceneView.lastActiveSceneView.pivot;
            Debug.Log("prefabRoot" + SceneView.lastActiveSceneView.pivot);
        }
        else
        {
            Debug.Log("Select object is not prefabRoot");
        }
    }

    void AutoFillInfo()
    {

    }

    void AutoFillPrefabs()  
    {
        //get path string
        string getFolderPath = "";
        if (targetFolder != null)
        {
            getFolderPath = AssetDatabase.GetAssetPath(targetFolder);
            Debug.Log("getPath = " + getFolderPath);
        }

        //get object in path
        var assets = AssetDatabase.FindAssets("t:prefab", new[] { getFolderPath });

        //ongui list reset
        groundSample = assets.Length;
        groundSamples = new List<Object>(new GameObject[groundSample]);
        groundSamplesTex = new List<Texture2D>(new Texture2D[groundSample]);

        //put object into list
        int count = assets.Length + 1;   //reverse loop count
        //Debug.Log("groundSamples.Count" + groundSamples.Count + " /getPaths.Length " + getPaths.Length + " /groundSample " + groundSample);
        foreach (var guid in assets)
        {
            count --;
            //var myAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
            //Debug.Log("<color=red>myAsset</color>" + myAsset + " count = " + count);
            groundSamples[groundSamples.Count - count] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
        }
    }

    void OnGUI()
    {
        //GUILayout.Label("Ground Utility (alpha 0.0.1)", EditorStyles.boldLabel);   //title
        EditorGUILayout.BeginVertical("Button");
        adescriptionFold = EditorGUILayout.Foldout(adescriptionFold, "Ground Utility (alpha 0.0.1)");
        if (adescriptionFold)
        {
            GUILayout.TextArea(description);
        }
        EditorGUILayout.EndVertical();
        //EditorGUILayout.Space();

        
        EditorGUILayout.BeginVertical("Box");   //----------------------------------------------v00 ┐
        //Object Selection State
        if (selectedObj != null && !PrefabFilter())
            GUI.color = Color.green;
        else
            GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal("Box");     //h01 ┐
        GUILayout.Label("[ Status ] Selected Obj : ");
        GUILayout.Label("" + selectedObjString);
        EditorGUILayout.EndHorizontal();            //h01 ┘

        GUI.color = Color.white;
        
        EditorGUILayout.BeginHorizontal();  //----------------------------------h02 ┐
        EditorGUI.BeginDisabledGroup(selectedObj == null & !PrefabFilter());
        if (selectedObj != null)
        {
            toggleSetActive = EditorGUILayout.Toggle("Selected Obj Set Active", selectedObj.activeSelf);

            if (toggleSetActive)
            {
                selectedObj.SetActive(true);
            }
            else
            {
                selectedObj.SetActive(false);
            }
        }
        else
        {
            toggleSetActive = EditorGUILayout.Toggle("Selected Obj Set Active", false);
        }
        EditorGUILayout.BeginVertical("Box");   //-------------------v03 ┐
        EditorGUILayout.BeginHorizontal();  //h04 ┐
        if (GUILayout.Button("Copy Transform", GUILayout.Height(30)))
            CopyTransform();
        if (GUILayout.Button("Paste Transform", GUILayout.Height(30)))
            PasteTransform();
        EditorGUILayout.EndHorizontal();    //h04 ┘
        copyOption = EditorGUILayout.Toggle("Hide Copied Objects", copyOption);
        EditorGUILayout.EndVertical();  //---------------------------v03 ┘
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();    //----------------------------------h02 ┘
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();  //----------------------------------------------v00 ┘
        
        //Extra function
        if (GUILayout.Button("Duplicate", GUILayout.Height(30)))
            DuplicateSelected();
        GUI.color = Color.red;
        if (GUILayout.Button("Delete", GUILayout.Height(30)))
            RemoveSelected();
        GUI.color = Color.white;

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false); //Utility window scroll start
        
        //thin line
        var line = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(line.x - 15, line.y), new Vector2(line.width + 15, line.y));
        EditorGUILayout.EndHorizontal();

        //EditorGUILayout.Space();
        
        //Arrow Key====================================================================================
        #region Arrow Keypad
        EditorGUILayout.BeginHorizontal();  //Keypad frame start
        EditorGUILayout.BeginVertical("box", GUILayout.Width(145));    //Left Keypad start(arrow key)
        EditorGUILayout.BeginHorizontal();  //arrow key row start
        //col1(1,4,7,180)
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("┌", GUILayout.Height(30)))
            MoveTo(7);
        if (GUILayout.Button("-x", GUILayout.Height(30)))
            MoveTo(6);
        if (GUILayout.Button("└", GUILayout.Height(30)))
            MoveTo(5);
        GUILayout.Space(10);
        if (GUILayout.Button("180.", GUILayout.Height(30)))
            TurnObj(2);
        EditorGUILayout.EndVertical();

        //col2(2,5,8,cw)
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("+z", GUILayout.Height(30)))
            MoveTo(0);
        if (GUILayout.Button("+", GUILayout.Height(30)))
            MoveTo(8);
        if (GUILayout.Button("-z", GUILayout.Height(30)))
            MoveTo(4);
        GUILayout.Space(10);
        if (GUILayout.Button("CW.", GUILayout.Height(30)))
            TurnObj(1);
        EditorGUILayout.EndVertical();

        //col3(3,6,9,ccw)
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("┐", GUILayout.Height(30)))
            MoveTo(1);
        if (GUILayout.Button("+x", GUILayout.Height(30)))
            MoveTo(2);
        if (GUILayout.Button("┘", GUILayout.Height(30)))
            MoveTo(3);
        GUILayout.Space(10);
        if (GUILayout.Button("CCW", GUILayout.Height(30)))
            TurnObj(0);
        EditorGUILayout.EndVertical();



        //col4(up, down)
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Up", GUILayout.Height(60)))
            MoveTo(9);
        if (GUILayout.Button("Dn", GUILayout.Height(60)))
            MoveTo(10);
        EditorGUILayout.EndVertical();




        EditorGUILayout.EndHorizontal();    //arrow key row end
        EditorGUILayout.EndVertical();  //Left Keypad end(arrow key)

        EditorGUILayout.BeginVertical("box", GUILayout.Height(145));    //Right start
        //Snap Value====================================================================================
        EditorGUILayout.BeginVertical();
        //groundHeight = EditorGUILayout.FloatField("Ground Height(Y):", groundHeight);
        snap = EditorGUILayout.FloatField("Snap Size(XZ):", snap);
        snapHeight = EditorGUILayout.FloatField("Snap Height(Y):", snapHeight);
        snapDegrees = EditorGUILayout.FloatField("Snap Degrees(Y):", snapDegrees);
        EditorGUILayout.EndVertical();
        //Snap Value end
        
        //GUILayout.TextArea(description);

        EditorGUILayout.EndVertical();  //Right end

        EditorGUILayout.EndHorizontal();    //Keypad frame end
        #endregion

        EditorGUILayout.Space();    //SpaceSpaceSpaceSpaceSpaceSpaceSpaceSpaceSpaceSpace

        #region Test Extra function
        
        
        

        EditorGUILayout.Space();
        #endregion

        #region Palette Settings
        //path
        EditorGUILayout.BeginVertical("Box");   //path frame
        
        autoFillFold = EditorGUILayout.Foldout(autoFillFold, "Ground Palette Settings");
        
        if (autoFillFold)
        {
            
            EditorGUILayout.BeginHorizontal("button");
            
            if (targetFolder != null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Auto Fill", GUILayout.Width(120)))
                {
                    AutoFillPrefabs();
                }
                GUI.color = Color.white;
            }
            else
            {
                GUILayout.Label("Please Select Folder!!", GUILayout.Width(120));
            }
            
            targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(targetFolder, typeof(DefaultAsset), false);
            EditorGUILayout.EndHorizontal();
            
            autoFillInfoFold = EditorGUILayout.Foldout(autoFillInfoFold, "Info. (Folder contents)");    //TODO:job done info string and list info clear.
            if (autoFillInfoFold)
            {
                if (targetFolder != null) 
                {
                    //EditorGUILayout.HelpBox("Valid folder! Name: " + targetFolder.name, MessageType.Info, true);
                    string prefabListTex = "";
                    string folderPath = AssetDatabase.GetAssetPath(targetFolder);
                    string[] getprefabLists = AssetDatabase.FindAssets("t:prefab", new[] { folderPath });
                    foreach (string getprefabList in getprefabLists)
                    {
                        string getprefabListCut = AssetDatabase.GUIDToAssetPath(getprefabList).Replace(folderPath, "");
                        prefabListTex = prefabListTex + "\n " + getprefabListCut;
                    }

                    EditorGUILayout.HelpBox("Prefab List(" + getprefabLists.Length + ") : " + prefabListTex, MessageType.Info, true);
                }
                else
                {
                    EditorGUILayout.HelpBox("Not valid!", MessageType.Warning, true);
                }
            }
            
            EditorGUI.BeginChangeCheck();
            groundSample = EditorGUILayout.IntField("Ground Size:", groundSample);
            if (EditorGUI.EndChangeCheck())
            {
                // Do something when the property changes 
                //some bug : List update ... slot links are broken.
                groundSamples = new List<Object>(new GameObject[groundSample]);
                groundSamplesTex = new List<Texture2D>(new Texture2D[groundSample]);
                Repaint();
            }



            groundObjFold = EditorGUILayout.Foldout(groundObjFold, "Ground Content");
            if (groundObjFold)
            {
                for (int i = 0; i < groundSample; i++)  //UI - ground slot
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("object : ");
                    groundSamples[i] = EditorGUILayout.ObjectField(groundSamples[i], typeof(GameObject), true) as GameObject;
                    //GUILayout.Label("Tex : ");
                    //groundSamplesTex[i] = EditorGUILayout.ObjectField(groundSamplesTex[i], typeof(Texture2D), false) as Texture2D;
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
        #endregion

        #region Thumbnail
        GUILayout.BeginVertical("box");

        
        //button setting.
        GUILayout.BeginHorizontal();
        GUILayout.Label("Thumbnail");
        row = EditorGUILayout.IntField("Button Row : ", row);
        GUILayout.EndHorizontal();
        removeNameFilter = EditorGUILayout.TextField("Remove String Filter: ", removeNameFilter);

        scrollThumPos = GUILayout.BeginScrollView(scrollThumPos, false, false, GUILayout.Height(350));  //Thumbnail scroll start H130
        
        EditorGUILayout.BeginHorizontal();  //Thumbnail frame start
        int rowCount = 0;
        if (groundSample <= 0)
        {
            GUI.enabled = false;
            Rect rr = EditorGUILayout.BeginVertical("Button");
            if (GUI.Button(rr, GUIContent.none))
            {
                //Debug.Log("CreatePrefab(groundSamples[i])");
                //CreatePrefab(groundSamples[i]);
            }
            GUILayout.Label("x", GUILayout.Width(80), GUILayout.Height(80));
            GUILayout.Label("Register your ground using the ground palette settings");

            EditorGUILayout.EndVertical();
            GUI.enabled = true;
        }
        else
        {
            
            for (int i = 0; i < groundSample; i++)
            {
                //GUILayout.Button("-");
                
                if (groundSamples[i] != null)
                {
                    //get thumbnail img
                    groundSamplesTex[i] = AssetPreview.GetAssetPreview(groundSamples[i]);

                    //add button with img
                    Rect rr = EditorGUILayout.BeginVertical("Button");
                    if (GUI.Button(rr, GUIContent.none))
                    {
                        CreatePrefab(groundSamples[i]);
                    }
                    GUILayout.Label(groundSamplesTex[i], GUILayout.Width(80), GUILayout.Height(80));

                    //remove string
                    string nameCut = "";
                    if (removeNameFilter == "Remove String Here." | removeNameFilter.Length == 0)
                    {
                        nameCut = groundSamples[i].name;
                    }
                    else
                    {
                        nameCut = groundSamples[i].name.Replace(removeNameFilter, "");
                    }
                    
                    GUILayout.Label(i + "." + nameCut, GUILayout.Width(120));

                    EditorGUILayout.EndVertical();
                }
                else
                {
                    GUI.enabled = false;

                    Rect rr = EditorGUILayout.BeginVertical("Button");
                    if (GUI.Button(rr, GUIContent.none))
                    {
                        Debug.Log("CreatePrefab(groundSamples[i])");
                        //CreatePrefab(groundSamples[i]);
                        //AddPrefab(groundSamples[i]);
                    }
                    GUILayout.Label("x", GUILayout.Width(80), GUILayout.Height(80));
                    GUILayout.Label(i + ". Empty");

                    EditorGUILayout.EndVertical();

                    GUI.enabled = true;
                }
                
                //table by row
                rowCount++;
                float rowMatch = rowCount / (float)row;
                if (IntOrFloat(rowMatch))
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }
        }

        EditorGUILayout.EndHorizontal();    //Thumbnail frame end
        GUILayout.EndScrollView();  //Thumbnail scroll end
        GUILayout.EndVertical();
        #endregion
        
        GUILayout.EndScrollView();  //Utility window scroll end
        
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("- End of Tool (Fatty Ground Utility) -");
        EditorGUILayout.EndVertical();
    }
    
    void MoveTo(int dir)
    {
        //Foward0, Fowardright1, Right2, BackRight3, Back4, BackLeft5, Left6, FowardLight7
        //Snap8, LiftUp9, LiftDown10
        Vector3 nwePos;
        if (Selection.activeGameObject && !PrefabFilter())
        {
            selectedObj = Selection.activeGameObject;
            objPos = selectedObj.transform;
        }
        else
        {
            Debug.Log("Please select the object first.");
            return;
        }
        
        switch (dir)
        {
            case 0:
                nwePos = objPos.transform.position + new Vector3(0, 0, snap);
                selectedObj.transform.position = nwePos;
                selectedObj = null;
                break;
            case 1:
                nwePos = objPos.transform.position + new Vector3(snap, 0, snap);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 2:
                //right
                nwePos = objPos.transform.position + new Vector3(snap, 0, 0);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 3:
                //back
                nwePos = objPos.transform.position + new Vector3(snap, 0, -snap);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 4:
                nwePos = objPos.transform.position + new Vector3(0, 0, -snap);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 5:
                nwePos = objPos.transform.position + new Vector3(-snap, 0, -snap);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 6:
                //left
                nwePos = objPos.transform.position + new Vector3(-snap, 0, 0);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 7:
                nwePos = objPos.transform.position + new Vector3(-snap, 0, snap);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 8:
                SnapToGrid(objPos);
                //selectedObj = null;
                break;
            case 9:
                //Up
                nwePos = objPos.transform.position + new Vector3(0, snapHeight, 0);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            case 10:
                //Down
                nwePos = objPos.transform.position + new Vector3(0, -snapHeight, 0);
                selectedObj.transform.position = nwePos;
                //selectedObj = null;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
        
    }

    void TurnObj(int dir)
    {
        if (Selection.activeGameObject)
        {
            selectedObj = Selection.activeGameObject;
            objPos = selectedObj.transform;
        }
        else
        {
            Debug.Log("Please select the object first.");
            return;
        }

        if (dir == 0)
        {
            objPos.transform.Rotate(Vector3.down * snapDegrees);
        }
        else if (dir == 1)
        {
            objPos.transform.Rotate(Vector3.up * snapDegrees);
        }
        else if (dir == 2)
        {
            objPos.transform.rotation = Quaternion.LookRotation(-objPos.transform.forward, Vector3.up);
        }
    }
    
    void CheckPosition(Transform _obj)
    {
        // not used - This has been replaced by "SnapToGrid".
        Transform objPos = _obj.transform;
        float xyzSum = objPos.position.x + objPos.position.y + objPos.position.z;
        float dividedValue = xyzSum / snap;

        bool trueInt;
        if (dividedValue - (int)dividedValue == 0)
        {
            trueInt = true;
        }
        else
        {
            trueInt = false;
        }
        

        if (!trueInt)
        {
            var currentPos = objPos.position;
            objPos.position = new Vector3(Mathf.Round(currentPos.x / snap) * snap, groundHeight, Mathf.Round(currentPos.z / snap) * snap);
        }
        else
        {
            Debug.Log("The object is on the snap.");
        }
    }

    void SnapToGrid(Transform _obj)
    {
        //var currentPos = objPos.position;
        //objPos.position = new Vector3(Mathf.Round(currentPos.x / snap) * snap, groundHeight, Mathf.Round(currentPos.z / snap) * snap);

        Vector3 currentPos = _obj.position;
        //_obj.position = new Vector3(Mathf.Round(currentPos.x / snap) * snap, groundHeight, Mathf.Round(currentPos.z / snap) * snap);
        _obj.position = new Vector3(Mathf.Round(currentPos.x / snap) * snap, Mathf.Round(currentPos.y / snapHeight) * snapHeight, Mathf.Round(currentPos.z / snap) * snap);
    }

    void OnSelectionChange()
    {
        if (Selection.activeGameObject)
        {
            selectedObj = Selection.activeGameObject as GameObject;
            selectedObjString = selectedObj.name;
            lastPrefabPos = selectedObj.transform.position;
            Repaint();
        }

        if (!Selection.activeGameObject)
        {
            selectedObj = null;
            selectedObjString = "You have to select game object!";
            Repaint();
        }

        /*
        GameObject go = Selection.activeObject as GameObject;
        
        if (go == null)
            projectObj = false;
        else
        projectObj = PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab;
        */
        }

        static public void MoveSceneViewCamera(Vector3 _position)
    {
        Debug.Log("_position" + _position);
        //Vector3 position = SceneView.lastActiveSceneView.pivot;
        //position.z -= 10.0f;
        SceneView.lastActiveSceneView.pivot = _position;
        SceneView.lastActiveSceneView.Repaint();
        Debug.Log("<color=blue>_position</color>" + SceneView.lastActiveSceneView.pivot);
    }
}
