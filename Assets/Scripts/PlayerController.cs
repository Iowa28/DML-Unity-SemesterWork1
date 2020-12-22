using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private bool isRunning;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpForce;

    private CharacterController controller;
    private Animator animator;
    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private float rotateSpeed;

    private Vector3 moveDirection;
    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private GameObject playerModel;

    [SerializeField]
    private float knockbackForce;
    [SerializeField]
    private float knockbackTime;
    private float knockbackCounter;

    void Start()
    {
       controller = GetComponent<CharacterController>();
       animator = GetComponent<Animator>();
    }


    void LateUpdate()
    {
        SelectMoveSpeed();
        
        if (knockbackCounter <= 0)
        {
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;
            

            if (Input.GetButtonUp("Jump") && controller.isGrounded)
            {
                moveDirection.y = jumpForce;
            }
        } else
        {
            knockbackCounter -= Time.deltaTime;
        }


        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        
        controller.Move(moveDirection * Time.deltaTime);



        bool isMoving = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        //Move the player in different directions based on camera look direction
        if (isMoving)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }


        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGrounded", controller.isGrounded);
    }

    void SelectMoveSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isRunning)
            {
                isRunning = true;
            } else
            {
                isRunning = false;
            }
            
        }

        if (isRunning)
        {
            moveSpeed = runSpeed;
            animator.SetBool("isRunning", true);
        } else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("isRunning", false);
        }
    }

    public void Knockback(Vector3 direction)
    {
        knockbackCounter = knockbackTime;

        moveDirection = direction * knockbackForce;
        moveDirection.y = knockbackForce;
    }
}
