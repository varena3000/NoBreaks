
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Players")]
    public Transform playerOne;
    public Transform playerTwo;

    [Header("Settings")]
    public float followDistance = 2f;
    public float heightOffset = 1.5f;
    public float followSmoothness = 5f;
    public float hoverAmplitude = 0.2f;
    public float hoverSpeed = 2f;

    private Transform currentPlayer;
    private Vector3 velocity;

    private void Start()
    {
        // Choose whichever player starts active
        currentPlayer = playerOne.gameObject.activeSelf ? playerOne : playerTwo;
    }

    private void Update()
    {
        // Check which player is currently active
        if (playerOne.gameObject.activeSelf)
            currentPlayer = playerOne;
        else if (playerTwo.gameObject.activeSelf)
            currentPlayer = playerTwo;

        if (currentPlayer == null) return;

        // Calculate target position in front of the current player
        Vector3 targetPosition = currentPlayer.position + currentPlayer.forward * followDistance + Vector3.up * heightOffset;

        // Add a subtle hover motion
        targetPosition.y += Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        // Smoothly move toward that position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSmoothness);

        // Make the bird look in the player's forward direction
        transform.LookAt(currentPlayer.position + currentPlayer.forward * 2f);

        //Yota's rotations
        Vector3 currentRotation = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
}
