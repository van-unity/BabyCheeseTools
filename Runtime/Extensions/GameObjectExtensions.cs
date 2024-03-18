using UnityEngine;

namespace BabyCheeseTools.Extensions {
    public static class GameObjectExtensions {
        public static Bounds GetWorldBounds(this GameObject gameObject) {
            var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            if (renderers.Length == 0) {
                return new Bounds(gameObject.transform.position,
                    Vector3.zero); // No renderers, return empty bounds centered at object's position
            }

            var combinedBounds = renderers[0].bounds;
            foreach (MeshRenderer renderer in renderers) {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            return combinedBounds;
        }

        public static Vector3 GetWorldScaleFromBounds(this GameObject gameObject) {
            var bounds = gameObject.GetWorldBounds();
            return bounds.size; // This is the "scale" or size of the combined bounds in world space
        }
    }
}