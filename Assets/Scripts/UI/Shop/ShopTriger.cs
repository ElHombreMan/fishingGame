using Cinemachine;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public GameObject shopUI;
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
            }
            else if (shopUI.activeSelf)
            {
                shopUI.SetActive(false);

                RestoreCameraState();
                HideCursor();

                PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            shopUI.SetActive(false); // Close shop when leaving

            RestoreCameraState();
            HideCursor();

            PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);

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
