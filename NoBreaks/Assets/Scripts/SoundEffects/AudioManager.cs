using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("--- Audio  Clip ---")]
    public AudioClip background;
    public AudioClip Menu;
    public AudioClip menuButton;
    public AudioClip Interact;
    public AudioClip jenniImbue;
    public AudioClip mochiImbue;
    public AudioClip Walk;
    public AudioClip Sprint;
    public AudioClip Jump;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX(AudioClip clip)
    {
        sfxSource.Stop();
    }
}
