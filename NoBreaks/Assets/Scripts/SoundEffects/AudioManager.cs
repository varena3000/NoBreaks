using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--- Audio  Clip ---")]
    public AudioClip background;
    public AudioClip Menu;
    public AudioClip menuButton;
    public AudioClip Interact;
    public AudioClip Imbue;
    public AudioClip Walk;
    public AudioClip Sprint;
    public AudioClip Jump;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
