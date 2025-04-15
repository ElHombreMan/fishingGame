using UnityEngine;

public class RodSoundController : MonoBehaviour
{
    [Header("Rod Sounds")]
    public AudioSource chargingAudioSource;
    public AudioSource castAudioSource;
    public AudioSource pullBackAudioSource; // ← New!

    public void StartChargingRod()
    {
        if (chargingAudioSource && !chargingAudioSource.isPlaying)
        {
            chargingAudioSource.loop = false;
            chargingAudioSource.Play();
        }
    }

    public void StopChargingRod()
    {
        if (chargingAudioSource && chargingAudioSource.isPlaying)
        {
            chargingAudioSource.Stop();
        }
    }

    public void PlayCastSound()
    {
        if (castAudioSource)
        {
            castAudioSource.Play();
        }
    }

    public void PlayPullBackSound()
    {
        if (pullBackAudioSource)
        {
            pullBackAudioSource.Play();
        }
    }
}
