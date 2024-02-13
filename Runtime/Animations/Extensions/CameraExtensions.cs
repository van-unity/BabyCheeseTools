using System;
using System.Collections;
using UnityEngine;

namespace BabyCheeseTools.Animations.Extensions {
    public static class CameraExtensions {
        public static IEnumerator AnimateBackgroundColor(this UnityEngine.Camera camera, Color targetColor,
            float duration,
            Easing easing) {
            var startColor = camera.backgroundColor;
            var easingFunction = EasingFunctions.FunctionByEasing[easing];

            return Iterator(duration, easingFunction, (t) => {
                // Lerp the background color from the start color to the target color based on the easing function's progress
                camera.backgroundColor = Color.Lerp(startColor, targetColor, t);
            });
        }

        private static IEnumerator Iterator(float duration, Func<float, float> easingFunction,
            Action<float> applyValue) {
            var time = 0f;
            while (time < duration) {
                time += Time.deltaTime;
                float progress = time / duration;
                float easedProgress = easingFunction(progress);
                applyValue(easedProgress);
                yield return null;
            }

            applyValue(1); // Ensure we set the background color to the target color at the end
        }

        public static IEnumerator
            AnimateFieldOfView(this UnityEngine.Camera camera, float targetFOV, float duration, Easing easing) {
            var startFOV = camera.fieldOfView;
            var easingFunction = EasingFunctions.FunctionByEasing[easing];

            return Iterator(duration, easingFunction,
                (t) => { camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t); });
        }
    }
}