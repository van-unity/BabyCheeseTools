using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace BabyCheeseTools.Editor {
    public static class PackageTools {
        private const string GIT_URL = "https://github.com/van-unity/BabyCheeseTools.git";

        private static AddRequest _addRequest;

        [MenuItem("BabyCheese/Update")]
        private static void UpdateBabyCheesePackage() {
            _addRequest = Client.Add(GIT_URL);

            EditorApplication.update += ProgressUpdate;
        }

        private static void ProgressUpdate() {
            if (_addRequest.IsCompleted) {
                if (_addRequest.Status == StatusCode.Success)
                    EditorUtility.DisplayDialog("Success",
                        "Package updated successfully: " + _addRequest.Result.packageId, "Ok");
                else if (_addRequest.Status >= StatusCode.Failure)
                    EditorUtility.DisplayDialog("Failed",
                        "Failed to update package: " + _addRequest.Error.message, "Ok");

                EditorApplication.update -= ProgressUpdate;
            }
        }
    }
}