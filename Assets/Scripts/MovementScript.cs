using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Rigidbody rb;
    Vector3 upforce;
    Vector3 downforce;
    Vector3 leftforce;
    Vector3 rightforce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        upforce = new Vector3(500.0f, 0.0f, 0.0f);
        downforce = new Vector3(-500.0f, 0.0f, 0.0f);
        leftforce = new Vector3(0.0f, 0.0f, -500.0f);
        rightforce = new Vector3(0.0f, 0.0f, 500.0f);
        Debug.Log("this is the start function!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(upforce);
            Debug.Log("this is the update function!");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForce(leftforce);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(downforce);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForce(rightforce);
        }
        Debug.Log("this is the update function!");
    }
}
