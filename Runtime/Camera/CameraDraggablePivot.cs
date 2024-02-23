using System;
using BabyCheeseTools.Animations;
using BabyCheeseTools.Animations.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BabyCheeseTools.Camera {
    [RequireComponent(typeof(BoxCollider))]
    public class CameraDraggablePivot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [Serializable]
        private class InertiaData {
            public float maxInertiaDuration = .75f;
            public float minInertiaDuration = .15f;
            public float minInertiaMagnitude = .1f;
            public float maxInertiaMagnitude = 10;
            public Easing inertiaEasing = Easing.OutExpo;
        }

        [SerializeField] private float _dragSpeed = 10;
        [SerializeField] private float _maxDragMagnitude = 15;
        [SerializeField] private InertiaData _inertiaData;
        [SerializeField] private float _dragResetDelay = .5f;
        [SerializeField] private float _positionDragSensitivity = 1;
        [SerializeField] private Transform _cameraTransform;
        private Vector2 _delta;
        private Coroutine _inertiaCoroutine;
        private Vector2 _lastPointerDelta;
        private BoxCollider _collider;
        private Quaternion _rotation;
        private float _lastDragTime;
        private Vector3 _lastMousePos;

        private bool _canRotate = true;

        public event Action DragBegun;
        public event Action DragEnded;

        public void SetEnabled(bool isEnabled) {
            _collider.enabled = isEnabled;
            enabled = isEnabled;
            _rotation = transform.rotation;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!_canRotate) {
                return;
            }

            _delta = Vector2.zero;
            if (_inertiaCoroutine != null) {
                StopCoroutine(_inertiaCoroutine);
            }

            DragBegun?.Invoke();
        }

        public void OnDrag(PointerEventData eventData) {
            if (!_canRotate) {
                return;
            }

            _delta = eventData.delta;
            var magnitude = _delta.magnitude;
            if (magnitude > _maxDragMagnitude) {
                _delta = Vector2.ClampMagnitude(_delta, _maxDragMagnitude);
            }

            RotateBy(_delta);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (Time.time - _lastDragTime < _dragResetDelay) {
                ApplyInertia();
            }

            DragEnded?.Invoke();
        }

        private void ApplyInertia() {
            var deltaMagnitude = _delta.magnitude;
            if (deltaMagnitude < _inertiaData.minInertiaMagnitude) {
                return;
            }

            if (deltaMagnitude > _inertiaData.maxInertiaMagnitude) {
                _delta = Vector2.ClampMagnitude(_delta, _inertiaData.maxInertiaMagnitude);
            }

            var durationMultiplier = deltaMagnitude / _inertiaData.maxInertiaMagnitude;
            var duration = durationMultiplier * _inertiaData.maxInertiaDuration;
            duration = Mathf.Clamp(duration, _inertiaData.minInertiaDuration, _inertiaData.maxInertiaDuration);
            _inertiaCoroutine = StartCoroutine(_delta.To(
                    Vector2.zero,
                    duration,
                    _inertiaData.inertiaEasing,
                    RotateBy
                )
            );
        }

        private void RotateBy(Vector2 delta) {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                _rotation *= Quaternion.Euler(Vector3.up * delta.x);
            } else {
                _rotation *= Quaternion.Euler(Vector3.right * -delta.y);
            }
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                _lastMousePos = Input.mousePosition;
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                _canRotate = false;
                _lastMousePos = Input.mousePosition;
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt)) {
                _canRotate = true;
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)) {
                var mousePos = Input.mousePosition;
                var delta = mousePos - _lastMousePos;
                Vector3 worldDelta = _cameraTransform.TransformDirection(new Vector3(delta.x, delta.y, 0));
                worldDelta *= (_positionDragSensitivity * Time.deltaTime);
                _lastMousePos = mousePos;
                _cameraTransform.position -= worldDelta;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, Time.deltaTime * _dragSpeed);
        }

        private void Awake() {
            _collider = GetComponent<BoxCollider>();
            _rotation = transform.rotation;
        }
    }
}