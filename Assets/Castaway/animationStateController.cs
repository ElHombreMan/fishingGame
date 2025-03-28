using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning; 
    private bool isJumping;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Checks if any of the movement keys are being pressed -Ed
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        //Checks if space bar is being pressed -Ed
        isJumping = Input.GetKey(KeyCode.Space);
        
        // Updates the Animator parameter -Ed
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

}