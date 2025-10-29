using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] activeParticle;
    [SerializeField]
    private GameObject[] inactiveParticle;

    public AudioSource source;
    public AudioClip areYouReady;

    private void OnTriggerEnter(Collider other)
    {
        foreach(GameObject obj in activeParticle)
        {
           obj.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach(GameObject obj in inactiveParticle)
        {
            obj.SetActive(false);
        }

        source.clip = areYouReady;
        source.Play();
            
    }
}
