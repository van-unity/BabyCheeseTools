using System;
using UnityEngine;

namespace Editor {
    public static class BoundsTools {
        //calculating Object-Oriented Bounding Box to respect objects rotation 
        public static Bounds CalculateOOBB(Transform transform) {
            // Start with an empty bounds object
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            bool hasBounds = false;

            // Get all MeshFilters in this object and its children
            MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters) {
                if (hasBounds) {
                    bounds.Encapsulate(CalculateLocalBounds(transform, meshFilter));
                }
                else {
                    bounds = CalculateLocalBounds(transform, meshFilter);
                    hasBounds = true;
                }
            }

            return bounds;
        }

        private static Bounds CalculateLocalBounds(Transform parent, MeshFilter meshFilter) {
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            if (vertices.Length == 0) return new Bounds();

            // No need to transform vertices to world space and back
            // Instead, directly calculate the bounds in local space

            Bounds bounds = new Bounds(vertices[0], Vector3.zero);

            for (int i = 1; i < vertices.Length; i++) {
                bounds.Encapsulate(vertices[i]);
            }

            // Now transform the bounds to the parent's local space
            Bounds transformedBounds = new Bounds(meshFilter.transform.TransformPoint(bounds.center), Vector3.zero);
            foreach (Vector3 vertex in vertices) {
                transformedBounds.Encapsulate(meshFilter.transform.TransformPoint(vertex));
            }

            transformedBounds.center = parent.InverseTransformPoint(transformedBounds.center);

            // We assume here that the object is uniformly scaled; otherwise, bounds need to be scaled properly
            transformedBounds.size = Vector3.Scale(bounds.size, meshFilter.transform.lossyScale);

            return transformedBounds;
        }
    }
}