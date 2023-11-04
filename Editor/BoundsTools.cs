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
                    bounds.Encapsulate(CalculateLocalBounds(meshFilter));
                }
                else {
                    bounds = CalculateLocalBounds(meshFilter);
                    hasBounds = true;
                }
            }

            return bounds;
        }

        private static Bounds CalculateLocalBounds(MeshFilter meshFilter) {
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
    }
}