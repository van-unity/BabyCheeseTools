using System;
using UnityEngine;

namespace Infrastructure {
    public class LoadAssetResult<TObject> {
        public TObject LoadedObject { get; }
        public string Error { get; }

        public LoadAssetResult(TObject loadedObject, string error = null) {
            LoadedObject = loadedObject;
            Error = error;
        }
    }

    public interface IAssetRepository {
        void LoadAsset<TObject>(object key, Action<LoadAssetResult<TObject>> callback);
        void Release(object key);
        bool ReleaseInstance(GameObject gameObject);
    }
}