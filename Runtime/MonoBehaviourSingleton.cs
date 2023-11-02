using UnityEngine;

namespace Packages.com.babycheese.tools {
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour
        where T : Component {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs is { Length: > 0 })
                        _instance = objs[0];
                    if (objs is { Length: > 1 }) {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }

                    if (_instance != null) {
                        return _instance;
                    }

                    var obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }

                return _instance;
            }
        }
    }

    public abstract class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
        where T : Component {
        public static T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance == null) {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}