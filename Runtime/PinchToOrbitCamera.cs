using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PinchToOrbitCamera : MonoBehaviour {
    private Camera _cam;
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private float _smoothTime = 7;

    private Transform _targetToOrbit;
    private bool _isOrbiting;
    private Vector3 _currentVelocity;

    void Awake() {
        _cam = GetComponent<Camera>();
    }

    void Update() {
        // For touch devices
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // To check the beginning of the touch
            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began) {
                CheckForHit(touchZero.position);
            }

            if (_targetToOrbit && touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved) {
                Vector2 rotationDelta = touchZero.deltaPosition;
                RotateCamera(rotationDelta);
            }
        }

        // For mouse input
        if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) {
            if (!_isOrbiting) {
                CheckForHit(Input.mousePosition);
            }
        }
        else {
            StopOrbiting();
        }

        if (_isOrbiting) {
            Vector2 rotationDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * _rotationSpeed;
            RotateCamera(rotationDelta);
        }
    }

    void CheckForHit(Vector2 inputPosition) {
        Ray ray = _cam.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            _targetToOrbit = hit.transform;
            _isOrbiting = true;
        }
    }

    void RotateCamera(Vector2 rotationDelta) {
        if (_targetToOrbit) {
            Vector3 desiredRotationAround =
                new Vector3(-rotationDelta.y * _rotationSpeed, rotationDelta.x * _rotationSpeed, 0f);
            _currentVelocity = Vector3.Lerp(_currentVelocity, desiredRotationAround, _smoothTime * Time.deltaTime);

            // Perform rotation around the target's position
            transform.RotateAround(_targetToOrbit.position, Vector3.up, _currentVelocity.y);
            transform.RotateAround(_targetToOrbit.position, Vector3.right, _currentVelocity.x);
        }
    }

    private void StopOrbiting() {
        _isOrbiting = false;
        _targetToOrbit = null;
        _currentVelocity = Vector3.zero;
    }
}