using System;
using UnityEngine;

namespace BabyCheeseTools {
    public class SwipeDetector : MonoBehaviour
    {
        public static event Action<SwipeDirection> OnSwipe;

        public enum SwipeDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        private Vector2 _startMousePosition;
        private bool _swipeDetected = false;

        public float _swipeThreshold = 50f; // Minimum distance for a swipe to be registered

        private void Update()
        {
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button press
            {
                _startMousePosition = Input.mousePosition; // Record start position
                _swipeDetected = false;
            }

            if (Input.GetMouseButtonUp(0) && !_swipeDetected) // Check for left mouse button release
            {
                Vector2 endMousePosition = Input.mousePosition;
                Vector2 swipe = endMousePosition - _startMousePosition;

                if (swipe.magnitude > _swipeThreshold) // Check if the swipe exceeds the threshold
                {
                    _swipeDetected = true; // Set to true to avoid multiple detections
                    swipe.Normalize();

                    // Determine swipe direction
                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    {
                        // Horizontal swipe
                        if (swipe.x > 0) {
                            OnSwipe?.Invoke(SwipeDirection.Right);
                        }
                        else
                        {
                            OnSwipe?.Invoke(SwipeDirection.Left);
                        }
                    }
                    else
                    {
                        // Vertical swipe
                        if (swipe.y > 0)
                        {
                            OnSwipe?.Invoke(SwipeDirection.Up);
                        }
                        else
                        {
                            OnSwipe?.Invoke(SwipeDirection.Down);
                        }
                    }
                }
            }
        }
    }
}
