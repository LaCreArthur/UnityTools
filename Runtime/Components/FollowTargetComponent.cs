using AS.Toolbox.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.Components
{
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
        bool _initialized;

        float _startX;
        float _startY;
        float _startZ;
        Transform _targetInternal;
        Vector3 _velocity;

        void Start()
        {
            _initialized = false;
            Vector3 position = transform.position;
            _startX = position.x;
            _startY = position.y;
            _startZ = position.z;

            _targetInternal = isSO ? targetSO.v : target;
            SetEnable(onStart);
            if (isSO) targetSO.AddOnChange(UpdateTarget);
        }


        void LateUpdate()
        {
            if (_targetInternal == null)
            {
                enabled = false;
                return;
            }

            Vector3 tarPos = _targetInternal.position;
            if (!isSmoothed)
            {
                transform.position = new Vector3(freezeX ? _startX : tarPos.x + offset.x,
                    freezeY ? _startY : tarPos.y + offset.y,
                    freezeZ ? _startZ : tarPos.z + offset.z);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position,
                    new Vector3(freezeX ? _startX : tarPos.x + offset.x,
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

        public void SetEnable(bool enable)
        {
            enabled = enable;
            if (enabled && keepStartingOffset && !_initialized)
            {
                offset += transform.position - _targetInternal.position;
                _initialized = true;
            }
        }

        void UpdateTarget()
        {
            _targetInternal = isSO ? targetSO.v : target;
            if (onStart && !enabled) SetEnable(true);
        }
    }
}
