using System.IO;
using UnityEditor;
using UnityEngine;

namespace BabyCheeseTools.Editor.Screenshots {
    public static class ScreenshotTool {
        private static string _lastDirectoryPath;
        
        [MenuItem("BabyCheese/Tools/Take Screenshot")]
        private static void TakeScreenshot() {
            string path = EditorUtility.SaveFilePanel(
                "Save screenshot",
                _lastDirectoryPath,
                "Screenshot.png",
                "png");

            _lastDirectoryPath = Path.GetDirectoryName(path);
            
            if (path.Length != 0) {
                ScreenCapture.CaptureScreenshot(path);
                // Optional: Open the folder containing the screenshot
                EditorUtility.RevealInFinder(path);
            }
        }
    }
}