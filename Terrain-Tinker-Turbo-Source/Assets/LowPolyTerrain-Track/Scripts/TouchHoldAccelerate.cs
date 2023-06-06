using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHoldAccelerate : MonoBehaviour {


    bool move = false;
    public bool isGrounded = false;
    bool isRespawn = false;

    Rigidbody rb;

    //[HideInInspector]
    GameObject flipDecetor;
    //[HideInInspector]
    Vector3 startPos;

    Vector3 endPos;
    public float deadHeight = 0;

    public float speed = 20f;
    public float rotationSpeed = 2f;

    RaycastHit hit;
    float maxDist = 100.0f;

    
    void Start () {
        if (transform.GetComponent<Rigidbody>())
        {
            rb = transform.GetComponent<Rigidbody>();
        }
        else
        {
            print("The object requires a rigid body.");
        }

        if (transform.GetChild(0).gameObject.name == "FlipDetector")
        {
            flipDecetor = transform.GetChild(0).gameObject;
        }
        else
        {
            print("There is no FlipDetector.");
        }

        startPos = transform.position;
        endPos = startPos;
        isRespawn = true;
    }
	
	void Update () {
        if (transform.position.y < deadHeight)
        {
            endPos = transform.position;
            ReGeneration();
        }

        if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //transform.Translate(transform.forward *Time.deltaTime * speed);
            //rb.velocity = transform.forward * Time.deltaTime * speed;
            move = true;
        }
        else
        {
            move = false;
        }
    }

    private void FixedUpdate()
    {
        if (move == true)
        {
            if (isGrounded)
            {
                //rb.AddForce(transform.forward * speed * Time.fixedDeltaTime * 100f, ForceMode.Force);
                rb.AddForce(transform.forward * speed * Time.fixedDeltaTime * 100f, ForceMode.Acceleration);
            }
            else
            {
                rb.AddTorque(-transform.right * rotationSpeed * Time.fixedDeltaTime * 100f, ForceMode.Force);
            }
        }
        else
        {
            if (!isGrounded && !isRespawn)
            {
                rb.AddTorque(transform.right * rotationSpeed * Time.fixedDeltaTime * 10f, ForceMode.Force);
                //rb.velocity = rb.velocity * Time.deltaTime * 50f; //rb.velocity = rb.velocity * 0.9f
                rb.AddRelativeForce(Vector3.back * 6.0f);   //Deceleration when floating.

            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        if (isRespawn)
        {
            isRespawn = false;
        }

        Collider myCollider = collision.contacts[0].thisCollider;
        if (myCollider.gameObject.name == flipDecetor.name)
        {
            if (collision.gameObject.GetComponent<StuntTrap>() && collision.gameObject.GetComponent<StuntTrap>().trapType == StuntTrap.TrapType.Domino)
            {
                
            }
            else
            {
                endPos = transform.position;
                ReGeneration();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    public void ReGeneration(GameObject _gameobject = null)
    {

        Vector3 newPosition;
        newPosition = new Vector3(endPos.x - 5, 20, startPos.z);

        for (int i = 0; i < 10; i++)
        {
            bool result = Physics.Raycast(newPosition + Vector3.left * i * 5, Vector3.down, out hit, maxDist);

            if (result && !hit.transform.GetComponent<StuntTrap>())
            {
                //Debug.Log(hit.collider.name + " / " + hit.transform.position);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                transform.position = hit.transform.position + new Vector3(0,5, startPos.z);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                isRespawn = true;

                break;
            }
        }
        /*
        if (_gameobject != null)
        {

        }
        else
        {

        }
        */
    }
}
