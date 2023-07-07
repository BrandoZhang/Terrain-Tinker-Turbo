using UnityEngine;
using UnityEngine.EventSystems;
using SoulGames.EasyGridBuilderPro;

namespace SoulGames.Utilities
{
    public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("This text will display on top of the tooltip as a Header")]
        [SerializeField]private string header;
        [Tooltip("This text will display below the Header of the tooltip")]
        [SerializeField]private string content;
        [Tooltip("Header text will be ignored and this game object's name will be used as the Header instead")]
        [SerializeField]private bool objectNameAsHeader;
        [Tooltip("Content text will be ignored and Object Build SO ToolTip will be used as the Content instead \nDo not use this without UI Buildable SO Data Container. (UI Feature Only)")]
        [SerializeField]private bool objectBuildSOToolTipAsContent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (objectNameAsHeader) header = gameObject.name;
            if (objectBuildSOToolTipAsContent)
            {
                if (GetComponent<UIBuildableSODataContainer>())
                {
                    if (GetComponent<UIBuildableSODataContainer>().GetBuildConditionToolTipContent() != null)
                    {
                        content = GetComponent<UIBuildableSODataContainer>().GetBuildConditionToolTipContent();
                    }
                }
            }
            ToolTipSystem.ShowToolTip(content, header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTipSystem.HideToolTip();
        }

        private void OnMouseEnter()
        {
            if (objectNameAsHeader) header = gameObject.name;
            ToolTipSystem.ShowToolTip(content, header);
        }

        private void OnMouseExit()
        {
            ToolTipSystem.HideToolTip();
        }
    }
}