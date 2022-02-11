using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAligment : MonoBehaviour
{
    public Camera mainCamera;
    private void AlignCamera()
    {
        if (mainCamera != null)
        {
            var camXform = mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    private void Update()
    {
        AlignCamera();
    }
}
