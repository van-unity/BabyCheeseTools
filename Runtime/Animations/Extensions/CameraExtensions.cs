using System;
using System.Collections;
using UnityEngine;

namespace Animations.Extensions {
    public static class CameraExtensions {
        public static IEnumerator AnimateBackgroundColor(this Camera camera, Color targetColor, float duration,
            Easing easing) {
            Color startColor = camera.backgroundColor;
            var easingFunction = EasingFunctions.FunctionByEasing[easing];

            return Iterator(duration, easingFunction, (t) => {
                // Lerp the background color from the start color to the target color based on the easing function's progress
                camera.backgroundColor = Color.Lerp(startColor, targetColor, t);
            });
        }

        private static IEnumerator Iterator(float duration, Func<float, float> easingFunction,
            Action<float> applyValue) {
            float time = 0f;
            while (time < duration) {
                time += Time.deltaTime;
                float progress = time / duration;
                float easedProgress = easingFunction(progress);
                applyValue(easedProgress);
                yield return null;
            }

            applyValue(1); // Ensure we set the background color to the target color at the end
        }
    }
}