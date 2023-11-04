using System;
using System.Collections;
using UnityEngine;

namespace Animations.Extensions {
    public static class TransformAnimationExtensions {
        public static IEnumerator Move(this Transform transform, Vector3 to, float duration, Easing easing) {
            var currentPos = transform.position;
            return Iterator(duration, easing, value => { transform.position = Vector3.Lerp(currentPos, to, value); });
        }

        public static IEnumerator MoveLocal(this Transform transform, Vector3 to, float duration, Easing easing) {
            var currentPos = transform.localPosition;
            return Iterator(duration, easing,
                value => { transform.localPosition = Vector3.Lerp(currentPos, to, value); });
        }

        public static IEnumerator RotateEuler(this Transform transform, Vector3 to, float duration, Easing easing,
            bool fastBeyond360 = false) {
            var currentEuler = transform.eulerAngles;
            Vector3 toEuler = fastBeyond360 ? to : OptimizeEuler(to, currentEuler);
            return Iterator(duration, easing,
                value => { transform.eulerAngles = Vector3.Lerp(currentEuler, toEuler, value); });
        }

        public static IEnumerator RotateEulerLocal(this Transform transform, Vector3 to, float duration, Easing easing,
            bool fastBeyond360 = false) {
            var currentEuler = transform.localEulerAngles;
            Vector3 toEuler = fastBeyond360 ? to : OptimizeEuler(to, currentEuler);
            return Iterator(duration, easing,
                value => { transform.localEulerAngles = Vector3.Lerp(currentEuler, toEuler, value); });
        }

        // Helper method to find the shortest path for rotation if not using FastBeyond360 mode
        private static Vector3 OptimizeEuler(Vector3 targetEuler, Vector3 currentEuler) {
            Vector3 optimizedEuler = targetEuler;
            for (int i = 0; i < 3; i++) {
                if (Mathf.Abs(targetEuler[i] - currentEuler[i]) > 180f) {
                    if (targetEuler[i] > currentEuler[i]) {
                        optimizedEuler[i] -= 360f;
                    }
                    else {
                        optimizedEuler[i] += 360f;
                    }
                }
            }

            return optimizedEuler;
        }

        public static IEnumerator RotateQuaternion(this Transform transform, Quaternion to, float duration,
            Easing easing) {
            var currentRot = transform.rotation;
            return Iterator(duration, easing,
                value => { transform.rotation = Quaternion.Lerp(currentRot, to, value); });
        }

        public static IEnumerator RotateQuaternionLocal(this Transform transform, Quaternion to, float duration,
            Easing easing) {
            var currentRot = transform.localRotation;
            return Iterator(duration, easing,
                value => { transform.localRotation = Quaternion.Lerp(currentRot, to, value); });
        }

        public static IEnumerator Scale(this Transform transform, Vector3 to, float duration, Easing easing) {
            var currentScale = transform.localScale;
            return Iterator(duration, easing,
                value => { transform.localScale = Vector3.Lerp(currentScale, to, value); });
        }

        private static IEnumerator Iterator(float duration, Easing easing, Action<float> callback) {
            float time = 0;
            var easingFunction = EasingFunctions.FunctionByEasing[easing];

            while (time <= duration) {
                time += Time.deltaTime;
                var t = easingFunction(time / duration);
                callback.Invoke(t);
                yield return null;
            }

            callback?.Invoke(1);
        }
    }
}