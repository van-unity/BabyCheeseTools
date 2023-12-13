using System;
using Unity.Collections;
using UnityEngine;

namespace BabyCheeseTools {
    [CreateAssetMenu(fileName = "ColorLibrary", menuName = "BabyCheese/Colors/ColorLibrary", order = 0)]
    public class ColorLibrary : ScriptableObject {
        [Serializable]
        private class ColorWithName {
            public string name;
            public Color color;
            public string ID { get; set; }
        }

        [SerializeField] private ColorWithName[] _colors;

        private void OnValidate() {
            foreach (var colorWithName in _colors) {
                if (string.IsNullOrEmpty(colorWithName.ID)) {
                    colorWithName.ID = Guid.NewGuid().ToString();
                }
            }
        }
    }
}