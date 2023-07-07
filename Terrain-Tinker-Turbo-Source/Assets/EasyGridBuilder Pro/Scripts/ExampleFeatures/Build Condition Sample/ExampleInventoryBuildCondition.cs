using UnityEngine;
using TMPro;
using SoulGames.EasyGridBuilderPro;
using System;

namespace SoulGames.Examples
{
    public class ExampleInventoryBuildCondition : MonoBehaviour
    {
        [Header("Inventory Resources Amount")] [Space]
        [SerializeField]private int currentFoodInInventory = 10;
        [SerializeField]private int currentMetalInInventory = 10;
        [SerializeField]private int currentWoodInInventory = 10;
        [Header("Inventory Add Resources")] [Space]
        [SerializeField]private int foodAddAmount = 5;
        [SerializeField]private int metalAddAmount = 5;
        [SerializeField]private int woodAddAmount = 5;
        [Header("Inventory UI")] [Space]
        [SerializeField]private AudioClip UIClickSound;
        [SerializeField]private TextMeshProUGUI foodAmountText;
        [SerializeField]private TextMeshProUGUI metalAmountText;
        [SerializeField]private TextMeshProUGUI woodAmountText;
        [Header("Debug")] [Space]
        [SerializeField]private bool showConsoleText = true;
        
        private void Update()
        {
            UpdateUIText();
        }

        //This function constantly update Inventory UI Text to display current resources amounts
        private void UpdateUIText()
        {
            if (foodAmountText) foodAmountText.text = currentFoodInInventory.ToString();
            if (metalAmountText) metalAmountText.text = currentMetalInInventory.ToString();
            if (woodAmountText) woodAmountText.text = currentWoodInInventory.ToString();
        }

