using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace SoulGames.EasyGridBuilderPro
{
    public class MultiGridUIManager : MonoBehaviour
    {
        private List<EasyGridBuilderPro> easyGridBuilderProList;
        private GridObjectSelector gridObjectSelector;
        private EasyGridBuilderProInputsSO gridBuilderProInputsSO;
        private EasyGridBuilderPro currentActiveSystem;
        private bool animationOpenTrigger = false;
        private bool animationCloseTrigger = true;

        [Space]
        [Tooltip("Add 'BuildableObjectTypeCategorySO' assets. Used to display buildable object categories in UI.")]
        [SerializeField]private List<BuildableObjectTypeCategorySO> buildableObjectTypeCategorySO;

        [SerializeField]public bool showBuildableListMenuData = false;

        [Space]
        [Tooltip("Displays build list menu in grid mode default")]
        [SerializeField]private bool showInGridModeDefault;
        [Tooltip("Displays build list menu in grid mode build")]
        [SerializeField]private bool showInGridModeBuild;
        [Tooltip("Displays build list menu in grid mode destruction")]
        [SerializeField]private bool showInGridModeDestruction;
        [Tooltip("Displays build list menu in grid mode selection")]
        [SerializeField]private bool showInGridModeSelection;

        private List<BuildableGridObjectTypeSO> buildableGridObjectTypeSOList;
        private List<BuildableEdgeObjectTypeSO> buildableEdgeObjectTypeSOList;
        private List<BuildableFreeObjectTypeSO> buildableFreeObjectTypeSOList;

        [SerializeField]public bool showHelpMenuData = false;

        [Space]
        [SerializeField]private GameObject inputGroupObject;
        [SerializeField]private TextMeshProUGUI gridModeResetText;
        [SerializeField]private TextMeshProUGUI gridHeightChangeText;
        [SerializeField]private TextMeshProUGUI buildModeActiveText;
        [SerializeField]private TextMeshProUGUI placementText;
        [SerializeField]private TextMeshProUGUI listScrollText;
        [SerializeField]private TextMeshProUGUI ghostRotateLText;
        [SerializeField]private TextMeshProUGUI ghostRotateRText;
        [SerializeField]private TextMeshProUGUI destructionModeActiveText;
        [SerializeField]private TextMeshProUGUI destroyText;
        [SerializeField]private TextMeshProUGUI selectionModeActiveText;
        [SerializeField]private TextMeshProUGUI selectionText;
        [SerializeField]private TextMeshProUGUI saveText;
        [SerializeField]private TextMeshProUGUI loadText;

        [SerializeField]public bool showBuildablesMenuData = false;
        
        [Space]
        [SerializeField]private GameObject categorySection;
        [SerializeField]private GameObject buildablesSection;
        [SerializeField]private GameObject placeHolderCategory;
        [SerializeField]private GameObject placeHolderBuildable;
        [SerializeField]private GameObject placeHolderBuildableSectionCategory;
        [SerializeField]private Animator buildableListAnimator;

        [SerializeField]public bool showVerticalGridMenuData = false;

        [Space]
        [SerializeField]private GameObject gridLevelUpButton;
        [SerializeField]private GameObject gridLevelDownButton;

        private List<GameObject> instantiatedCategoryObjectsList = new List<GameObject>();
        private List<GameObject> instantiatedBuildableObjectsList = new List<GameObject>();
        private List<GameObject> instantiatedBuildableSectionCategoryList = new List<GameObject>();

        //Events For Created Buttons-----------------------------------------------------------
        private event OnCategoryButtonPressedDelegate OnCategoryButtonPressed;
        private delegate void OnCategoryButtonPressedDelegate(String buttonName);
        private event OnBuildablesButtonPressedDelegate OnBuildablesButtonPressed;
        private delegate void OnBuildablesButtonPressedDelegate(String buttonName);
        //-------------------------------------------------------------------------------------

        private void Start()
        {
            easyGridBuilderProList = MultiGridManager.Instance.easyGridBuilderProList;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            if (FindObjectOfType<GridObjectSelector>()) gridObjectSelector = FindObjectOfType<GridObjectSelector>();
            
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetInputGridModeVariables(true, true, true);
                easyGridBuilderPro.OnBuildableGridObjectTypeSOListChange += OnBuildableGridObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnBuildableEdgeObjectTypeSOListChange += OnBuildableEdgeObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnBuildableFreeObjectTypeSOListChange += OnBuildableFreeObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnGridModeChange += OnGridModeChangeMethod;
            }
            if (gridObjectSelector) gridObjectSelector.SetInputGridModeVariables(true, true, true);
            gridBuilderProInputsSO = MultiGridInputManager.Instance.GetEasyGridBuilderProInputsSO();

            MultiGridManager.Instance.OnActiveGridChanged += OnActiveGridChangedMethod;
            OnCategoryButtonPressed += OnCategoryButtonPressedMethod;
            OnBuildablesButtonPressed += OnBuildablesButtonPressedMethod;

            buildableGridObjectTypeSOList = new List<BuildableGridObjectTypeSO>();
            buildableEdgeObjectTypeSOList = new List<BuildableEdgeObjectTypeSO>();
            buildableFreeObjectTypeSOList = new List<BuildableFreeObjectTypeSO>();

            HandleCategorySection();

            if (instantiatedBuildableSectionCategoryList[0])
            {
                if (instantiatedBuildableSectionCategoryList[0].activeSelf == false) instantiatedBuildableSectionCategoryList[0].SetActive(true);
            }

            if (showInGridModeDefault)
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None)
                {
                    if (!animationOpenTrigger)
                    {
                        buildableListAnimator.SetTrigger("Open");
                        animationOpenTrigger = true;
                        animationCloseTrigger = false;
                    }
                }
            }
            else
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None)
                {
                    if (!animationCloseTrigger)
                    {
                        buildableListAnimator.SetTrigger("Close");
                        animationOpenTrigger = false;
                        animationCloseTrigger = true;
                    }
                }
            }

            HandleBuildablesListSection();
            
            if (currentActiveSystem.gridEditorMode == GridEditorMode.GridLite)
            {
                if (gridLevelUpButton.activeSelf == true) gridLevelUpButton.SetActive(false);
                if (gridLevelDownButton.activeSelf == true) gridLevelDownButton.SetActive(false);
            }
            else if (currentActiveSystem.gridEditorMode == GridEditorMode.GridPro)
            {
                if (gridLevelUpButton.activeSelf == false) gridLevelUpButton.SetActive(true);
                if (gridLevelDownButton.activeSelf == false) gridLevelDownButton.SetActive(true);
            }
            else
            {
                if (gridLevelUpButton.activeSelf == true) gridLevelUpButton.SetActive(false);
                if (gridLevelDownButton.activeSelf == true) gridLevelDownButton.SetActive(false);
            }
        }

        private void OnDisable()
        {            
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.OnBuildableGridObjectTypeSOListChange -= OnBuildableGridObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnBuildableEdgeObjectTypeSOListChange -= OnBuildableEdgeObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnBuildableFreeObjectTypeSOListChange -= OnBuildableFreeObjectTypeSOListChangeMethod;
                easyGridBuilderPro.OnGridModeChange -= OnGridModeChangeMethod;
            }
            MultiGridManager.Instance.OnActiveGridChanged -= OnActiveGridChangedMethod;
            OnCategoryButtonPressed -= OnCategoryButtonPressedMethod;
            OnBuildablesButtonPressed -= OnBuildablesButtonPressedMethod;
        }

        private void Update()
        {
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            HandleHelpMenuInputs();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                             HANDLE MAIN BUTTONS                                                                                          //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void BuildButton()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetGridModeBuilding();
            }
        }

        public void DestroyButton()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetGridModeDestruction();
            }
        }

        public void SelectionButton()
        {
            gridObjectSelector.SetGridModeSelection();
        }

        public void SaveButton()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridSave();
            }
        }

        public void LoadButton()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridLoad();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                         HANDLE HELP MENU INPUTS                                                                                          //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void HandleHelpMenuInputs()
        {
            if (inputGroupObject.activeSelf)
            {
                if (gridModeResetText) gridModeResetText.text = gridBuilderProInputsSO.gridModeResetKey.bindings[0].ToDisplayString();
                if (gridModeResetText) gridHeightChangeText.text = gridBuilderProInputsSO.gridHeightChangeKey.bindings[0].ToDisplayString();
                if (gridModeResetText) buildModeActiveText.text = gridBuilderProInputsSO.buildModeActivationKey.bindings[0].ToDisplayString();
                if (gridModeResetText) placementText.text = gridBuilderProInputsSO.buildablePlacementKey.bindings[0].ToDisplayString();
                if (gridModeResetText) listScrollText.text = gridBuilderProInputsSO.buildableListScrollKey.bindings[0].ToDisplayString();
                if (gridModeResetText) ghostRotateLText.text = gridBuilderProInputsSO.ghostRotateLeftKey.bindings[0].ToDisplayString();
                if (gridModeResetText) ghostRotateRText.text = gridBuilderProInputsSO.ghostRotateRightKey.bindings[0].ToDisplayString();
                if (gridModeResetText) destructionModeActiveText.text = gridBuilderProInputsSO.destructionModeActivationKey.bindings[0].ToDisplayString();
                if (gridModeResetText) destroyText.text = gridBuilderProInputsSO.buildableDestroyKey.bindings[0].ToDisplayString();
                if (gridModeResetText) selectionModeActiveText.text = gridBuilderProInputsSO.selectionModeActivationKey.bindings[0].ToDisplayString();
                if (gridModeResetText) selectionText.text = gridBuilderProInputsSO.buildableSelectionKey.bindings[0].ToDisplayString();
                if (gridModeResetText) saveText.text = gridBuilderProInputsSO.gridSaveKey.bindings[0].ToDisplayString();
                if (gridModeResetText) loadText.text = gridBuilderProInputsSO.gridLoadKey.bindings[0].ToDisplayString();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                         HANDLE BUILDABLES LIST                                                                                           //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void HandleCategorySection()
        {
            foreach (BuildableObjectTypeCategorySO item in buildableObjectTypeCategorySO)
            {
                if (categorySection && placeHolderCategory)
                {
                    Transform categoryObject = Instantiate(placeHolderCategory, Vector3.zero, Quaternion.identity).transform;
                    categoryObject.SetParent(categorySection.transform, false);
                    categoryObject.position = Vector3.zero;
                    categoryObject.gameObject.name = item.categoryName;
                    categoryObject.GetChild(0).GetComponent<Image>().sprite = item.categoryIcon;

                    Transform buildableSectionCategoryObject = Instantiate(placeHolderBuildableSectionCategory, Vector3.zero, Quaternion.identity).transform;
                    buildableSectionCategoryObject.SetParent(buildablesSection.transform, false);
                    buildableSectionCategoryObject.position = buildablesSection.transform.position;
                    buildableSectionCategoryObject.gameObject.name = item.categoryName;
                    buildableSectionCategoryObject.gameObject.SetActive(false);

                    instantiatedCategoryObjectsList.Add(categoryObject.gameObject);
                    instantiatedBuildableSectionCategoryList.Add(buildableSectionCategoryObject.gameObject);

                    categoryObject.GetComponent<Button>().onClick.AddListener(delegate { OnCategoryButtonPressed(categoryObject.name); });
                }
            }
        }

        private void OnCategoryButtonPressedMethod(string buttonName)
        {
            foreach (GameObject categoryButton in instantiatedBuildableSectionCategoryList)
            {
                if (buttonName == categoryButton.name)
                {
                    foreach (GameObject buildableCategorySection in instantiatedBuildableSectionCategoryList)
                    {
                        if (buildableCategorySection.name == categoryButton.name)
                        {
                            if (buildableCategorySection.activeSelf == false) buildableCategorySection.SetActive(true);
                        }
                        else
                        {
                            if (buildableCategorySection.activeSelf == true) buildableCategorySection.SetActive(false);
                        }
                    }
                }
            }
        }

        private void OnGridModeChangeMethod(object sender, EventArgs e)
        {
            if (showInGridModeDefault)
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None)
                {
                    if (!animationOpenTrigger)
                    {
                        buildableListAnimator.SetTrigger("Open");
                        animationOpenTrigger = true;
                        animationCloseTrigger = false;
                    }
                }
            }
            else
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None)
                {
                    if (!animationCloseTrigger)
                    {
                        buildableListAnimator.SetTrigger("Close");
                        animationOpenTrigger = false;
                        animationCloseTrigger = true;
                    }
                }
            }

            if (showInGridModeBuild)
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Build)
                {
                    if (!animationOpenTrigger)
                    {
                        buildableListAnimator.SetTrigger("Open");
                        animationOpenTrigger = true;
                        animationCloseTrigger = false;
                    }
                }
            }
            else
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Build)
                {
                    if (!animationCloseTrigger)
                    {
                        buildableListAnimator.SetTrigger("Close");
                        animationOpenTrigger = false;
                        animationCloseTrigger = true;
                    }
                }
            }

            if (showInGridModeDestruction)
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Destruct)
                {
                    if (!animationOpenTrigger)
                    {
                        buildableListAnimator.SetTrigger("Open");
                        animationOpenTrigger = true;
                        animationCloseTrigger = false;
                    }
                }
            }
            else
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Destruct)
                {
                    if (!animationCloseTrigger)
                    {
                        buildableListAnimator.SetTrigger("Close");
                        animationOpenTrigger = false;
                        animationCloseTrigger = true;
                    }
                }
            }

            if (showInGridModeSelection)
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Selected)
                {
                    if (!animationOpenTrigger)
                    {
                        buildableListAnimator.SetTrigger("Open");
                        animationOpenTrigger = true;
                        animationCloseTrigger = false;
                    }
                }
            }
            else
            {
                if (currentActiveSystem.GetGridMode() == GridMode.Selected)
                {
                    if (!animationCloseTrigger)
                    {
                        buildableListAnimator.SetTrigger("Close");
                        animationOpenTrigger = false;
                        animationCloseTrigger = true;
                    }
                }
            }
        }
        
        private void HandleBuildablesListSection()
        {
            buildableGridObjectTypeSOList = currentActiveSystem.GetBuildableGridObjectTypeSOList();
            buildableEdgeObjectTypeSOList = currentActiveSystem.GetBuildableEdgeObjectTypeSOList();
            buildableFreeObjectTypeSOList = currentActiveSystem.GetBuildableFreeObjectTypeSOList();
            ClearInstantiatedBuildableObjectsList();

            foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
            {
                CreateBuidableGridGameObject(item);
            }
            foreach (BuildableEdgeObjectTypeSO item in buildableEdgeObjectTypeSOList)
            {
                CreateBuidableEdgeGameObject(item);
            }
            foreach (BuildableFreeObjectTypeSO item in buildableFreeObjectTypeSOList)
            {
                CreateBuidableFreeGameObject(item);
            }
        }

        private void OnBuildableGridObjectTypeSOListChangeMethod()
        {
            buildableGridObjectTypeSOList = currentActiveSystem.GetBuildableGridObjectTypeSOList();
            buildableEdgeObjectTypeSOList = currentActiveSystem.GetBuildableEdgeObjectTypeSOList();
            buildableFreeObjectTypeSOList = currentActiveSystem.GetBuildableFreeObjectTypeSOList();
            ClearInstantiatedBuildableObjectsList();

            foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
            {
                CreateBuidableGridGameObject(item);
            }
            foreach (BuildableEdgeObjectTypeSO item in buildableEdgeObjectTypeSOList)
            {
                CreateBuidableEdgeGameObject(item);
            }
            foreach (BuildableFreeObjectTypeSO item in buildableFreeObjectTypeSOList)
            {
                CreateBuidableFreeGameObject(item);
            }
        }

        private void OnBuildableEdgeObjectTypeSOListChangeMethod()
        {
            buildableGridObjectTypeSOList = currentActiveSystem.GetBuildableGridObjectTypeSOList();
            buildableEdgeObjectTypeSOList = currentActiveSystem.GetBuildableEdgeObjectTypeSOList();
            buildableFreeObjectTypeSOList = currentActiveSystem.GetBuildableFreeObjectTypeSOList();
            ClearInstantiatedBuildableObjectsList();

            foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
            {
                CreateBuidableGridGameObject(item);
            }
            foreach (BuildableEdgeObjectTypeSO item in buildableEdgeObjectTypeSOList)
            {
                CreateBuidableEdgeGameObject(item);
            }
            foreach (BuildableFreeObjectTypeSO item in buildableFreeObjectTypeSOList)
            {
                CreateBuidableFreeGameObject(item);
            }
        }

        private void OnBuildableFreeObjectTypeSOListChangeMethod()
        {
            buildableGridObjectTypeSOList = currentActiveSystem.GetBuildableGridObjectTypeSOList();
            buildableEdgeObjectTypeSOList = currentActiveSystem.GetBuildableEdgeObjectTypeSOList();
            buildableFreeObjectTypeSOList = currentActiveSystem.GetBuildableFreeObjectTypeSOList();
            ClearInstantiatedBuildableObjectsList();

            foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
            {
                CreateBuidableGridGameObject(item);
            }
            foreach (BuildableEdgeObjectTypeSO item in buildableEdgeObjectTypeSOList)
            {
                CreateBuidableEdgeGameObject(item);
            }
            foreach (BuildableFreeObjectTypeSO item in buildableFreeObjectTypeSOList)
            {
                CreateBuidableFreeGameObject(item);
            }
        }

        private void OnActiveGridChangedMethod(EasyGridBuilderPro currentActiveSystem)
        {
            this.currentActiveSystem = currentActiveSystem;

            buildableGridObjectTypeSOList = currentActiveSystem.GetBuildableGridObjectTypeSOList();
            buildableEdgeObjectTypeSOList = currentActiveSystem.GetBuildableEdgeObjectTypeSOList();
            buildableFreeObjectTypeSOList = currentActiveSystem.GetBuildableFreeObjectTypeSOList();
            ClearInstantiatedBuildableObjectsList();

            foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
            {
                CreateBuidableGridGameObject(item);
            }
            foreach (BuildableEdgeObjectTypeSO item in buildableEdgeObjectTypeSOList)
            {
                CreateBuidableEdgeGameObject(item);
            }
            foreach (BuildableFreeObjectTypeSO item in buildableFreeObjectTypeSOList)
            {
                CreateBuidableFreeGameObject(item);
            }

            //HANDLE VERTICAL GRID LEVEL BUTTONS  

            if (currentActiveSystem.gridEditorMode == GridEditorMode.GridLite)
            {
                if (gridLevelUpButton.activeSelf == true) gridLevelUpButton.SetActive(false);
                if (gridLevelDownButton.activeSelf == true) gridLevelDownButton.SetActive(false);
            }
            else if (currentActiveSystem.gridEditorMode == GridEditorMode.GridPro)
            {
                if (gridLevelUpButton.activeSelf == false) gridLevelUpButton.SetActive(true);
                if (gridLevelDownButton.activeSelf == false) gridLevelDownButton.SetActive(true);
            }
            else
            {
                if (gridLevelUpButton.activeSelf == true) gridLevelUpButton.SetActive(false);
                if (gridLevelDownButton.activeSelf == true) gridLevelDownButton.SetActive(false);
            }
        }

        private void CreateBuidableGridGameObject(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            if (buildablesSection && placeHolderBuildable)
            {
                Transform buildableObject = Instantiate(placeHolderBuildable, Vector3.zero, Quaternion.identity).transform;

                foreach (GameObject item in instantiatedBuildableSectionCategoryList)
                {
                    if (buildableGridObjectTypeSO.buildableCategorySO.categoryName == item.name)
                    {
                        buildableObject.SetParent(item.transform, false);
                        buildableObject.position = Vector3.zero;
                        buildableObject.gameObject.name = buildableGridObjectTypeSO.objectName;
                        buildableObject.GetChild(0).GetComponent<Image>().sprite = buildableGridObjectTypeSO.objectIcon;

                        if (buildableGridObjectTypeSO.enableBuildCondition && buildableGridObjectTypeSO.buildConditionSO)
                        {
                            if (buildableObject.GetComponent<UIBuildableSODataContainer>())
                            {
                                UIBuildableSODataContainer container = buildableObject.GetComponent<UIBuildableSODataContainer>();
                                container.SetBuildConditionToolTipContent(buildableGridObjectTypeSO.buildConditionSO.tooltipContent);
                            }
                        }
                    }
                }

                instantiatedBuildableObjectsList.Add(buildableObject.gameObject);

                buildableObject.GetComponent<Button>().onClick.AddListener(delegate { OnBuildablesButtonPressed(buildableObject.name); });
            }
        }

        private void CreateBuidableEdgeGameObject(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO)
        {
            if (buildablesSection && placeHolderBuildable)
            {
                Transform buildableObject = Instantiate(placeHolderBuildable, Vector3.zero, Quaternion.identity).transform;

                foreach (GameObject item in instantiatedBuildableSectionCategoryList)
                {
                    if (buildableEdgeObjectTypeSO.buildableCategorySO.categoryName == item.name)
                    {
                        buildableObject.SetParent(item.transform, false);
                        buildableObject.position = Vector3.zero;
                        buildableObject.gameObject.name = buildableEdgeObjectTypeSO.objectName;
                        buildableObject.GetChild(0).GetComponent<Image>().sprite = buildableEdgeObjectTypeSO.objectIcon;

                        if (buildableEdgeObjectTypeSO.enableBuildCondition && buildableEdgeObjectTypeSO.buildConditionSO)
                        {
                            if (buildableObject.GetComponent<UIBuildableSODataContainer>())
                            {
                                UIBuildableSODataContainer container = buildableObject.GetComponent<UIBuildableSODataContainer>();
                                container.SetBuildConditionToolTipContent(buildableEdgeObjectTypeSO.buildConditionSO.tooltipContent);
                            }
                        }
                    }
                }

                instantiatedBuildableObjectsList.Add(buildableObject.gameObject);

                buildableObject.GetComponent<Button>().onClick.AddListener(delegate { OnBuildablesButtonPressed(buildableObject.name); });
            }
        }

        private void CreateBuidableFreeGameObject(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO)
        {
            if (buildablesSection && placeHolderBuildable)
            {
                Transform buildableObject = Instantiate(placeHolderBuildable, Vector3.zero, Quaternion.identity).transform;

                foreach (GameObject item in instantiatedBuildableSectionCategoryList)
                {
                    if (buildableFreeObjectTypeSO.buildableCategorySO.categoryName == item.name)
                    {
                        buildableObject.SetParent(item.transform, false);
                        buildableObject.position = Vector3.zero;
                        buildableObject.gameObject.name = buildableFreeObjectTypeSO.objectName;
                        buildableObject.GetChild(0).GetComponent<Image>().sprite = buildableFreeObjectTypeSO.objectIcon;

                        if (buildableFreeObjectTypeSO.enableBuildCondition && buildableFreeObjectTypeSO.buildConditionSO)
                        {
                            if (buildableObject.GetComponent<UIBuildableSODataContainer>())
                            {
                                UIBuildableSODataContainer container = buildableObject.GetComponent<UIBuildableSODataContainer>();
                                container.SetBuildConditionToolTipContent(buildableFreeObjectTypeSO.buildConditionSO.tooltipContent);
                            }
                        }
                    }
                }

                instantiatedBuildableObjectsList.Add(buildableObject.gameObject);

                buildableObject.GetComponent<Button>().onClick.AddListener(delegate { OnBuildablesButtonPressed(buildableObject.name); });
            }
        }

        private void ClearInstantiatedBuildableObjectsList()
        {
            foreach (GameObject item in instantiatedBuildableObjectsList)
            {
                Destroy(item);
            }
            instantiatedBuildableObjectsList.Clear();
        }

        private void OnBuildablesButtonPressedMethod(string buttonName)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerBuildableListUI(buttonName);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                 HANDLE VERTICAL GRID LEVEL BUTTONS                                                                                       //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void TriggerVerticalGridUp()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridHeightChangeUI(new Vector2(1, 1));
            }
        }

        public void TriggerVerticalGridDown()
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridHeightChangeUI(new Vector2(-1, -1));
            }
        }
    }
}