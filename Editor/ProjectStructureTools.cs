using UnityEditor;

namespace BabyCheeseTools.Editor {
    public static class ProjectStructureTools {
        public static void CreateAssetStoreProjectFolders(string projectName) {
            var folderGuid = AssetDatabase.CreateFolder("Assets", projectName);
            if (string.IsNullOrEmpty(folderGuid)) {
                EditorUtility.DisplayDialog($"Couldn't create folder {projectName}",
                    $"Couldn't create folder {projectName}", "OK");
                return;
            }

            AssetDatabase.CreateFolder($"Assets/{projectName}", "Models");
            AssetDatabase.CreateFolder($"Assets/{projectName}", "Textures");
            AssetDatabase.CreateFolder($"Assets/{projectName}", "Materials");
            AssetDatabase.CreateFolder($"Assets/{projectName}", "Prefabs");
            AssetDatabase.CreateFolder($"Assets/{projectName}", "Demo");
            AssetDatabase.CreateFolder($"Assets/{projectName}/Demo", "Scenes");
            AssetDatabase.CreateFolder($"Assets/{projectName}/Demo", "Materials");

            AssetDatabase.CreateFolder("Assets", "Demo");
            AssetDatabase.CreateFolder("Assets/Demo", "Scenes");
            AssetDatabase.CreateFolder("Assets/Demo", "Materials");
            AssetDatabase.CreateFolder("Assets/Demo", "Prefabs");
            AssetDatabase.CreateFolder("Assets/Demo", "Scripts");
            AssetDatabase.CreateFolder("Assets/Demo", "Textures");
        }
    }
}