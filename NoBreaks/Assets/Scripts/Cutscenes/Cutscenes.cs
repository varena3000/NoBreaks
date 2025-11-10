using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Cutscenes : MonoBehaviour
{
    public PlayableDirector cutscene1;
    public GameObject cineCamera;
    public GameObject cineBrain;

    private PlayerSwapManager controller;
    private AdvanceDialogueManager manager;
    private SceneDialogueTrigger hud;

    private bool hasPlayed = false;

    public GameObject staminaCanvas;

    private void Start()
    {
        hud = FindAnyObjectByType<SceneDialogueTrigger>();
        manager = FindAnyObjectByType<AdvanceDialogueManager>();
        controller = FindAnyObjectByType<PlayerSwapManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(PlayCutscene());
        }     
    }

    private IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(1f);

        if (SceneManager.GetActiveScene().name == "2. MortalEngines")
        {
            yield return new WaitUntil( () => manager.DialogueActivated == false);
            controller.activeController.enabled = false;
        }

        cineCamera.SetActive(true);
        cineBrain.SetActive(true);
        cutscene1.Play();
        hud.HudOff();
        manager.audioSource.SetActive(false);
        staminaCanvas.SetActive(false);

        yield return new WaitForSeconds((float) cutscene1.duration);

        cineCamera.SetActive(false);
        cineBrain.SetActive(false);
        controller.activeController.enabled = true;
        manager.audioSource.SetActive(true);
        hud.HudOn();
        staminaCanvas.SetActive(true);
    }
}
