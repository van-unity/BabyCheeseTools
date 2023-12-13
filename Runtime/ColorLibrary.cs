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
            public string colorCode;
            public string ID { get; set; }
        }

        [SerializeField] private ColorWithName[] _colors;

        private void OnValidate() {
            foreach (var colorWithName in _colors) {
                if (string.IsNullOrEmpty(colorWithName.ID)) {
                    colorWithName.ID = Guid.NewGuid().ToString();
                }

                if (string.IsNullOrEmpty(colorWithName.colorCode)) {
                    colorWithName.colorCode = ColorToHex(colorWithName.color);
                }
            }
        }


        public static string ColorToHex(Color color) {
            int r = Mathf.RoundToInt(color.r * 255.0f);
            int g = Mathf.RoundToInt(color.g * 255.0f);
            int b = Mathf.RoundToInt(color.b * 255.0f);
            return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
        }
    }
}