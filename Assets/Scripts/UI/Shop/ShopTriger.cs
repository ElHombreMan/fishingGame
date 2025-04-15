using Cinemachine;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public GameObject shopUI;
    public GameObject interactionButton;
    private bool isPlayerNear = false;
    private float storedXMaxSpeed, storedYMaxSpeed, storedXValue, storedYValue;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!shopUI.activeSelf)
            {
                PlayerStateHandler.Instance.ChangeState(PlayerState.InShop);

                StoreCameraState();
                LockCamera();
                ShowCursor();

                shopUI.SetActive(true);
                interactionButton.SetActive(false);
            }
            else if (shopUI.activeSelf)
            {
                ShopExit();
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactionButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionButton.SetActive(false);
            ShopExit();
        }
    }

    public void ShopExit()
    {
        shopUI.SetActive(false); // Close shop when leaving

        RestoreCameraState();
        HideCursor();

        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);

        if (isPlayerNear)
        {
            interactionButton.SetActive(true);
        }
    }

    void StoreCameraState()
    {
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
        storedXValue = freeLookCam.m_XAxis.Value;
        storedYValue = freeLookCam.m_YAxis.Value;
    }

    void LockCamera()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        freeLookCam.m_YAxis.m_MaxSpeed = 0f;
    }

    void RestoreCameraState()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = storedXMaxSpeed;
        freeLookCam.m_YAxis.m_MaxSpeed = storedYMaxSpeed;
    }

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
