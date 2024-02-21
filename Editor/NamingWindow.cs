using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BabyCheeseTools.Editor {
    public class NamingWindow : EditorWindow {
        [MenuItem("BabyCheese/Naming")]
        private static void ShowWindow() {
            var window = GetWindow<NamingWindow>();
            window.titleContent = new GUIContent("Naming");
            window.Show();
        }

        private void CreateGUI() {
            var searchInput = new TextField("Search");
            var replaceWithInput = new TextField("Replace");
            var replaceButton = new Button(() => {
                if (string.IsNullOrEmpty(searchInput.text) || string.IsNullOrEmpty(replaceWithInput.text)) {
                    return;
                }

                ReplaceInAssetNames(searchInput.text, replaceWithInput.text);
            }) {
                text = "Replace"
            };
            rootVisualElement.Add(searchInput);
            rootVisualElement.Add(replaceWithInput);
            rootVisualElement.Add(replaceButton);
        }

        public static void ReplaceInAssetNames(string search, string replaceWith) {
            // Get all asset paths in the project
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string assetPath in allAssetPaths) {
                // Check if the asset's name contains the search string
                if (Path.GetFileName(assetPath).Contains(search)) {
                    // Calculate the new name by replacing the search string with replaceWith
                    string newName = Path.GetFileName(assetPath).Replace(search, replaceWith);
                    // Calculate the new path for the asset
                    string newPath = Path.GetDirectoryName(assetPath) + "/" + newName;
                    // Use AssetDatabase to rename the asset
                    AssetDatabase.RenameAsset(assetPath, newName);
                    // Optional: Print a message to confirm the asset has been renamed
                    Debug.Log($"Renamed {assetPath} to {newPath}");
                }
            }

            // Refresh the AssetDatabase to apply changes
            AssetDatabase.Refresh();
        }
    }
}