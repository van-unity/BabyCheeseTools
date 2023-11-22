using UnityEngine;

namespace BabyCheeseTools.ObjectPool {
    public interface IObjectPool<T> where T : Component, ISpawnable {
        bool IsReady { get; }
        T Get();
        void Return(T objectToReturn);
    }
}