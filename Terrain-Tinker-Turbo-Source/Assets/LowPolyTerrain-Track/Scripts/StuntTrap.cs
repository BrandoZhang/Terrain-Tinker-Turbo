using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StuntTrap : MonoBehaviour {

    public enum TrapType { None = 0, Switch = 1, Fall = 2, Fan = 3, Revolving = 4, Domino = 5, Swing = 6, Slide = 7 }
    TouchHoldAccelerate tha;
    StuntTrap myTrap;
    public bool trapActivation = false;
    public bool doNotTouch = false;
    //public bool destoryCol = false;
    public TrapType trapType = TrapType.None;
    public float speed = 1;
    

    [Header("[Switch & Revolving & Swing]")]
    public float angle = 0;
    Quaternion originAngle;
    Quaternion targetAngle;

    public float interval = 1;
    private float intervalTimer = 0.0f;


    [Header("[Fall]")]
    public AnimationCurve curve;
    //public float DelayTime = 0.2f;
    public float curvePlayTime = 1.0f;
    //private float delayTimer = 0.0f;
    private float curvePlayTimer = 0.0f;

    [Header("[Slide]")]
    public float slideDist = 0;
    public Vector3 slideOffset;
    Vector3 originPos;
    Vector3 targetPos;


    void Start () {
        if (GameObject.Find("DummyCar") || GameObject.Find("DummyCar").GetComponent<TouchHoldAccelerate>())
        {
            tha = GameObject.Find("DummyCar").GetComponent<TouchHoldAccelerate>();
        }
        myTrap = GetComponent<StuntTrap>();
        originAngle = transform.rotation;
        targetAngle = originAngle * Quaternion.Euler(0, 0, angle);

        originPos = transform.position;
        targetPos = originPos + slideOffset;
    }
	
	void Update () {
        
        if (trapActivation)
        {
            switch (myTrap.trapType)
            {
                case TrapType.None:
                    //None
                    break;
                case TrapType.Switch:
                    SwitchTrap();
                    break;
                case TrapType.Fall:
                    FallTrap();
                    break;
                case TrapType.Fan:
                    FanTrap();
                    break;
                case TrapType.Revolving:
                    RevolvingTrap();
                    break;
                case TrapType.Domino:
                    DominoTrap();
                    break;
                case TrapType.Swing:
                    SwingTrap();
                    break;
                case TrapType.Slide:
                    SlideTrap();
                    break;
                default:
                    Debug.LogError("Unrecognized Option");
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //StuntTrap st = GetComponent<StuntTrap>();
        trapActivation = true;

        if (doNotTouch && collision.gameObject.tag == "Player")
        {
            tha.ReGeneration();
            print("doNotTouch " + doNotTouch + " tag " + collision.gameObject.tag);
        }
        
        if (trapType == TrapType.Domino)
        {
            if (GetComponent<Rigidbody>() && GetComponent<Rigidbody>().isKinematic)
            {
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("other" + other.gameObject.name);
        //StuntTrap st = GetComponent<StuntTrap>();
        //print("st=" + parent.name);
        trapActivation = true;
    }

    public void SwitchTrap()
    {
        //Quaternion targetAngle = originAngle * Quaternion.Euler(0, 0, angle);
        
        //newRotation = Quaternion.LookRotation(Vector3.down * angle);
        //newRotation = Quaternion.LookRotation(transform.up, Vector3.right);
        
        if (transform.rotation == targetAngle)
        {
            trapActivation = false;
        }
        else
        {
            //transform.Rotate(0, 0, 1);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * speed);
        }
    }

    public void FallTrap()
    {
        /*
        /if (delayTimer <= DelayTime)
        {
            delayTimer += Time.deltaTime;
            return;
        }

        if (playTimer <= PlayTime)
        {
        transform.position = Vector3.Lerp(StartPos, EndPos, curve.Evaluate(playTimer / PlayTime));
            transform.position = new Vector3(transform.position.x, transform.position.y - curve.Evaluate(playTimer / PlayTime), transform.position.z);
            playTimer += Time.deltaTime;
        }
        */

        Vector3 tr = transform.position;
        tr.y = tr.y - curve.Evaluate(curvePlayTimer / curvePlayTime);
        transform.position = tr;
        curvePlayTimer += Time.deltaTime;

        if (transform.position.y <= tha.deadHeight)
        {
            //trapMove = false;
            Destroy(gameObject);
        }
    }

    public void RevolvingTrap()
    {
        float currentAngle = Quaternion.Angle(transform.rotation, targetAngle);
        if (currentAngle < 0.5) //stop turn.
        {
            transform.rotation = targetAngle;
            //after interval
            intervalTimer += Time.deltaTime;
            if (intervalTimer > interval)
            {
                intervalTimer = 0;

                //reset angle. TODO:change to localAxis
                if (targetAngle == originAngle)
                {
                    targetAngle = originAngle * Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    targetAngle = originAngle;
                }
            }
        }
        else
        {
            //transform.Rotate(0, 0, 1);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime * speed);
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);   //+speed ccw, -speed cw
            //print("Revolving()else " + transform.eulerAngles + " / " + targetAngle.eulerAngles);
        }
    }

    public void SwingTrap()
    {
        float currentAngle = Quaternion.Angle(transform.rotation, targetAngle);
        
        if (currentAngle < 0.2) //stop turn.
        {
            //after interval
            intervalTimer += Time.deltaTime;
            if (intervalTimer > interval)
            {
                intervalTimer = 0;

                //reset angle.
                if (targetAngle == originAngle)
                {
                    targetAngle = originAngle * Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    targetAngle = originAngle;
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime * speed);
        }
    }

    public void FanTrap()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }

    public void DominoTrap()
    {
        if (GetComponent<Rigidbody>() && !GetComponent<Rigidbody>().isKinematic)
        {
            if (transform.position.y < tha.deadHeight)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SlideTrap()
    {
        float targetDist = Vector3.Distance(transform.position, targetPos);
        
        if (targetDist < 0.2)
        {
            intervalTimer += Time.deltaTime;
            
            if (intervalTimer > interval)
            {
                intervalTimer = 0;
                
                //reset position.
                if (targetPos == originPos)
                {
                    targetPos = originPos + slideOffset;
                }
                else
                {
                    targetPos = originPos;
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}
