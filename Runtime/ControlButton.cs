using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BabyCheeseTools {
    [RequireComponent(typeof(BoxCollider))]
    public class ControlButton : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private ParticleSystem _particle;

        private BoxCollider _collider;

        public Transform Transform { get; private set; }

        public event Action Clicked;

        public void Show() {
            _collider.enabled = true;
            _particle.Play(true);
        }

        public void Hide() {
            _collider.enabled = false;
            _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void OnPointerClick(PointerEventData eventData) {
            Clicked?.Invoke();
        }

        private void Awake() {
            _collider = GetComponent<BoxCollider>();
            Transform = transform;
        }
    }
}