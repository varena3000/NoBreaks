using UnityEngine;

public class RotateCollectible : MonoBehaviour
{
    [Tooltip("Rotation speed around X axis (degrees per second)")]
    public float rotationSpeedX = 0f;

    [Tooltip("Rotation speed around Y axis (degrees per second)")]
    public float rotationSpeedY = 180f;

    [Tooltip("Rotation speed around Z axis (degrees per second)")]
    public float rotationSpeedZ = 0f;

    void Update()
    {
        Vector3 rotation = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime;
        transform.Rotate(rotation);
    }
}