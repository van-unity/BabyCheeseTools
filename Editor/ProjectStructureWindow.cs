using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BabyCheeseTools.Editor {
    public class ProjectStructureWindow : EditorWindow {
        [MenuItem("BabyCheese/CreateProjectStructure")]
        private static void ShowWindow() {
            var window = GetWindow<ProjectStructureWindow>();
            window.titleContent = new GUIContent("Project Structure");
            window.Show();
        }

        private void CreateGUI() {
            var projectNameInput = new TextField("Project Name");
            var createButton = new Button(() => {
                ProjectStructureTools.CreateAssetStoreProjectFolders(projectNameInput.value);
            }) {
                text = "Create"
            };

            rootVisualElement.Add(projectNameInput);
            rootVisualElement.Add(createButton);
        }
    }
}