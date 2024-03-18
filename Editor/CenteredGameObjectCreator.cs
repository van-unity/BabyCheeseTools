using BabyCheeseTools.Extensions;
using UnityEditor;
using UnityEngine;

namespace BabyCheeseTools.Editor {
    public static class CenteredGameObjectCreator {
        [MenuItem("BabyCheese/Tools/Create Centered Pivot", false, 10)]
        static void CreateCenteredGameObject() {
            // Ensure that there is an active GameObject selected
            if (Selection.activeGameObject == null) {
                Debug.LogWarning("Please select a GameObject.");
                return;
            }

            // Calculate the bounds
            Bounds bounds = BoundsTools.CalculateOOBB(Selection.activeTransform);

            // Create a new GameObject
            GameObject centeredObject = new GameObject("Pivot");

            // Set the new GameObject's position to the center of the bounds in world space
            centeredObject.transform.position = Selection.activeTransform.TransformPoint(bounds.center);

            // Make the new GameObject a child of the selected GameObject
            Selection.activeTransform.SetParent(centeredObject.transform, true);

            // Select the new GameObject in the editor
            Selection.activeGameObject = centeredObject;
        }

        // The rest of the script with CalculateOOBB and CalculateLocalBounds methods remains unchanged...

        // Validate the MenuItem
        [MenuItem("BabyCheese/Tools/Create Centered Pivot", true)]
        static bool ValidateCreateCenteredGameObject() {
            // The menu item will be disabled if no GameObject is selected.
            return Selection.activeGameObject != null;
        }

        [MenuItem(
            "BabyCheese/Tools/Create Pivot at Bottom Center %#p")] // Shortcut: Ctrl+Shift+P (Cmd+Shift+P on macOS)
        private static void CreatePivotAtBottomCenter() {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0) {
                Debug.LogWarning("No objects selected.");
                return;
            }

            Bounds combinedBounds = new Bounds();
            bool hasBounds = false;

            foreach (var obj in selectedObjects) {
                Bounds bounds = obj.GetWorldBounds();
                if (bounds.size != Vector3.zero) {
                    if (!hasBounds) {
                        combinedBounds = bounds;
                        hasBounds = true;
                    } else {
                        combinedBounds.Encapsulate(bounds);
                    }
                }
            }

            if (!hasBounds) {
                Debug.LogWarning("Selected objects do not have MeshRenderers.");
                return;
            }

            // Create a new GameObject as the pivot
            GameObject pivot = new GameObject("Custom Pivot");
            Vector3 bottomCenter =
                new Vector3(combinedBounds.center.x, combinedBounds.min.y, combinedBounds.center.z);
            pivot.transform.position = bottomCenter;

            Undo.RegisterCreatedObjectUndo(pivot, "Create Pivot at Bottom Center");

            foreach (var selectedObject in selectedObjects) {
                selectedObject.transform.SetParent(pivot.transform, true);
            }

            Selection.activeGameObject = pivot;
            EditorGUIUtility.PingObject(pivot);
        }

        // Validate the MenuItem
        [MenuItem(
            "BabyCheese/Tools/Create Pivot at Bottom Center %#p",
            true)] // Shortcut: Ctrl+Shift+P (Cmd+Shift+P on macOS)
        static bool ValidateCreateBottomPivotGameObject() {
            // The menu item will be disabled if no GameObject is selected.
            return Selection.activeGameObject != null;
        }
    }
}