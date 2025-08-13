using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]


    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpHeight;

    [Header("Look")]
    public float MouseSens;

    [SerializeField]
    private CharacterController cc;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private FPSInput input;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        input = new FPSInput();
    }
    void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += ctx =>
        {
            moveDirection = ctx.ReadValue<Vector2>();
            Debug.Log("Player Moved");
        };
    }
    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        float yVel = rb.velocity.y;
        rb.velocity = new Vector3(move.x * walkSpeed, yVel, move.z * walkSpeed);
    }

}
