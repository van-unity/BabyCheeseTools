using BabyCheeseTools.Extensions;
using UnityEngine;

namespace BabyCheeseTools {
    public class DragRotateTarget : MonoBehaviour {
        private Collider[] _collider;
        private Transform _transform;

        public Quaternion Rotation => _transform.rotation;
        public Vector3 Right => _transform.right;
        public Vector3 Up => _transform.up;

        public void SetRotation(Quaternion rotation) {
            _transform.rotation = rotation;
        }

        public bool IsSelfCollider(Collider other) {
            foreach (var col in _collider) {
                if (other == col) {
                    return true;
                }
            }

            return false;
        }

        private void Awake() {
            _collider = GetComponentsInChildren<Collider>();
            _transform = transform;
        }

        private void OnEnable() {
            //just to be sure that awake was called on DragRotateSystem
            this.WaitForFramesAndExecute(3, () => { DragRotateSystem.Instance.AddTarget(this); });
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
}