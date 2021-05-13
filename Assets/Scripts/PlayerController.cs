using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public Transform playerCamera;
    public CharacterController player;

    [Header("Values")]
    public bool lockCursor = true;
    public float mouseSensitivty = 3.5f ;
    public float cameraPitch = 0f;
    public float walkSpeed = 5f;
    public float gravity = -13f;
    [Range(0f, 5f)]public float moveSmoothTime = 0.3f;
    [Range(0f, 5f)]public float mouseSmoothTime = 0.03f;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    float velocityY = 0f;

    void Start()
    {
        player = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {
        MouseLook();
        Movement();
    }

    void MouseLook()
    {
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        Vector2 targetMouseDelta = new Vector2(mX, mY);

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivty;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivty);
    }

    void Movement()
    {
        float mH = Input.GetAxisRaw("Horizontal");
        float mV = Input.GetAxisRaw("Vertical");

        Vector2 targetDirection = new Vector2(mH, mV);
        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        if (player.isGrounded)
        {
            velocityY = 0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up * velocityY;

        player.Move(velocity * Time.deltaTime);
    }
}
