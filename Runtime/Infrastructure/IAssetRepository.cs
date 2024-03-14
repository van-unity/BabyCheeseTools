using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BabyCheeseTools.Infrastructure {
    public class LoadAssetResult<TObject> where TObject : Object {
        public TObject LoadedObject { get; }
        public string Error { get; }

        public LoadAssetResult(TObject loadedObject, string error = null) {
            LoadedObject = loadedObject;
            Error = error;
        }
    }

    public interface IAssetRepository {
        void LoadAsset<TObject>(object key, Action<LoadAssetResult<TObject>> callback) where TObject : Object;
        void Release(object key);
        bool ReleaseInstance(GameObject gameObject);
    }
}