using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animations {
    public enum Easing {
        InQuad = 0,
        OutQuad = 1,
        InOutQuad = 2,
        InCubic = 3,
        OutCubic = 4,
        InOutCubic = 5,
        InExpo = 6,
        OutExpo = 7,
        InOutExpo = 8,
        InSine = 9,
        OutSine = 10,
        InOutSine = 11,
        InElastic = 12,
        OutElastic = 13,
        InOutElastic = 14
    }
    
    public static class EasingFunctions {
        public static Dictionary<Easing, Func<float, float>> FunctionByEasing { get; }

        static EasingFunctions() {
            FunctionByEasing = new Dictionary<Easing, Func<float, float>> {
                { Easing.InQuad, InQuad },
                { Easing.OutQuad, OutQuad },
                { Easing.InOutQuad, InOutQuad },
                { Easing.InCubic, InCubic },
                { Easing.OutCubic, OutCubic },
                { Easing.InOutCubic, InOutCubic },
                { Easing.InExpo, InExpo },
                { Easing.OutExpo, OutExpo },
                { Easing.InOutExpo, InOutExpo },
                { Easing.InSine, InSine },
                { Easing.OutSine, OutSine },
                { Easing.InOutSine, InOutSine },
                { Easing.InElastic, InElastic },
                { Easing.OutElastic, OutElastic },
                { Easing.InOutElastic, InOutElastic }
            };
        }

        private static float InCubic(float t) {
            return t * t * t;
        }

        private static float OutCubic(float t) {
            return 1 - Mathf.Pow(1 - t, 3);
        }

        private static float InOutCubic(float t) {
            return t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        }

        private static float InQuad(float t) {
            return t * t;
        }

        private static float OutQuad(float t) {
            return 1 - (1 - t) * (1 - t);
        }

        private static float InOutQuad(float t) {
            return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }

        private static float InExpo(float t) {
            return Mathf.Approximately(t, 0) ? 0 : Mathf.Pow(2, 10 * t - 10);
        }

        private static float OutExpo(float t) {
            return Mathf.Approximately(t, 1) ? 1 : 1 - Mathf.Pow(2, -10 * t);
        }

        private static float InOutExpo(float t) {
            if (Mathf.Approximately(t, 0) || Mathf.Approximately(t, 1)) return t;
            if (t < 0.5f) return Mathf.Pow(2, 20 * t - 10) / 2;
            return (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
        }

        private static float InSine(float t) {
            return 1 - Mathf.Cos((t * Mathf.PI) / 2);
        }

        private static float OutSine(float t) {
            return Mathf.Sin((t * Mathf.PI) / 2);
        }

        private static float InOutSine(float t) {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }

// Elastic easing functions can be more complex due to the need for more
// parameters to define the elasticity of the motion. The simplest implementation
// without additional parameters is as follows, but you might want to extend these
// with more control over the elasticity.

        private static float InElastic(float t) {
            float c4 = (2 * Mathf.PI) / 3;
            return t == 0
                ? 0
                : Math.Abs(t - 1) < Mathf.Epsilon
                    ? 1
                    : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4);
        }

        private static float OutElastic(float t) {
            float c4 = (2 * Mathf.PI) / 3;
            return t == 0
                ? 0
                : Math.Abs(t - 1) < Mathf.Epsilon
                    ? 1
                    : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
        }

        private static float InOutElastic(float t) {
            float c5 = (2 * Mathf.PI) / 4.5f;
            return t == 0
                ? 0
                : Math.Abs(t - 1) < Mathf.Epsilon
                    ? 1
                    : t < 0.5
                        ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
        }
    }
}