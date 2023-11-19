using UnityEditor;
using UnityEngine;

namespace Editor.Screenshots {
    public static class ScreenshotTool {
        [MenuItem("BabyCheese/Tools/Take Screenshot")]
        private static void TakeScreenshot() {
            string path = EditorUtility.SaveFilePanel(
                "Save screenshot",
                "",
                "Screenshot.png",
                "png");

            if (path.Length != 0) {
                ScreenCapture.CaptureScreenshot(path);
                // Optional: Open the folder containing the screenshot
                EditorUtility.RevealInFinder(path);
            }
        }
    }
}