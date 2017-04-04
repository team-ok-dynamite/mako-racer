using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraAccelerationX;
    public float cameraAccelerationY;

    private const float EPSILON = .0001f;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        rotateCameraOffsetHorizontal();
        rotateCameraOffsetVertical();

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }

    private void rotateCameraOffsetHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mouseX = -mouseX;

        float playerX = player.transform.position.x;
        float xDiff = offset.x - playerX;

        float playerZ = player.transform.position.z;
        float zDiff = offset.z - playerZ;

        if (Mathf.Abs(mouseX) > EPSILON)
        {
            mouseX *= cameraAccelerationX;
            float newX = (xDiff * Mathf.Cos(mouseX) - zDiff * Mathf.Sin(mouseX)) + playerX;
            float newZ = (xDiff * Mathf.Sin(mouseX) + zDiff * Mathf.Cos(mouseX)) + playerZ;
            offset.x = newX;
            offset.z = newZ;
        }
    }

    private void rotateCameraOffsetVertical()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        if (Mathf.Abs(mouseY) > EPSILON)
        {
            offset.y -= mouseY * cameraAccelerationY;
            if (offset.y < EPSILON)
            {
                offset.y = EPSILON;
            }
        }
    }
}
