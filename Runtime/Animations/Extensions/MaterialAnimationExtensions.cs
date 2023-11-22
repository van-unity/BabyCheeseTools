using System;
using System.Collections;
using UnityEngine;

namespace BabyCheeseTools.Animations.Extensions {
    public static class MaterialAnimationExtensions {
        public static IEnumerator AnimateColor(this Material material, Color to, float duration, Easing easing,
            Action onComplete = null) {
            var startColor = material.color;
            return Common.Iterator(duration, easing,
                value => material.color = Color.Lerp(startColor, to, value), onComplete);
        }
    }
}