using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float CameraSpeed;

    public float minX, maxX;
    public float minY, maxY;
    void Start()
    {

    }


    void Update()
    {

    }
    void LateUpdate()
    {
        if (target != null)
        {
            Vector2 newCamPosition = Vector2.Lerp(transform.position, target.position, Time.deltaTime * CameraSpeed);

            float ClampX = Mathf.Clamp(newCamPosition.x, minX, maxX);
            float ClampY = Mathf.Clamp(newCamPosition.y, minY, maxY);

            transform.position = new Vector3(ClampX, ClampY, -10f);
        }

    }
}
