using System;
using System.Collections;
using UnityEngine;

namespace Animations.Extensions {
    public static class TransformAnimationExtensions {
        public static IEnumerator Move(this Transform transform, Vector3 to, float duration, Easing easing,
            Action onComplete = null) {
            var currentPos = transform.position;
            return Common.Iterator(duration, easing, value => transform.position = Vector3.Lerp(currentPos, to, value),
                onComplete);
        }

        public static IEnumerator MoveLocal(this Transform transform, Vector3 to, float duration, Easing easing,
            Action onComplete = null) {
            var currentPos = transform.localPosition;
            return Common.Iterator(duration, easing,
                value => transform.localPosition = Vector3.Lerp(currentPos, to, value), onComplete);
        }

        public static IEnumerator RotateEuler(this Transform transform, Vector3 to, float duration, Easing easing,
            bool fastBeyond360 = false, Action onComplete = null) {
            var currentEuler = transform.eulerAngles;
            Vector3 toEuler = fastBeyond360 ? to : OptimizeEuler(to, currentEuler);
            return Common.Iterator(duration, easing,
                value => transform.eulerAngles = Vector3.Lerp(currentEuler, toEuler, value), onComplete);
        }

        public static IEnumerator RotateEulerLocal(this Transform transform, Vector3 to, float duration, Easing easing,
            bool fastBeyond360 = false, Action onComplete = null) {
            var currentEuler = transform.localEulerAngles;
            Vector3 toEuler = fastBeyond360 ? to : OptimizeEuler(to, currentEuler);
            return Common.Iterator(duration, easing,
                value => transform.localEulerAngles = Vector3.Lerp(currentEuler, toEuler, value), onComplete);
        }

        public static IEnumerator RotateQuaternion(this Transform transform, Quaternion to, float duration,
            Easing easing, Action onComplete = null) {
            var currentRot = transform.rotation;
            return Common.Iterator(duration, easing,
                value => transform.rotation = Quaternion.Lerp(currentRot, to, value), onComplete);
        }

        public static IEnumerator RotateQuaternionLocal(this Transform transform, Quaternion to, float duration,
            Easing easing, Action onComplete = null) {
            var currentRot = transform.localRotation;
            return Common.Iterator(duration, easing,
                value => transform.localRotation = Quaternion.Lerp(currentRot, to, value), onComplete);
        }

        public static IEnumerator Scale(this Transform transform, Vector3 to, float duration, Easing easing,
            Action onComplete = null) {
            var currentScale = transform.localScale;
            return Common.Iterator(duration, easing,
                value => transform.localScale = Vector3.Lerp(currentScale, to, value), onComplete);
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
    }
}