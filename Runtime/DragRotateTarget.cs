using UnityEngine;

namespace Packages.com.babycheese.tools {
    [RequireComponent(typeof(BoxCollider))]
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

        private void AdjustBoxColliderSize() {
            if (_collider == null) {
                _collider = GetComponent<BoxCollider>();
            }

            if (_transform == null) {
                _transform = transform;
            }

            Bounds combinedBounds = new Bounds(_transform.position, Vector3.zero);
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in childRenderers) {
                combinedBounds.Encapsulate(meshRenderer.bounds);
            }

            _collider.center = _transform.InverseTransformPoint(combinedBounds.center);
            _collider.size = combinedBounds.size;
        }

        private void OnDisable() {
            DragRotateSystem.Instance.RemoveTarget(this);
        }

        private void OnValidate() {
            if (Application.isPlaying) {
                return;
            }

            AdjustBoxColliderSize();
            _collider.isTrigger = true;
        }
    }
}