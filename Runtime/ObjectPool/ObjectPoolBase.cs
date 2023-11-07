using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace ObjectPool {
    public abstract class ObjectPoolBase<TObject> : MonoBehaviour, IObjectPool<TObject>
        where TObject : Component, ISpawnable {
        [SerializeField] private int _initialSize = 10;

        private Transform _container;
        private IAssetRepository _repository;
        private GameObject _objectPrefab;
        private readonly Stack<TObject> _pool = new();

        public bool IsReady { get; private set; }

        public void Initialize(IAssetRepository repository) {
            _repository = repository;
            _repository.LoadAsset<GameObject>(GetAssetPath(), result => {
                if (string.IsNullOrEmpty(result.Error)) {
                    _objectPrefab = result.LoadedObject;
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

        private TObject Create() {
            var instantiatedItem = Instantiate(_objectPrefab, _container, false);
            var component = instantiatedItem.GetComponent<TObject>();
            component.Initialize();
            return component;
        }

        protected abstract object GetAssetPath();

        private void Awake() {
            _container = new GameObject($"{typeof(TObject).Name}_Pool").transform;
        }

        private void OnDestroy() {
            _repository.Release(_objectPrefab);
            _pool.Clear();
        }
    }
}