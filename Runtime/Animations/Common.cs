using System;
using System.Collections;
using UnityEngine;

namespace BabyCheeseTools.Animations {
    public static class Common {
        public static IEnumerator Iterator(float duration, Easing easing, Action<float> callback, Action onComplete) {
            float time = 0;
            var easingFunction = EasingFunctions.FunctionByEasing[easing];

            while (time <= duration) {
                time += Time.deltaTime;
                var t = easingFunction(time / duration);
                callback.Invoke(t);
                yield return null;
            }

            // Ensure the final state is set
            callback.Invoke(1);

            // Call the onComplete action if it's not null
            onComplete?.Invoke();
        }
    }
}