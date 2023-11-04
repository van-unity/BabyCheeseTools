using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public abstract class ColliderAdjusterStrategyBase {
        public void AdjustCollider(GameObject target) {
            var transform = target.transform;
            var collider = target.GetComponent<Collider>();
            if (!collider) {
                collider = CreateCollider(target);
            }

            var bounds = CalculateMeshBounds(transform);

            ApplyBounds(collider, bounds);
        }

        private Bounds CalculateMeshBounds(Transform transform) {
            return CalculateOOBB(transform);
        }


        //calculating Object-Oriented Bounding Box to respect objects rotation 
        private Bounds CalculateOOBB(Transform transform) {
            // Start with an empty bounds object
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            bool hasBounds = false;

            // Get all MeshFilters in this object and its children
            MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters) {
                if (hasBounds) {
                    bounds.Encapsulate(CalculateLocalBounds(meshFilter));
                }
                else {
                    bounds = CalculateLocalBounds(meshFilter);
                    hasBounds = true;
                }
            }

            return bounds;
        }

        private Bounds CalculateLocalBounds(MeshFilter meshFilter) {
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            if (vertices.Length == 0) return new Bounds();

            // Transform vertices to local space
            Transform transform = meshFilter.transform;
            vertices = Array.ConvertAll(vertices, v => transform.TransformPoint(v));

            // Calculate local bounds
            Vector3 min = transform.InverseTransformPoint(vertices[0]);
            Vector3 max = min;

            foreach (Vector3 vertex in vertices) {
                min = Vector3.Min(min, transform.InverseTransformPoint(vertex));
                max = Vector3.Max(max, transform.InverseTransformPoint(vertex));
            }

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }


        protected abstract Collider CreateCollider(GameObject target);


        protected abstract void ApplyBounds(Collider collider, Bounds bounds);
    }

    public class BoxColliderAdjuster : ColliderAdjusterStrategyBase {
        protected override Collider CreateCollider(GameObject target) {
            return target.AddComponent<BoxCollider>();
        }

        protected override void ApplyBounds(Collider collider, Bounds bounds) {
            if (collider is BoxCollider boxCollider) {
                boxCollider.center = bounds.center;
                boxCollider.size = bounds.size;
            }
        }
    }

    public class SphereColliderAdjuster : ColliderAdjusterStrategyBase {
        protected override Collider CreateCollider(GameObject target) {
            return target.AddComponent<SphereCollider>();
        }

        protected override void ApplyBounds(Collider collider, Bounds bounds) {
            if (collider is SphereCollider sphereCollider) {
                sphereCollider.center = bounds.center;
                // For sphere collider, we set the radius to the largest extent of the bounds divided by 2
                sphereCollider.radius = Mathf.Max(Mathf.Max(bounds.extents.x, bounds.extents.y),
                    bounds.extents.z);
            }
        }
    }

    public class CapsuleColliderAdjuster : ColliderAdjusterStrategyBase {
        protected override Collider CreateCollider(GameObject target) {
            return target.AddComponent<CapsuleCollider>();
        }

        protected override void ApplyBounds(Collider collider, Bounds bounds) {
            if (collider is CapsuleCollider capsuleCollider) {
                capsuleCollider.center = bounds.center;

                // Determine the primary axis based on the largest extent
                int primaryAxis = DeterminePrimaryAxis(bounds.extents);

                // Set the capsule collider's direction
                capsuleCollider.direction = primaryAxis; // 0 = x-axis, 1 = y-axis, 2 = z-axis

                // The radius should be the average of the two smaller extents
                // The height should be the largest extent
                if (primaryAxis == 0) {
                    // X-axis is the primary axis
                    capsuleCollider.radius = Mathf.Max(bounds.extents.y, bounds.extents.z);
                    capsuleCollider.height = bounds.size.x;
                }
                else if (primaryAxis == 1) {
                    // Y-axis is the primary axis
                    capsuleCollider.radius = Mathf.Max(bounds.extents.x, bounds.extents.z);
                    capsuleCollider.height = bounds.size.y;
                }
                else {
                    // Z-axis is the primary axis
                    capsuleCollider.radius = Mathf.Max(bounds.extents.x, bounds.extents.y);
                    capsuleCollider.height = bounds.size.z;
                }

                // Adjust the height to account for the radius at the ends of the capsule
                capsuleCollider.height = Mathf.Max(capsuleCollider.height, capsuleCollider.radius * 2);
                // Account for the height being the 'end to end' distance including the caps
                capsuleCollider.height += capsuleCollider.radius * 2;
            }
        }

        private int DeterminePrimaryAxis(Vector3 extents) {
            // Find the largest extent to determine the primary axis.
            if (extents.x > extents.y && extents.x > extents.z) {
                return 0; // x-axis is the primary
            }

            if (extents.y > extents.x && extents.y > extents.z) {
                return 1; // y-axis is the primary
            }

            return 2; // z-axis is the primary
        }
    }

    public static class ColliderAdjuster {
        private static readonly Dictionary<Type, Action<GameObject>> _adjustStrategyByColliderType;

        static ColliderAdjuster() {
            _adjustStrategyByColliderType = new Dictionary<Type, Action<GameObject>> {
                { typeof(BoxCollider), new BoxColliderAdjuster().AdjustCollider },
                { typeof(SphereCollider), new SphereColliderAdjuster().AdjustCollider },
                { typeof(CapsuleCollider), new CapsuleColliderAdjuster().AdjustCollider }
            };
        }

        [MenuItem("BabyCheese/Tools/FitColliderToMesh")]
        public static void AdjustColliderMenuItem() {
            var target = Selection.activeGameObject;
            if (!target) {
                return;
            }

            AdjustCollider(target);
        }

        public static void AdjustCollider(GameObject target) {
            var collider = target.GetComponent<Collider>();

            if (_adjustStrategyByColliderType.TryGetValue(collider.GetType(), out var adjustMethod)) {
                adjustMethod.Invoke(target);
            }
            else {
                Debug.LogError($"Unsupported collider type: {collider.GetType()}");
            }
        }
    }
}