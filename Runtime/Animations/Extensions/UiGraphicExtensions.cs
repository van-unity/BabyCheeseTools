using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BabyCheeseTools.Animations.Extensions {
    namespace Animations.Extensions {
        public static class GraphicAnimationExtensions {
            public static IEnumerator AnimateGraphicColor(this Graphic graphic, Color to, float duration, Easing easing,
                Action onComplete = null) {
                var startColor = graphic.color;
                return Common.Iterator(
                    duration,
                    easing,
                    value => graphic.color = Color.Lerp(startColor, to, value),
                    onComplete
                );
            }

            public static IEnumerator AnimateGraphicAlpha(this Graphic graphic, float toAlpha, float duration,
                Easing easing, Action onComplete = null) {
                float startAlpha = graphic.color.a;
                return Common.Iterator(duration, easing, value => {
                    Color newColor = graphic.color;
                    newColor.a = Mathf.Lerp(startAlpha, toAlpha, value);
                    graphic.color = newColor;
                }, onComplete);
            }
        }
    }
}