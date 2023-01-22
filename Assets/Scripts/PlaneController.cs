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

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

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
    }
    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if(Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();
        updateHUD();
        engineSound.volume = throttle * 0.01f;
    }

    private void FixedUpdate()
    {
        // Anytime we change physics we do it here
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responsiveModifier);
        rb.AddTorque(transform.right * pitch * responsiveModifier);
        rb.AddTorque(-transform.forward * roll * responsiveModifier);

        rb.AddForce(Vector3.up*rb.velocity.magnitude*lift);
    }

    private void updateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0")+"km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
}