using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    [SerializeField] private float dashAmount = 20f;
    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f; 
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashing = true;
        }*/
    }

    private void FixedUpdate()
    {
        bool isIdle = moveDir == Vector3.zero;

        if (isIdle)
        {
            // Idle
            animator.SetBool("isMoving", false);
        }
        else
        {
            // is moving
            rb.AddForce(moveDir * moveSpeed);
            animator.SetFloat("xMovement", moveDir.x);
            animator.SetFloat("yMovement", moveDir.y);
            animator.SetBool("isMoving", true);
        }

        /*if (isDashing)
        {
            Vector3 dashPosition = transform.position + moveDir * dashAmount;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, dashAmount, dashLayerMash);
            if (raycastHit2D.collider != null)
            {
                dashPosition = raycastHit2D.point;
            }
            rigidbody2D.MovePosition(dashPosition);
            isDashing = false;
        }*/
    }
}
