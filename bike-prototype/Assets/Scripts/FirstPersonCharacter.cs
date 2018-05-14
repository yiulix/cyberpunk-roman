using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class FirstPersonCharacter : MonoBehaviour {

    //settings
    public float JumpForce;
    public float RotateSpeed; // angle per fixed frame
    public float Acceleration;


    //components
    public RPMController rpmController;
    private Rigidbody mRigidBody;

    //states
    private bool isJumping = false;
    private bool isRotating = false;
    private int RotateDirection; // -1 for left, 1 for right
    private float rstamp = 0;
    private float DragForce;


    private float frequence;

	// Use this for initialization
	void Start () {
        mRigidBody = GetComponent<Rigidbody>();
        DragForce = mRigidBody.drag;
    }

    // Update is called once per frame
    void Update () {
        frequence = rpmController.RPM();
        mRigidBody.AddForce(Acceleration * frequence * transform.forward / 1000);
        //inputs
        if (Input.GetKey(KeyCode.O) && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isRotating = true;
            RotateDirection = -1;
            rstamp = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            isRotating = true;
            RotateDirection = 1;
            rstamp = Time.time;
        }
    }

    private void FixedUpdate()
    {
        //mRigidBody.AddForce(Acceleration * transform.forward);
        if (isRotating)
        {
            float RotateTime = 90 / RotateSpeed;
            if (Time.time - rstamp < RotateTime)
            {
                
                Quaternion q = Quaternion.AngleAxis(RotateSpeed * RotateDirection*Time.deltaTime, transform.up) * transform.rotation;
                mRigidBody.MoveRotation(q);
                float mag = mRigidBody.velocity.magnitude;
                mRigidBody.velocity = transform.forward * mag * 0.995f;
                
            }
            else
            {
                isRotating = false;
                float error = transform.localEulerAngles.y % 90;
                if (Mathf.Abs(error) > 45)
                {
                    error = error - 90;
                }
                Quaternion adjust = Quaternion.Euler(new Vector3(0, -error, 0));
                mRigidBody.MoveRotation(mRigidBody.rotation * adjust);
            }
        }
    }

    // If collide something while jumping, then consider it lands.
    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping)
        {
            isJumping = false;
            mRigidBody.drag = DragForce;
        }
    }


    void Turn ()
    {

    }

    void Jump ()
    {
        mRigidBody.drag = 0f;
        mRigidBody.velocity = new Vector3(mRigidBody.velocity.x, 0f, mRigidBody.velocity.z);
        mRigidBody.AddForce(new Vector3(0f, JumpForce, 0f), ForceMode.Impulse);
    }

    void Dive ()
    {

    }
}
