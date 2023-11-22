using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabyCheeseTools.Extensions {
    public static class YieldInstructionsCache {
        private static readonly Dictionary<float, YieldInstruction> _yieldInstructionBySecond;

        static YieldInstructionsCache() {
            _yieldInstructionBySecond = new Dictionary<float, YieldInstruction>();
        }

        public static YieldInstruction Get(float seconds) {
            if (_yieldInstructionBySecond.TryGetValue(seconds, out var instruction)) {
                return instruction;
            }

            var newInstruction = new WaitForSeconds(seconds);
            _yieldInstructionBySecond[seconds] = newInstruction;

            return _yieldInstructionBySecond[seconds];
        }
    }

    public static class MonoBehaviourExtensions {
        public static void WaitForFramesAndExecute(this MonoBehaviour target, int frameCount, Action callback) {
            target.StartCoroutine(FramesCoroutine(frameCount, callback));
        }

        private static IEnumerator FramesCoroutine(int frameCount, Action callback) {
            for (int i = 0; i < frameCount; i++) {
                yield return null;
            }

            callback.Invoke();
        }

        public static void WaitForSecondsAndExecute(this MonoBehaviour target, float seconds, Action callback) {
            target.StartCoroutine(WaitCoroutine(seconds, callback));
        }

        private static IEnumerator WaitCoroutine(float seconds, Action callback) {
            yield return YieldInstructionsCache.Get(seconds);

            callback.Invoke();
        }
    }
}