using UnityEngine;

namespace SoulGames.Utilities
{
    public class ToolTipSystem : MonoBehaviour
    {
        public static ToolTipSystem instance;
        [SerializeField]private ToolTipTrigger toolTipTrigger;

        void Awake()
        {
            instance = this;
        }

        public static void ShowToolTip(string content, string header = "")
        {
            instance.toolTipTrigger.SetText(content, header);
            instance.toolTipTrigger.gameObject.SetActive(true);
        }

        public static void HideToolTip()
        {
            instance.toolTipTrigger.gameObject.SetActive(false);
        }
    }
}