using System;
using System.Collections;
using UnityEngine;

namespace BabyCheeseTools.Animations.Extensions {
    public static class Vector2Extensions {
        public static IEnumerator To(this Vector2 from, Vector2 to, float duration, Easing easing, Action<Vector2> callback,
            Action onComplete = null) {
            return Common.Iterator(duration, easing, t => { callback?.Invoke(Vector2.Lerp(from, to, t)); }, onComplete);
        }
    }
}