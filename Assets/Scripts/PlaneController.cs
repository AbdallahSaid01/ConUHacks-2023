using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats.")]
    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle.")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pithching and yarning")]
    public float responsiveness = 10f;
    [Tooltip("Howmust life the plane needs")]
    public float lift = 136f;

    [SerializeField] GameObject inputReciever;
    private UDPReceive inputRecieverComponent;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    public bool isKeyboardControls;

    private float responsiveModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }
    Rigidbody rb; // Object instance
    [SerializeField] TextMeshProUGUI hud;
    AudioSource engineSound;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineSound = GetComponent<AudioSource>();
        inputRecieverComponent = inputReciever.GetComponent<UDPReceive>();
        

        roll = 0.0f;
        pitch = 0.0f;
        yaw = 0.0f;

        isKeyboardControls = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) isKeyboardControls = !isKeyboardControls;

        if (isKeyboardControls)
            HandleKeyboardInputs();
        else if (!isKeyboardControls)
            HandleHandCaptureInput();

        Debug.Log(isKeyboardControls);

        updateHUD();
        engineSound.volume = throttle * 0.01f;
    }

    private void HandleHandCaptureInput()
    {
        //gestures can be: Pointer, Open, Close, Ok
        //movement can be: Clockwise, Counter Clockwise, Move, Stop

        //throtle control
        if (inputRecieverComponent.gestureMovementArr[0].Equals("OK"))
        {
            throttle += throttleIncrement;
        }
        else if (inputRecieverComponent.gestureMovementArr[0].Equals("Close"))
        {
            throttle -= throttleIncrement;
        }
        Debug.Log(inputRecieverComponent.gestureMovementArr[0] + " " + inputRecieverComponent.gestureMovementArr[1]);

        //roll and yaw control
        if(inputRecieverComponent.gestureMovementArr[0].Equals("Pointer") && inputRecieverComponent.gestureMovementArr[1].Equals("Clockwise"))
        {
            roll = 1;
            yaw += 0.1f;
        }
        else if (inputRecieverComponent.gestureMovementArr[0].Equals("Pointer") && inputRecieverComponent.gestureMovementArr[1].Equals("Counter Clockwise"))
        {
            roll = -1;
            yaw -= 0.1f;
        }
        else
        {
            roll = 0.0f;
            yaw = 0.0f;
        }

        //pitch control
        if (inputRecieverComponent.gestureMovementArr[0].Equals("Pointer") && inputRecieverComponent.gestureMovementArr[1].Equals("Move"))
        {
            pitch += 0.1f;
        }
        else if (inputRecieverComponent.gestureMovementArr[0].Equals("Pointer") && inputRecieverComponent.gestureMovementArr[1].Equals("Stop"))
        {
            pitch -= 0.1f;
        }
        else
        {
            pitch = 0.0f;
        }

        //yaw control
        //if (inputRecieverComponent.gestureMovementArr[0].Equals("Ok") && inputRecieverComponent.gestureMovementArr[1].Equals("Move"))
        //{
        //    yaw += 0.1f;
        //}
        //else if (inputRecieverComponent.gestureMovementArr[0].Equals("Ok") && inputRecieverComponent.gestureMovementArr[1].Equals("Stop"))
        //{
        //    yaw -= 0.1f;
        //}
        //else
        //{
        //    yaw = 0.0f;
        //}

        Debug.Log(roll + " " + pitch + " " + yaw);

        roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        pitch = Mathf.Clamp(pitch, -1.0f, 1.0f);
        yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void HandleKeyboardInputs()
    {
        roll = Input.GetAxis("Roll"); // d positive, a negative cap at 1
        pitch = Input.GetAxis("Pitch"); // w positive, s negative cap at 1
        yaw = Input.GetAxis("Yaw"); // e positive, q negative cap at 1

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
    }

    private void FixedUpdate()
    {
        // Anytime we change physics we do it here
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responsiveModifier);
        rb.AddTorque(transform.right * pitch * responsiveModifier);
        rb.AddTorque(-transform.forward * roll * responsiveModifier);
        if (transform.position.y < 20) {
            rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        }
        
    }

    private void updateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0")+"km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
}