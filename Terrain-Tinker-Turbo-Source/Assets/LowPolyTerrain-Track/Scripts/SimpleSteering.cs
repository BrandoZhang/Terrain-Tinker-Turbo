using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimpleSteering : MonoBehaviour
{
    public bool isGround = false;
    public bool inverseControl = false;
    public bool respawn = false;
    public bool bodyTilt = false;
    Rigidbody rb;
    Vector3 m_EulerAngleVelocity;
    public float steeringAngle = 10;
    public float steeringSpeed = 1;
    public float driftAngle = 45f;
    public float power = 1;
    public float jumpPower = 2;

    public float limitVelocity;
    private float sqrMaxVelocity;

    //test
    public Transform tiltBody;
    public GameObject flipDetector;
    public GameObject skidTrail;
    public Transform frontSkidPos;
    public Transform rearSkidPos;
    List<GameObject> skids = new List<GameObject>();

    //
    public float debugNum = 0;

    //UI
    public Text speedometer;


    void Start()
    {
        if (!respawn)
        {
            respawn = true;
        }
        
        rb = GetComponent<Rigidbody>();
        SetMaxVelocity(limitVelocity);
    }

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            transform.RotateAround(transform.position + transform.forward * -1, -transform.right, 2);
        }

        if (Input.GetKeyUp("space"))
        {
            Vector3 jumpDir = Quaternion.AngleAxis(30, Vector3.forward) * transform.forward;
            rb.AddForce(jumpDir * rb.velocity.magnitude * jumpPower, ForceMode.Impulse);
            //rb.AddForce(transform.forward * rb.velocity.magnitude * jumpPower, ForceMode.Impulse);
        }

        //skidmark
        if (m_EulerAngleVelocity.magnitude >= driftAngle * steeringSpeed)
        {
            if (skids.Count == 0)
            {
                CallSkid();
            }
        }
        else
        {
            for (int i = 0; i < skids.Count; i++)
            {
                if (skids[i] != null)
                {
                    skids[i].transform.SetParent(null);
                }
            }
            skids.Clear();
        }
    }

    void FixedUpdate()
    {
        Vector3 wheelieAngleForce = Quaternion.AngleAxis(debugNum, Vector3.forward) * transform.forward;
        Debug.DrawLine(transform.position, transform.position + wheelieAngleForce, Color.blue);

        //float v = Input.GetAxis("Vertical");  //Activate to control with the keyboard.
        float h;
        if (inverseControl)
        {
            h = Input.GetAxis("Horizontal");
        }
        else
        {
            h = -Input.GetAxis("Horizontal");
        }


        if (tiltBody & bodyTilt)
        {
            tiltBody.transform.localRotation = Quaternion.Euler(0, 0, steeringAngle / 2 * h);
        }

        if (!respawn & isGround)
        {
            m_EulerAngleVelocity = new Vector3(0, steeringAngle * -h * steeringSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            Vector3 addSteeringForce = Quaternion.AngleAxis(-h * steeringAngle, Vector3.up) * transform.forward;
            //Debug.DrawLine(transform.position, transform.position + addSteeringForce, Color.red);
            //print("addSteeringForce " + addSteeringForce);

            rb.AddForce(addSteeringForce * power, ForceMode.Force);
        }
        
        //limit velocity
        var vv = rb.velocity;
        if (vv.sqrMagnitude > sqrMaxVelocity)
        {
            rb.velocity = vv.normalized * limitVelocity;
        }

        float speed = Vector3.Magnitude(rb.velocity);
        //print("speed" + speed);

        
        if (speedometer)
        {
            //speedometer.text = "" + speed.ToString("N2");
            //string result = string.Format("{0:000.##}", speed);
            string result = string.Format("{0:000.00}", speed);
            speedometer.text = result;
        }
    }

    public void SetMaxVelocity(float _limitVelocity)
    {
        this.limitVelocity = _limitVelocity;
        sqrMaxVelocity = limitVelocity * limitVelocity;
    }

    public void CallSkid()
    {
        if (skidTrail == null)
            return;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                skids.Add(Instantiate(skidTrail, frontSkidPos.position, Quaternion.identity));
                skids[i].transform.SetParent(frontSkidPos);
            }
            else
            {
                skids.Add(Instantiate(skidTrail, rearSkidPos.position, Quaternion.identity));
                skids[i].transform.SetParent(rearSkidPos);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGround = true;
        if (respawn)
        {
            respawn = false;
        }

        Collider myCollider = collision.contacts[0].thisCollider;
        if (myCollider.gameObject.name == flipDetector.name)
        {
            if (collision.gameObject.GetComponent<StuntTrap>() && collision.gameObject.GetComponent<StuntTrap>().trapType == StuntTrap.TrapType.Domino)
            {
                
            }
            else
            {
                Vector3 endPos = transform.position;

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                transform.position = endPos + new Vector3(0, 5, 0);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                respawn = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            ParticleSystem ceremony = other.transform.GetChild(0).GetComponent<ParticleSystem>();
            if (!ceremony.isPlaying)
            {
                ceremony.Play();
            }

            power = 0;
        }
    }

    void Wheelie()
    {
        //print("wheelie");
        //Vector3 wheelieAngleForce = Quaternion.AngleAxis(45, Vector3.forward) * transform.forward;

        //Vector3 wheelieAngle = new Vector3(-45, 0, 0);
        //Quaternion deltaRotation = Quaternion.Euler(wheelieAngle * Time.deltaTime);

        //rb.MoveRotation(rb.rotation * deltaRotation);
        //transform.rotation = transform.rotation * deltaRotation;
        //transform.RotateAround(new Vector3(transform.position.x + transform.position.y + 0.4f, transform.position.z - 0.6f), Vector3.forward, 45);
        transform.RotateAround(transform.forward * -0.6f, Vector3.forward, 1);




        Vector3 wheelieAngleForce = Quaternion.AngleAxis(45, Vector3.forward) * transform.forward;
        Debug.DrawLine(transform.position, transform.position + wheelieAngleForce, Color.red);
        //print("addSteeringForce " + addSteeringForce);

        //rb.AddForce(addSteeringForce * power, ForceMode.Force);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
}
