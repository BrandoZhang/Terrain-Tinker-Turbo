using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SoulGames.Utilities
{
    public class ToolTipTrigger : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI headerField;
        [SerializeField]private TextMeshProUGUI contentField;
        [SerializeField]private LayoutElement layoutElement;
        [SerializeField]private int wrapLimit;
        [SerializeField]private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            Vector2 position  = new Vector2 (Input.mousePosition.x - 16, Input.mousePosition.y + 16);
            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }

            contentField.text = content;

            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;
            layoutElement.enabled = (headerLength > wrapLimit || contentLength > wrapLimit) ? true : false;
        }
    }
}
