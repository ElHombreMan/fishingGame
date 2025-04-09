using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public AudioClip jumpClip;

    [Header("Footstep Settings")]
    public float footstepInterval = 0.5f; // Interval between steps
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f); // Random pitch range
    private float footstepDelayBuffer = 0.1f; // Small buffer to ensure no double-play
    private float lastFootstepTime = 0f; // Store the last time a footstep was played

    [Header("References")]
    public PlayerMovement playerMovement;

    private float footstepTimer = 0f;
    private bool hasJumped = false;

    void Update()
    {
        if (playerMovement == null) return;

        bool isGrounded = playerMovement.IsGrounded();
        bool isMoving = IsPlayerMoving();

        // Handle jump sound
        if (!isGrounded && !hasJumped)
        {
            PlayJump();
            hasJumped = true;
        }

        // Handle footsteps
        if (isGrounded)
        {
            hasJumped = false;

            // Check if the player is moving and the interval has passed
            if (isMoving)
            {
                footstepTimer += Time.deltaTime;

                // Only play footstep if enough time has passed since the last one
                if (footstepTimer >= footstepInterval && Time.time - lastFootstepTime >= footstepInterval)
                {
                    PlayFootstep();
                    footstepTimer = 0f; // Reset the timer after playing a footstep
                    lastFootstepTime = Time.time; // Record the last time a footstep was played
                }
            }
            else
            {
                // Reset the timer and stop the last played footstep sound when player stops
                footstepTimer = 0f;
                lastFootstepTime = 0f; // Ensure no footstep is played when the player stops

                // Ensure we stop any lingering footstep sound if player isn't moving
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        // Pick a random footstep sound and randomize the pitch
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip);
        audioSource.pitch = 1f; // Reset pitch to normal after playing the sound
    }

    private void PlayJump()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private bool IsPlayerMoving()
    {
        // Detects if player is pressing any movement key (W, A, S, D)
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }
}
