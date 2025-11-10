using System.Collections;
using UnityEngine;

public class npcFocus : MonoBehaviour
{
    public Transform npc;  // The NPC you want to face
    public Transform playerCamera; // Your camera
    public float rotateSpeed = 5f;

    public void FocusOnNPC()
    {
        StartCoroutine(FaceNPC());
    }

    private IEnumerator FaceNPC()
    {
        Vector3 direction = (npc.position - playerCamera.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the camera toward the NPC
        while (Quaternion.Angle(playerCamera.rotation, targetRotation) > 0.5f)
        {
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }
}
