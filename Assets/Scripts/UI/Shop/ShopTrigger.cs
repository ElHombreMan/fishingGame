using Cinemachine;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("References")]
    public CinemachineFreeLook freeLookCam;
    public GameObject shopUI;
    public GameObject interactionButton;

    [Header("Audio")]
    public AudioSource shopBell;
    public AudioSource KeyPressed;

    private bool isPlayerNear = false;
    private float storedXMaxSpeed, storedYMaxSpeed;

    private void Start()
    {
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!shopUI.activeSelf)
            {
                PlayerStateHandler.Instance.ChangeState(PlayerState.InShop);
                
                KeyPressed.Play();
                StoreCameraState();
                LockCamera();
                ShowCursor();
                
                shopBell.Play();
                shopUI.SetActive(true);
                interactionButton.SetActive(false);

                // First time shop dialogue
                if (!ShopStateManager.Current.hasOpenedShop)
                {
                    ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();
                    if (dialogue != null)
                    {
                        dialogue.PlayLine(0);
                    }

                    ShopStateManager.Current.hasOpenedShop = true;
                    ShopStateManager.Save();
                }
            }
            else if (shopUI.activeSelf)
            {
                ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();
                if (dialogue != null)
                    dialogue.StopLine();

                ShopExit();
                KeyPressed.Play();
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

        // This remains just in case it’s called from other places too
        ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();
        if (dialogue != null)
            dialogue.StopLine();
    }

    void StoreCameraState()
    {
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
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
