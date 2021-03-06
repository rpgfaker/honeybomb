﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 5f;
    public float sensitivity = 3f;
    public Camera cam;
    Rigidbody rb;
    Vector3 velocity;
    Vector3 rotation;
    Vector3 cameraRotation;

    //Gun
    public GameObject PHitbox;
    int cooldown = 0;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {

        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector3 xPos = transform.right * xMovement;
        Vector3 yPos = transform.forward * yMovement;

        velocity = (xPos + yPos).normalized * speed;

        float xRotation = Input.GetAxisRaw("Mouse Y");
        float yRotation = Input.GetAxisRaw("Mouse X");

        // vector for turning around x axis
        rotation = new Vector3(0, yRotation, 0) * sensitivity;

        // vector for turning camera around y axis
        cameraRotation = new Vector3(xRotation, 0, 0) * sensitivity;

        //Shoot
        bool shoot = Input.GetMouseButtonDown(0);
        if (shoot && cooldown == 0)
        {
            PHitbox.SetActive(true);
            cooldown = 120;
            Debug.Log("Poof!");
        }
        if(cooldown > 0)
        {
            if(PHitbox.active && cooldown < 60)
            {
                PHitbox.SetActive(false);
                Debug.Log("Poof Deactived");
            }
            cooldown--;
            if(cooldown == 0)
            {
                Debug.Log("Poof Refreshed");
            }
        }
	}

    private void FixedUpdate()
    {
        playerMove();
        playerRotate();
        cameraRotate();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Boundary")
        {
            velocity = new Vector3(0, 0, 0);
            Debug.Log("collided");
        }

        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("You lost! A bee got you.");
        }
    }

    void playerMove ()
    {
        if (velocity != new Vector3(0,0,0))
        {
            
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
    }

    void playerRotate()
    {
        if (rotation != new Vector3(0,0,0))
        {
            rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        }

    }

    void cameraRotate()
    {
        if (cameraRotation != new Vector3(0,0,0))
        {
            cam.transform.Rotate(cameraRotation * -1);
        }
    }

    
}
