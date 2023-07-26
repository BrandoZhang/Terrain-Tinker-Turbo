using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textToBlink = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Blink(textToBlink, 3, 0.2f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Blink(TextMeshProUGUI text, float duration, float blinkTime)
    {
        float endTime = Time.time + duration;
        bool isVisible = true;

        while (Time.time < endTime)
        {
            isVisible = !isVisible;
            text.alpha = isVisible ? 1 : 0;

            yield return new WaitForSeconds(blinkTime);
        }

        // Make sure the text is visible when the blinking ends
        text.alpha = 1;
    }

}
