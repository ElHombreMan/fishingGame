using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    float rotationSpeed = GlobalSettings.sensitivity;

    void Start()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
       
    }

    void Update()
    {
        if (PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu &&
            PlayerStateHandler.Instance.CurrentState != PlayerState.InMiniGame &&
            PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory)
        {
            // Calculate viewDir
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

            // If distance is tiny, skip
            // Otherwise, set orientation
            if (viewDir.sqrMagnitude > 0.0001f)
            {
                orientation.forward = viewDir.normalized;
            }

            // rotate player object
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero && PlayerStateHandler.Instance.CurrentState != PlayerState.RodCharging)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
