using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SimpleRunner : MonoBehaviour
{
    //mouse & touch
    Vector3 firstPos, gap;
    bool wait;

    [Header("[Bike Controller]")]
    public float speed = 10;

    int posState = 0;    //-1 = left, 0 = center, 1 = right
    Vector3 targetPos;
    bool isSliding;
    
    public float jumpHeight = 1f;
    public float laneGap = 1;
    public Renderer bikeBody;
    public Animator anim;

    public ParticleSystem explosion;
    
    [Header("[Background Scroller]")]
    public int randomTrackNum;
    GameObject tracksPool;

    public GameObject[] tracks;
    public List<GameObject> trackPieces = new List<GameObject>();
    public float scrollSpeed = 1;
    public float tileSizeZ = 1;
    public int tileShiftCount = 1;
    public int trackPiece = 2;
    
    private Vector3 startPosition;

    [Header("[UI]")]
    public Text gemCountTex;
    public GameObject endGamePannel;
    public float gemCost = 1f;
    public float gemScore = 0;
    private float CostCount = 1f;
    //private float startBetweenWaves;
    bool hitGem;
    

    void Start()
    {
        //background scroller
        if (GameObject.Find("TracksPool"))
        {
            tracksPool = GameObject.Find("TracksPool");
            startPosition = tracksPool.transform.position;
        }
        else
        {
            print("GameObject.Find(TracksPool) is empty");
        }

        if (trackPieces.Count < trackPiece)
        {
            SpawnTrack();
        }

        //UI
        CostCount = gemCost;
        if (endGamePannel.activeSelf)
        {
            endGamePannel.SetActive(false);
        }
    }

    
    void Update()
    {
        // touch
        if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            wait = true;
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            
            gap = (Input.GetMouseButtonUp(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
            gap.Normalize();
            
            if (wait)
            {
                wait = false;
                // jump/up
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f) SwipeAction(0, 1);
                // sliding/down
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f) SwipeAction(0, -1);
                // right
                if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f) SwipeAction(1, 0);
                // left
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f) SwipeAction(-1, 0);
                else return;
            }
        }

        if (targetPos.y == 1)
        {
            float jumpDist = Vector3.Distance(targetPos, transform.position);
            if (jumpDist < 0.1f)
            {
                targetPos = new Vector3(posState * laneGap, 0, 0);  //go to back
            }
        }

        if (isSliding)
        {
            AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] animatorClip = anim.GetCurrentAnimatorClipInfo(0);
            float slidingTime = animatorClip[0].clip.length * animationState.normalizedTime;

            if (slidingTime > 0.95f)
            {
                if (transform.GetComponent<BoxCollider>().center.y != 0.5f)
                {
                    transform.GetComponent<BoxCollider>().center = new Vector3(0, 0.5f, 0);
                }
                isSliding = false;
            }
        }
        
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);


        //background scroller
        /*
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        tracksPool.transform.position = startPosition + Vector3.back * newPosition;
        }
        */
        
        float poolDist = Vector3.Distance(startPosition, tracksPool.transform.position);
        
        if (poolDist > tileSizeZ)
        {
            tracksPool.transform.position = startPosition;

            //remove passed track
            Destroy(trackPieces[0].gameObject);
            trackPieces.RemoveAt(0);
            //track pieces position realignment
            for (int i = 0; i < trackPieces.Count; i++)
            {
                trackPieces[i].transform.localPosition = new Vector3(0, 0, i * tileSizeZ); ;
            }
            SpawnTrack();
        }
        else
        {
            tracksPool.transform.Translate(-transform.forward * Time.deltaTime * scrollSpeed);
        }
        /*
         * float partDist = Vector3.Distance(startPosition, trackPieces[0].transform.position);
        if (partDist > tileSizeZ * tileShiftCount & poolDist < 51)
        {
            //GameObject removePart = partLists[0].gameObject;

            Destroy(partLists[0].gameObject);
            partLists.RemoveAt(0);

            //spawn
            print("<color=yellow>2.partLists.Count</color> " + partLists.Count);
            SpawnTrack();
        }
        */

        //gem count
        if (hitGem)
        {

            if (CostCount <= 0f)
            {
                hitGem = false;
                CostCount = gemCost;
                gemScore += gemCost;

                return;
            }

            CostCount -= Time.deltaTime * (10 * gemCost);
        }
        CostCount = Mathf.Clamp(0f, CostCount, Mathf.Infinity);
        float tmp = gemScore + (gemCost - CostCount);
        //0317gemCountTex.text = string.Format("{0:F0}", tmp);
    }

    public void SwipeAction(int _xDir, int _yDir)
    {
        int targetLane = posState + _xDir;

        if (targetLane > -2 && targetLane < 2)
        {
            switch (targetLane - posState)
            {
                case -1:
                    anim.SetTrigger("Left");
                    break;
                case 1:
                    anim.SetTrigger("Right");
                    break;
                default:
                    break;
            }
            targetPos = new Vector3(targetLane, 0, 0);
            posState = targetLane;
        }
        else if (targetLane < -1 || targetLane > 1)
        {
            print("out of range" + targetLane);
            
            switch (targetLane)
            {
                case -2:
                    anim.SetTrigger("LeftOver");
                    break;
                case 2:
                    anim.SetTrigger("RightOver");
                    break;
                default:
                    break;
            }
        }

        if (_yDir > 0)
        {
            anim.SetTrigger("Jump");
            targetPos = new Vector3(posState * laneGap, jumpHeight, 0);
            transform.GetComponent<Rigidbody>().useGravity = false;
        }

        if (_yDir < 0)
        {
            anim.SetTrigger("Sliding");
            //collider scale
            BoxCollider bikeCollider = transform.GetComponent<BoxCollider>();
            bikeCollider.center = new Vector3(0, 0.2f, 0);
            isSliding = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string otherName;
        //otherName = (other.name.ToString()).Substring(0, 8);
        otherName = other.transform.parent.gameObject.name;

        if (otherName == "Obstacle")
        {
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }

            if (bikeBody)
            {
                bikeBody.enabled = false;
            }

            targetPos = new Vector3(0, 0, 0);
            posState = 0;
            transform.GetComponent<BoxCollider>().enabled = false;
            endGamePannel.SetActive(true);
        }
        else
        {
            hitGem = true;
        }
    }

    public void SpawnTrack()
    {
        int partCountNum = trackPieces.Count;
        for (int i = 0; i < trackPiece - partCountNum; i++)
        {
            randomTrackNum = Random.Range(0, tracks.Length);
            
            GameObject emptyTrack = Instantiate(tracks[randomTrackNum], Vector3.zero, Quaternion.identity);
            emptyTrack.transform.SetParent(tracksPool.transform);
            trackPieces.Add(emptyTrack);
            
            //at last one position
            if (trackPieces.Count > 1)
            {
                emptyTrack.transform.localPosition = trackPieces[trackPieces.Count - 2].transform.localPosition + new Vector3(0, 0, tileSizeZ);
            }
        }
    }

    public void GameRestart()
    {
        if (bikeBody & !bikeBody.enabled)
        {
            bikeBody.enabled = true;
        }

        targetPos = new Vector3(0, 0, 0);
        transform.GetComponent<BoxCollider>().enabled = true;
        endGamePannel.SetActive(false);
    }
}