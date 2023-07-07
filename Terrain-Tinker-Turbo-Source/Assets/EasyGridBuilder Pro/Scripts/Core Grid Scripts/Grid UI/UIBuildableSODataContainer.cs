using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class UIBuildableSODataContainer : MonoBehaviour
    {
        private string uniqueObjectName;
        private string objectDescription;
        private string objectToolTipDescription;
        private string buildConditionToolTipContent;
        private Sprite objectIcon;
        private BuildableObjectTypeCategorySO buildableCategorySO;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                                SET FUNCTIONS                                                                                             //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void SetUniqueObjectName(string uniqueObjectName)
        {
            this.uniqueObjectName = uniqueObjectName;
        }

        public void SetObjectDescription(string objectDescription)
        {
            this.objectDescription = objectDescription;
        }

        public void SetObjectToolTipDescription(string objectToolTipDescription)
        {
            this.objectToolTipDescription = objectToolTipDescription;
        }

        public void SetBuildConditionToolTipContent(string buildConditionToolTipContent)
        {
            this.buildConditionToolTipContent = buildConditionToolTipContent;
        }

        public void SetObjectIcon(Sprite objectIcon)
        {
            this.objectIcon = objectIcon;
        }

        public void SetBuildableCategorySO(BuildableObjectTypeCategorySO buildableCategorySO)
        {
            this.buildableCategorySO = buildableCategorySO;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                                GET FUNCTIONS                                                                                             //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        
        public string GetUniqueObjectName()
        {
            return uniqueObjectName;
        }

        public string GetObjectDescription()
        {
            return objectDescription;
        }

        public string GetObjectToolTipDescription()
        {
            return objectToolTipDescription;
        }

        public string GetBuildConditionToolTipContent()
        {
            return buildConditionToolTipContent;
        }

        public Sprite GetObjectIcon()
        {
            return objectIcon;
        }
        
        public BuildableObjectTypeCategorySO GetBuildableCategorySO()
        {
            return buildableCategorySO;
        }
    }
}
