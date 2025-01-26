using Sirenix.OdinInspector;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    public bool useCustomCam;
    [ShowIf("useCustomCam")]
    public Camera customCam;
    public Vector3 rotationOffset;
    Camera _camera;
    Camera Cam => _camera ??= useCustomCam && customCam != null ? customCam : Camera.main;

    //Orient the camera after all movement is completed this frame to avoid jitter
    void LateUpdate()
    {
        Quaternion rotation = Cam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);

        transform.rotation *= Quaternion.Euler(rotationOffset);
    }
}
