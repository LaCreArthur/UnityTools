using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Variables;
using UnityEngine;

public class FollowTargetComponent : MonoBehaviour
{
    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;
    public bool isLookingAt;
    public bool isSO;
    [ShowIf("isSO")]
    public TransformVar targetSO;

    [HideIf("isSO")] public Transform target;
    public bool keepStartingOffset;
    public Vector3 offset;

    public bool onStart = true;
    public bool rotateWith;

    public bool isSmoothed;
    [ShowIf("isSmoothed")]
    public float smoothFollowFactor;

    float _startX;
    float _startY;
    float _startZ;
    bool _initialized;
    [ShowInInspector, ReadOnly]
    bool _isActive = true;
    Transform _targetInternal;
    Vector3 _velocity;

    void Start()
    {
        _initialized = false;
        var position = transform.position;
        _startX = position.x;
        _startY = position.y;
        _startZ = position.z;

        _targetInternal = isSO ? targetSO.v : target;
        if (onStart) SetActive(true);
    }


    void LateUpdate()
    {
        if (!_isActive) return;
        if (_targetInternal == null)
        {
            _isActive = false;
            return;
        }
        var tarPos = _targetInternal.position;
        if (!isSmoothed)
        {
            transform.position = new Vector3(
                freezeX ? _startX : tarPos.x + offset.x,
                freezeY ? _startY : tarPos.y + offset.y,
                freezeZ ? _startZ : tarPos.z + offset.z);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                new Vector3(
                    freezeX ? _startX : tarPos.x + offset.x,
                    freezeY ? _startY : tarPos.y + offset.y,
                    freezeZ ? _startZ : tarPos.z + offset.z),
                ref _velocity,
                smoothFollowFactor * Time.deltaTime);
        }

        if (isLookingAt)
            transform.LookAt(_targetInternal);
        if (rotateWith)
            transform.rotation = _targetInternal.rotation;
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        if (_isActive && keepStartingOffset && !_initialized)
        {
            offset += transform.position - _targetInternal.position;
            _initialized = true;
        }
    }
}