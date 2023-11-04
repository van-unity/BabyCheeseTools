using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component {
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
            Debug.LogError("asas");
        }
        else {
            Destroy(gameObject);
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