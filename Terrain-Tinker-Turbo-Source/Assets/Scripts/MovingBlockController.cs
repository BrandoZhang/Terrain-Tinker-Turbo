using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockController : MonoBehaviour
{
    public Transform startPos;  // One end of the path
    public Transform endPos;  // The other end of the path
    public float speed = 5.0f;  // Speed of the movement

    private Vector3 startRelPos;
    private Vector3 endRelPos;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial start and end positions
        startRelPos = transform.parent.InverseTransformPoint(startPos.position);
        endRelPos = transform.parent.InverseTransformPoint(endPos.position);
        
        // Start the MoveObject coroutine
        StartCoroutine(MoveBlock());
    }

    IEnumerator MoveBlock()
    {
        while (true)
        {
            yield return Move(startRelPos, endRelPos, speed);
            yield return Move(endRelPos, startRelPos, speed);
        }
    }

    IEnumerator Move(Vector3 start, Vector3 end, float speed)
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(start, end);

        while (true)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.localPosition = Vector3.Lerp(start, end, fractionOfJourney);

            if (fractionOfJourney >= 1)
            {
                break;
            }

            yield return null;
        }
    }
}
