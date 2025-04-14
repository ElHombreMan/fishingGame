using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Stopping Camera")]
    public Cinemachine.CinemachineFreeLook freeLookCam;

    [Header("UI References")]
    public InfoWindow infoWindow;
    public GameObject inventoryPanel;

    [Header("Audio")]
    public AudioSource openSound;
    public AudioSource closeSound;

    private bool isOpen = false;
    private float storedXSpeed;
    private float storedYSpeed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerState currentState = PlayerStateHandler.Instance.CurrentState;

            if (!isOpen && (currentState == PlayerState.InMiniGame || currentState == PlayerState.InEscapeMenu || currentState == PlayerState.InInventory))
                return;

            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        inventoryPanel.SetActive(isOpen);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;

        // Play sound
        if (isOpen && openSound != null)
        {
            openSound.Play();
        }
        else if (!isOpen && closeSound != null)
        {
            closeSound.Play();
        }

        // Handle camera lock
        if (isOpen)
        {
            storedXSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
            storedYSpeed = freeLookCam.m_YAxis.m_MaxSpeed;

            freeLookCam.m_XAxis.m_MaxSpeed = 0f;
            freeLookCam.m_YAxis.m_MaxSpeed = 0f;
        }
        else
        {
            freeLookCam.m_XAxis.m_MaxSpeed = storedXSpeed;
            freeLookCam.m_YAxis.m_MaxSpeed = storedYSpeed;

            if (infoWindow != null)
                infoWindow.Hide();
        }

        PlayerStateHandler.Instance.ChangeState(
            isOpen ? PlayerState.InInventory : PlayerState.Idle
        );
    }
}
