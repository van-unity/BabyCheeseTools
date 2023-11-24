using UnityEditor;
using UnityEngine;

namespace BabyCheeseTools.Editor {
    public static class MeshInfoTool {
        [MenuItem("BabyCheese/Tools/Mesh Info")]
        private static void GetMeshInfo() {
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject == null) {
                Debug.Log("No object selected.");
                return;
            }

            MeshFilter[] meshFilters = selectedObject.GetComponentsInChildren<MeshFilter>();
            int totalTriangles = 0;
            int totalVertices = 0;

            foreach (MeshFilter mf in meshFilters) {
                if (mf.sharedMesh != null) {
                    totalTriangles += mf.sharedMesh.triangles.Length / 3; // Each triangle is 3 indices
                    totalVertices += mf.sharedMesh.vertexCount;
                }
            }

            EditorUtility.DisplayDialog("Mesh Info",
                $"Object: {selectedObject.name}\n" + $"Total Triangles: {totalTriangles}\n" +
                $"Total Vertices: {totalVertices}", "OK");
        }
    }
}