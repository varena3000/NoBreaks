using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] public AudioSource actionSource;
    [SerializeField] public AudioSource playerSource;
    [SerializeField] public AudioSource menuSource;
    [SerializeField] public AudioSource imbueSource;

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
    public AudioClip Crouch;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    //SFX
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX(AudioClip clip)
    {
        sfxSource.Stop();
    }

    //Actions
    public void PlayActionSFX(AudioClip clip)
    {
        actionSource.PlayOneShot(clip);
    }

    public void StopActionSFX(AudioClip clip)
    {
        actionSource.Stop();
    }

    //Player
    public void PlayPlayerSFX(AudioClip clip)
    {
        playerSource.PlayOneShot(clip);
    }

    public void StopPlayerSFX(AudioClip clip)
    {
        playerSource.Stop();
    }

    //Menu
    public void PlayMenuSFX(AudioClip clip)
    {
        menuSource.PlayOneShot(clip);
    }

    public void StopMenuSFX(AudioClip clip)
    {
        menuSource.Stop();
    }

    //Imbue
    public void PlayImbueSFX(AudioClip clip)
    {
        imbueSource.PlayOneShot(clip);
    }

    public void StopImbueSFX(AudioClip clip)
    {
        imbueSource.Stop();
    }
}
