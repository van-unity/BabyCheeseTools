using UnityEngine;

namespace BabyCheeseTools.Camera {
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraScrollZoom : MonoBehaviour {
        [SerializeField] private float _sensitivity = 10f; // How sensitive the zoom is to scroll wheel movement.
        [SerializeField] private float _minFOV = 15f; // The minimum FOV value.
        [SerializeField] private float _maxFOV = 90f; // The maximum FOV value.
        [SerializeField] private float _smoothness = 10f; // Smoothness of the zoom for lerping.
        [SerializeField] private float _pinchSensitivity = 0.1f; // Sensitivity for pinch zooming.

        private UnityEngine.Camera _camera;
        private float _targetFOV; // The target FOV to lerp towards.
        private float _initialPinchDistance;
        private bool _isPinching = false;

        private void Start() {
            _camera = GetComponent<UnityEngine.Camera>();
            _targetFOV = _camera.fieldOfView;
        }

        private void Update() {
            // Desktop zoom using the mouse scroll wheel
            if (Input.mousePresent) {
                var scroll = Input.GetAxis("Mouse ScrollWheel");
                AdjustFOV(scroll * _sensitivity);
            }

            // Mobile zoom using pinch
            if (Input.touchCount == 2) {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (!_isPinching) {
                    _initialPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    _isPinching = true;
                } else {
                    var currentPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    var pinchDelta = currentPinchDistance - _initialPinchDistance;

                    // Adjust FOV based on the pinch delta
                    AdjustFOV(pinchDelta * _pinchSensitivity);

                    // Update the initial pinch distance for the next frame
                    _initialPinchDistance = currentPinchDistance;
                }
            } else {
                _isPinching = false;
            }

            // Smoothly interpolate the camera's FOV towards the target FOV
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, _smoothness * Time.deltaTime);
        }

        private void AdjustFOV(float delta) {
            _targetFOV -= delta;
            _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);
        }
    }
}