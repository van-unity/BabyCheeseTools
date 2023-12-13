using System.Collections.Generic;
using UnityEngine;

namespace BabyCheeseTools {
    public class DragRotateSystem : MonoBehaviourSingleton<DragRotateSystem> {
        private enum RotationViewPoint {
            Local,
            World,
            Camera
        }

        [SerializeField] private float _rotationSmoothing = 5f;
        [SerializeField] private float _dragSpeed = 15f;
        [SerializeField] private float _inertiaSmoothing = 5f;
        [SerializeField] private float _inertiaDuration = 2.0f;
        [SerializeField] private float _maxDragMagnitude = 15;
        [SerializeField] private float _minMagnitude = .1f;
        [SerializeField] private RotationViewPoint _yawViewPoint = RotationViewPoint.World;
        [SerializeField] private RotationViewPoint _pitchViewPoint = RotationViewPoint.World;
        private Camera _camera;
        private bool _canDrag;
        private Vector3 _lastMousePos;
        private Quaternion _targetRotation;
        private Vector3 _rotationVelocity;
        private float _inertiaTimeRemaining;

        private List<DragRotateTarget> _targets;
        private DragRotateTarget _currentTarget;
        private float _removeTargetTimer;

        public void AddTarget(DragRotateTarget target) {
            if (_targets.Contains(target) || target == null) {
                return;
            }

            _targets.Add(target);
        }

        public void RemoveTarget(DragRotateTarget target) {
            if (target == null || !_targets.Contains(target)) {
                return;
            }

            _targets.Remove(target);
        }

        protected override void Awake() {
            base.Awake();
            _targets = new List<DragRotateTarget>();
            _camera = Camera.main;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo)) {
                    foreach (var target in _targets) {
                        if (target.IsSelfCollider(hitInfo.collider)) {
                            _currentTarget = target;
                            _targetRotation = _currentTarget.Rotation;
                            _lastMousePos = Input.mousePosition;
                            _canDrag = true;
                            _inertiaTimeRemaining = 0; // Stop inertia when starting to drag
                            _removeTargetTimer = 0;
                            break;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                _canDrag = false;
                _inertiaTimeRemaining = _inertiaDuration; // Start inertia
            }

            if (_canDrag && Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftAlt) &&
                !Input.GetKey(KeyCode.RightAlt)) {
                var mouseDelta = (Input.mousePosition - _lastMousePos) * (Time.deltaTime * _dragSpeed);
                var magnitude = mouseDelta.magnitude;
                if (magnitude > _minMagnitude) {
                    mouseDelta = Vector3.ClampMagnitude(mouseDelta, _maxDragMagnitude);

                    float yaw = -mouseDelta.x;
                    float pitch = -mouseDelta.y;

                    // Apply the rotations to the target rotation
                    if (Mathf.Abs(yaw) > Mathf.Abs(pitch)) {
                        _targetRotation *= Quaternion.AngleAxis(yaw, GetViewPointYawAxis());
                    }
                    else {
                        _targetRotation *= Quaternion.AngleAxis(pitch, GetViewPointPitchAxis());
                    }

                    _rotationVelocity = new Vector3(pitch, yaw, 0);
                }
                else {
                    _rotationVelocity = Vector3.zero;
                }
                
                _lastMousePos = Input.mousePosition;
            }
            else if (_inertiaTimeRemaining > 0) {
                // Apply the inertia
                _inertiaTimeRemaining -= Time.deltaTime * _inertiaSmoothing;
                var t = 1f - (_inertiaTimeRemaining / _inertiaDuration); // Normalized time
                var deltaRotation = Vector3.Lerp(_rotationVelocity, Vector3.zero, t);
                _targetRotation *= Quaternion.Euler(deltaRotation);
            }
            else if (_currentTarget) {
                _rotationVelocity = Vector3.zero;
                _removeTargetTimer += Time.deltaTime;
                if (_removeTargetTimer >= _inertiaDuration / _inertiaSmoothing) {
                    _currentTarget = null;
                }
            }

            // Slerp smoothly towards the target rotation
            if (_currentTarget) {
                var rotation = Quaternion.Slerp(_currentTarget.Rotation, _targetRotation,
                    Time.deltaTime * _rotationSmoothing);
                _currentTarget.SetRotation(rotation);
            }
        }

        private Vector3 GetViewPointYawAxis() {
            switch (_yawViewPoint) {
                case RotationViewPoint.Local:
                    return _currentTarget.Up;
                default:
                case RotationViewPoint.World:
                    return Vector3.up;
                case RotationViewPoint.Camera:
                    return _camera.transform.up;
            }
        }

        private Vector3 GetViewPointPitchAxis() {
            switch (_pitchViewPoint) {
                case RotationViewPoint.Local:
                    return _currentTarget.Right;
                default:
                case RotationViewPoint.World:
                    return Vector3.right;
                case RotationViewPoint.Camera:
                    return _camera.transform.right;
            }
        }
    }
}