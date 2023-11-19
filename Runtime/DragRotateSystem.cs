using System.Collections.Generic;
using UnityEngine;

public class DragRotateSystem : MonoBehaviourSingleton<DragRotateSystem> {
    [SerializeField] private float _rotationSmoothing = 5f;
    [SerializeField] private float _inertiaSmoothing = 5f;
    [SerializeField] private float _inertiaDuration = 2.0f;
    [SerializeField] private float _maxDragMagnitude = 15;

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

        if (_canDrag && Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) {
            var mouseDelta = Input.mousePosition - _lastMousePos;
            mouseDelta = Vector3.ClampMagnitude(mouseDelta, _maxDragMagnitude);
            _lastMousePos = Input.mousePosition;

            float yaw = -mouseDelta.x;
            float pitch = mouseDelta.y;

            // Apply the rotations to the target rotation
            _targetRotation *= Quaternion.AngleAxis(yaw, Vector3.up);
            _targetRotation *= Quaternion.AngleAxis(pitch, Vector3.right);
            _rotationVelocity = new Vector3(pitch, yaw, 0);
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
}