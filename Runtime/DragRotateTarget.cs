using UnityEngine;

public class DragRotateTarget : MonoBehaviour {
    private BoxCollider _collider;
    private Transform _transform;

    public Quaternion Rotation => _transform.rotation;
    public Vector3 Right => _transform.right;
    public Vector3 Up => _transform.up;

    public void SetRotation(Quaternion rotation) {
        _transform.rotation = rotation;
    }

    public bool IsSelfCollider(Collider other) {
        return other == _collider;
    }

    private void Awake() {
        _collider = GetComponent<BoxCollider>();
        _transform = transform;
    }

    private void OnEnable() {
        DragRotateSystem.Instance.AddTarget(this);
    }

    private void OnDisable() {
        DragRotateSystem.Instance.RemoveTarget(this);
    }

    private void Reset() {
        if (!FindObjectOfType<DragRotateSystem>()) {
            var system = new GameObject("DragRotateSystem");
            system.AddComponent<DragRotateSystem>();
        }
    }
}