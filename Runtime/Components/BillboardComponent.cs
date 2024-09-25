using Sirenix.OdinInspector;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    public bool customCam;
    [ShowIf("customCam")]
    public Camera cam;
    public Vector3 rotationOffset;
    Camera _camera;

    void Start() => _camera = customCam && (cam != null) ? cam : Camera.main;

    //Orient the camera after all movement is completed this frame to avoid jitter
    void LateUpdate()
    {
        var rotation = _camera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);

        transform.rotation *= Quaternion.Euler(rotationOffset);
    }
}
