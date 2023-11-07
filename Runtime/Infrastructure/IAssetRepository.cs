using System;

namespace Infrastructure {
    public class LoadAssetResult<TObject> {
        public TObject Result { get; }
        public string Error { get; }

        public LoadAssetResult(TObject result, string error = null) {
            Result = result;
            Error = error;
        }
    }

    public interface IAssetRepository {
        void LoadAsset<TObject>(object key, Action<LoadAssetResult<TObject>> callback);
    }
}