        //This function Add selected amount of Food to the inventory
        //Currently calling through Inventory UI Food Button
        public void AddFood()
        {
            currentFoodInInventory = currentFoodInInventory + foodAddAmount;
            if (UIClickSound) AudioSource.PlayClipAtPoint(UIClickSound, transform.position);
            if (showConsoleText) Debug.Log("<color=green>Food added :</color> " + foodAddAmount + " <color=green>Current Food amount in inventory :</color>" + currentFoodInInventory);
        }
        //This function Add selected amount of Metal to the inventory
        //Currently calling through Inventory UI Metal Button
        public void AddMetal()
        {
            currentMetalInInventory = currentMetalInInventory + metalAddAmount;
            if (UIClickSound) AudioSource.PlayClipAtPoint(UIClickSound, transform.position);
            if (showConsoleText) Debug.Log("<color=green>Metal added :</color> " + metalAddAmount + " <color=green>Current Metal amount in inventory :</color>" + currentMetalInInventory);
        }
        //This function Add selected amount of Wood to the inventory
        //Currently calling through Inventory UI Wood Button
        public void AddWood()
        {
            currentWoodInInventory = currentWoodInInventory + woodAddAmount;
            if (UIClickSound) AudioSource.PlayClipAtPoint(UIClickSound, transform.position);
            if (showConsoleText) Debug.Log("<color=green>Wood added :</color> " + woodAddAmount + " <color=green>Current Wood amount in inventory :</color>" + currentWoodInInventory);
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                          EASY GRID BUILDER BUILD CONDITIONS INTEGRATION                                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        //Subscribing to events that are called from 'MultiGridBuildConditionManager'
        //Each subscribed event will have an own function, total of four functions
        private void OnEnable()
        {
            MultiGridBuildConditionManager.OnBuildConditionCheckBuildableGridObject += CheckBuildConditionBuildableGridObject;
            MultiGridBuildConditionManager.OnBuildConditionCompleteBuildableGridObject += CompleteBuildConditionBuildableGridObject;
            MultiGridBuildConditionManager.OnBuildConditionCheckBuildableFreeObject += CheckBuildConditionBuildableFreeObject;
            MultiGridBuildConditionManager.OnBuildConditionCompleteBuildableFreeObject += CompleteBuildConditionBuildableFreeObject;
        }

        //Unsubscribe from subscribed events when OnDisable function is called
        private void OnDisable()
        {
            MultiGridBuildConditionManager.OnBuildConditionCheckBuildableGridObject -= CheckBuildConditionBuildableGridObject;
            MultiGridBuildConditionManager.OnBuildConditionCompleteBuildableGridObject -= CompleteBuildConditionBuildableGridObject;
            MultiGridBuildConditionManager.OnBuildConditionCheckBuildableFreeObject -= CheckBuildConditionBuildableFreeObject;
            MultiGridBuildConditionManager.OnBuildConditionCompleteBuildableFreeObject -= CompleteBuildConditionBuildableFreeObject;
        }

        //First event function, This function is called before a 'BuildableGridObject' that has 'BuildConditions' enabled is placed on grid.
        //Function return a boolean value, If 'BuildConditions' met returns true, If not returns false. If 'BuildConditions' are met, 'BuildableGridObject' will be placed on the grid.
        //Since this function is called before object is placed, you can do condition checks in here and then consume materials in Second Function. Which is called after 'BuildableGridObject' is placed.
        private void CheckBuildConditionBuildableGridObject(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            foreach (BuildableGridObjectTypeSO item in MultiGridBuildConditionManager.BuildableGridObjectTypeSOList)
            {
                if (item == buildableGridObjectTypeSO && item.enableBuildCondition)
                {
                    //You can simply replace this if statement with your own conditions
                    if (buildableGridObjectTypeSO.buildConditionSO.foodAmount <= currentFoodInInventory &&
                        buildableGridObjectTypeSO.buildConditionSO.metalAmount <= currentMetalInInventory &&
                        buildableGridObjectTypeSO.buildConditionSO.woodAmount <= currentWoodInInventory)
                    //And you can leave rest of the code from here as it is
                    {
                        MultiGridBuildConditionManager.BuidConditionResponseBuildableGridObject = true;
                        return;
                    }
                    else
                    {
                        MultiGridBuildConditionManager.BuidConditionResponseBuildableGridObject = false;
                        return;
                    }
                }
            }
            MultiGridBuildConditionManager.BuidConditionResponseBuildableGridObject = false;
            return;
        }

        //Second event function, This function is called after the 'BuildableGridObject' is placed on grid.
        //Since this function is called after the 'BuildableGridObject' is placed, you can consume materials in here.
        private void CompleteBuildConditionBuildableGridObject(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            foreach (BuildableGridObjectTypeSO item in MultiGridBuildConditionManager.BuildableGridObjectTypeSOList)
            {
                //You can simply replace this if statement with your own conditions
                if (item == buildableGridObjectTypeSO && item.enableBuildCondition)
                {
                    if (buildableGridObjectTypeSO.buildConditionSO.consumeFoodOnBuild) currentFoodInInventory = currentFoodInInventory - buildableGridObjectTypeSO.buildConditionSO.foodAmount;
                    if (buildableGridObjectTypeSO.buildConditionSO.consumeMetalOnBuild) currentMetalInInventory = currentMetalInInventory - buildableGridObjectTypeSO.buildConditionSO.metalAmount;
                    if (buildableGridObjectTypeSO.buildConditionSO.consumeWoodOnBuild) currentWoodInInventory = currentWoodInInventory - buildableGridObjectTypeSO.buildConditionSO.woodAmount;
                }
                //And you can leave rest of the code from here as it is
            }
        }

        //Third event function, Similar to the first function this function is called before a 'BuildableFreeObject' that has 'BuildConditions' enabled is placed on grid.
        //Function return a boolean value, If 'BuildConditions' met returns true, If not returns false. If 'BuildConditions' are met, 'BuildableFreeObject' will be placed on the grid.
        //Since this function is called before object is placed, you can do condition checks in here and then consume materials in Fourth Function. Which is called after 'BuildableFreeObject' is placed.
        private void CheckBuildConditionBuildableFreeObject(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO)
        {
            foreach (BuildableFreeObjectTypeSO item in MultiGridBuildConditionManager.BuildableFreeObjectTypeSOList)
            {
                if (item == buildableFreeObjectTypeSO && item.enableBuildCondition)
                {
                    //You can simply replace this if statement with your own conditions
                    if (buildableFreeObjectTypeSO.buildConditionSO.foodAmount <= currentFoodInInventory &&
                        buildableFreeObjectTypeSO.buildConditionSO.metalAmount <= currentMetalInInventory &&
                        buildableFreeObjectTypeSO.buildConditionSO.woodAmount <= currentWoodInInventory)
                    //And you can leave rest of the code from here as it is
                    {
                        MultiGridBuildConditionManager.BuidConditionResponseBuildableFreeObject = true;
                        return;
                    }
                    else
                    {
                        MultiGridBuildConditionManager.BuidConditionResponseBuildableFreeObject = false;
                        return;
                    }
                }
            }
            MultiGridBuildConditionManager.BuidConditionResponseBuildableFreeObject = false;
            return;
        }

        //Fourth event function, This function is called after the 'BuildableFreeObject' is placed on grid.
        //Since this function is called after the 'BuildableFreeObject' is placed, you can consume materials in here.
        private void CompleteBuildConditionBuildableFreeObject(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO)
        {
            foreach (BuildableFreeObjectTypeSO item in MultiGridBuildConditionManager.BuildableFreeObjectTypeSOList)
            {
                //You can simply replace this if statement with your own conditions
                if (item == buildableFreeObjectTypeSO && item.enableBuildCondition)
                {
                    if (buildableFreeObjectTypeSO.buildConditionSO.consumeFoodOnBuild) currentFoodInInventory = currentFoodInInventory - buildableFreeObjectTypeSO.buildConditionSO.foodAmount;
                    if (buildableFreeObjectTypeSO.buildConditionSO.consumeMetalOnBuild) currentMetalInInventory = currentMetalInInventory - buildableFreeObjectTypeSO.buildConditionSO.metalAmount;
                    if (buildableFreeObjectTypeSO.buildConditionSO.consumeWoodOnBuild) currentWoodInInventory = currentWoodInInventory - buildableFreeObjectTypeSO.buildConditionSO.woodAmount;
                }
                //And you can leave rest of the code from here as it is
            }
        }

        /// <summary>
        /// In above four functions:
        /// <param name="buildableGridObjectTypeSO">Holds the Buildable Grid Object Type Scriptable Object</param>
        /// <param name="buildableFreeObjectTypeSO">Holds the Buildable Free Object Type Scriptable Object</param>
        /// <param name="buildConditionSO">Holds the Build Condition Scriptable Object That attached to on of above Scriptable Objects</param>
        /// <param name="foodAmount, metalAmount, woodAmount, consumeFoodOnBuild, currentMetalInInventory, currentWoodInInventory">These data from Build Condition Scriptable Object</param>
        /// </summary>
    }
}
