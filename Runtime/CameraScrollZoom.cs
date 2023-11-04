using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScrollZoom : MonoBehaviour {
    [SerializeField] float _sensitivity = 10f; // How sensitive the zoom is to scroll wheel movement.
    [SerializeField] float _minFOV = 15f; // The minimum FOV value.
    [SerializeField] float _maxFOV = 90f; // The maximum FOV value.
    [SerializeField] float _smoothness = 10f; // Smoothness of the zoom for lerping.

    private Camera _camera;
    private float _targetFOV; // The target FOV to lerp towards.

    private void Start() {
        _camera = GetComponent<Camera>();
        _targetFOV = _camera.fieldOfView;
    }

    private void Update() {
        var scroll = Input.GetAxis("Mouse ScrollWheel");

        _targetFOV -= scroll * _sensitivity;
        _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, _smoothness * Time.deltaTime);
    }
}