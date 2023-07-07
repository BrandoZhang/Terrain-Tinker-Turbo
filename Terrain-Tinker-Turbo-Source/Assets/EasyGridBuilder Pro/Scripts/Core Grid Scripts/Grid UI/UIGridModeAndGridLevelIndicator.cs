using UnityEngine;
using TMPro;

namespace SoulGames.EasyGridBuilderPro
{
    public class UIGridModeAndGridLevelIndicator : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI gridModeText;
        [SerializeField]private TextMeshProUGUI verticalGridText;
        private EasyGridBuilderPro currentActiveSystem;
        private GridMode gridMode;

        private void Start()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            MultiGridManager.Instance.OnActiveGridChanged += OnGridSystemChanged;
        }
        
        private void OnDestroy()
        {
            MultiGridManager.Instance.OnActiveGridChanged -= OnGridSystemChanged;  
        }

        private void OnGridSystemChanged(EasyGridBuilderPro currentActiveSystem)
        {
            this.currentActiveSystem = currentActiveSystem;
        }

        private void Update()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;

            HandleText();
        }

        private void HandleText()
        {
            if (gridModeText)
            {
                gridMode = currentActiveSystem.GetGridMode();
                switch (gridMode)
                {
                    case GridMode.None:
                        gridModeText.text = "Default";
                    break;
                    case GridMode.Destruct:
                        gridModeText.text = "Destruct";
                    break;
                    case GridMode.Build:
                        gridModeText.text = "Build";
                    break;
                    case GridMode.Selected:
                        gridModeText.text = "Selection";
                    break;
                    case GridMode.Moving:
                        gridModeText.text = "Moving";
                    break;
                }
            }

            if (verticalGridText)
            {
                verticalGridText.text = currentActiveSystem.GetActiveVerticalGridIndex().ToString();
            }
        }
    }
}
