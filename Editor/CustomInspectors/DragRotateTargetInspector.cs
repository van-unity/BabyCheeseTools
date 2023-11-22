using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BabyCheeseTools.Editor.CustomInspectors {
    [CustomEditor(typeof(DragRotateTarget))]
    public class DragRotateTargetInspector : UnityEditor.Editor {
        private enum ColliderType {
            Box,
            Sphere,
            Capsule
        }

        private DragRotateTarget _target;

        private void OnEnable() {
            _target = (DragRotateTarget)target;
        }

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();
            var fitButton = new Button(() => { ColliderAdjuster.AdjustCollider(_target.gameObject); }) {
                text = "Adjust Collider"
            };
            root.Add(fitButton);

            var collider = _target.GetComponent<Collider>();
            if (!collider) {
                root.Add(ColliderButtonsPanel());
            }

            return root;
        }

        private VisualElement ColliderButtonsPanel() {
            var panel = new VisualElement {
                style = {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.Center)
                }
            };

            panel.Add(CreateColliderButton(ColliderType.Box));
            panel.Add(CreateColliderButton(ColliderType.Sphere));
            panel.Add(CreateColliderButton(ColliderType.Capsule));

            return panel;
        }

        private VisualElement CreateColliderButton(ColliderType colliderType) {
            var button = new Button(() => {
                AddCollider(colliderType);
                ColliderAdjuster.AdjustCollider(_target.gameObject);
            }) {
                text = colliderType.ToString(),
                style = {
                    width = 64,
                    height = 64
                }
            };

            return button;
        }

        private void AddCollider(ColliderType colliderType) {
            switch (colliderType) {
                case ColliderType.Box:
                    _target.gameObject.AddComponent<BoxCollider>();
                    break;
                case ColliderType.Sphere:
                    _target.gameObject.AddComponent<SphereCollider>();
                    break;
                case ColliderType.Capsule:
                    _target.gameObject.AddComponent<CapsuleCollider>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colliderType), colliderType, null);
            }
        }
    }
}