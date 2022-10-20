using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviourPun
{
    CharacterController controller;
    //[SerializeField] Transform cam;

    [SerializeField] private float speed = 6f;

    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Vector3 velocity;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    
    private Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        (Cursor.lockState, Cursor.visible) = (CursorLockMode.None, true);
        //animator = GetComponent<Animator>();

        if (photonView != null) {
            if (photonView.IsMine) {
                var vrCam = FindObjectOfType<CinemachineFreeLook>();
                (vrCam.Follow, vrCam.LookAt) = (transform, transform);
            } else {
                if (controller != null) controller.enabled = false;
            } 
        }
    }

    void Update()
    {
        if (photonView == null || photonView.IsMine == false) return;

        if (Chatting.Instance != null 
            && (Chatting.Instance.isDone == false 
            || Chatting.Instance.IsFocused == true)) return;

        //animator.SetBool("bIsJumping", false);
        //Check if it's on Ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //If it's on Ground, resetting velocity from infinite speed 
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Getting Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Due to Physics of a Free Fall, mulitplying Time.deltaTime
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //animator.SetBool("bIsJumping", true);
        }

        //Movement & Change Direction of Camera
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
    }
}
