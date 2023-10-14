using Sirenix.OdinInspector;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    public bool customCam;
    [ShowIf("customCam")]
    public Camera cam;
    public Vector3 rotationOffset;
    Camera _camera;

    bool _isActive;

    //Orient the camera after all movement is completed this frame to avoid jitter
    void Start() => _camera = customCam && cam != null ? cam : Camera.main;

    void LateUpdate()
    {
        //if (!_isActive) return;
        Quaternion rotation = _camera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);

        transform.rotation *= Quaternion.Euler(rotationOffset);
    }

    void OnBecameInvisible()
    {
        _isActive = false;
        Debug.Log("became invisible");
    }

    void OnBecameVisible()
    {
        _isActive = true;
        Debug.Log("became visible");
    }
}
