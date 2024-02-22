using UnityEngine;

namespace BabyCheeseTools {
    public class SetTargetFrameRate : MonoBehaviour {
        public int _frameRate = -1;

        private void Awake() {
            Application.targetFrameRate = _frameRate;
        }
    }
}