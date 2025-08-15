using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    public Camera cam;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpHeight;

    [Header("Look")]
    public float mouseSens;

    private Rigidbody rb;
    private FPSInput input;
    private Vector2 moveDirection;
    private Vector2 mouseDelta;
    private bool sprint = false;
    private float pitch;
    private float groundCheckDistance = 1.2f;
    [SerializeField] private LayerMask groundMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new FPSInput();
    }
    void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += ctx =>
        {
            moveDirection = ctx.ReadValue<Vector2>();
        };
        input.Player.Sprint.performed += ctx =>
        {
            sprint = true;
        };
        input.Player.Sprint.canceled += ctx =>
        {
            sprint = false;
        };
        input.Player.Jump.performed += ctx =>
        {
            jumpHandler();
        };
        input.Player.Fire.performed += ctx =>
        {
            shootHandler();
        };
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    private void Update()
    {
        mouseLookHandler();
    }

    private void FixedUpdate()
    {
        moveHandler();
    }

    void mouseLookHandler() 
    {
        mouseDelta = input.Player.Look.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * mouseSens * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSens * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void moveHandler() 
    {
        if (!sprint)
        {
            Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            float yVel = rb.velocity.y;
            rb.velocity = new Vector3(move.x * walkSpeed, yVel, move.z * walkSpeed);
        }
        else
        {
            Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            float yVel = rb.velocity.y;
            rb.velocity = new Vector3(move.x * sprintSpeed, yVel, move.z * sprintSpeed);
        }
    }

    void jumpHandler() 
    {
        if (onGround()) { 
            float xVel = rb.velocity.x;
            float zVel = rb.velocity.z;
            rb.velocity = new Vector3(xVel, jumpHeight, zVel);
        }
    }

    void shootHandler() 
    {
        Debug.Log("Shot");
    }
    public bool onGround() 
    {
        bool grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundMask);
        Debug.Log(grounded);
        return grounded;
    }



}
