using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace ObjectPool {
    public abstract class ObjectPoolBase<TObject> : MonoBehaviour, IObjectPool<TObject>
        where TObject : Component, ISpawnable {
        [SerializeField] private int _initialSize = 10;

        private Transform _container;
        private IAssetRepository _repository;
        private readonly Stack<TObject> _pool = new();

        public bool IsReady { get; private set; }

        public void Initialize(IAssetRepository repository) {
            _repository = repository;
            _repository.LoadAsset<TObject>(GetAssetPath(), result => {
                if (string.IsNullOrEmpty(result.Error)) {
                    for (int i = 0; i < _initialSize; i++) {
                        _pool.Push(Create());
                    }

                    IsReady = true;
                }
                else {
                    Debug.LogError($"Error initializing object pool: {result.Error}");
                }
            });
        }

        public TObject Get() {
            if (_pool.TryPop(out var item)) {
                item.OnSpawned();
                return item;
            }

            var newItem = Create();
            newItem.OnSpawned();
            return newItem;
        }

        public void Return(TObject objectToReturn) {
            _pool.Push(objectToReturn);
            objectToReturn.transform.SetParent(_container, false);
            objectToReturn.OnDeSpawned();
        }

        protected abstract object GetAssetPath();
        protected abstract TObject Create();
        protected abstract void ReleaseResources();

        private void Awake() {
            _container = new GameObject($"{typeof(TObject).Name}_Pool").transform;
        }

        private void OnDestroy() {
            ReleaseResources();
            _pool.Clear();
        }
    }
}