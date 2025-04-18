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

    // Greeting system
    private bool FirstIntroduction = false;
    private const int FirstIntro = 0;
    private const int Intro1 = 1;
    private const int Intro2 = 2;
    private const int Intro3 = 3;

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

                // GREETING DIALOGUE
                ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();
                if (dialogue != null)
                {
                    if (!FirstIntroduction)
                    {
                        dialogue.PlayLine(FirstIntro);
                        FirstIntroduction = true;
                    }
                    else
                    {
                        int randomIntroIndex = Random.Range(Intro2, Intro3 + 1);

                        dialogue.PlayLine(randomIntroIndex);
                    }
                }

                // Optional first-time shop open logic (if still needed)
                if (!ShopStateManager.Current.hasOpenedShop)
                {
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
