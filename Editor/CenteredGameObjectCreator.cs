using UnityEditor;
using UnityEngine;

namespace BabyCheeseTools.Editor {
    public class CenteredGameObjectCreator {
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
    }
}