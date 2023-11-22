namespace BabyCheeseTools.ObjectPool {
    public interface ISpawnable {
        void Initialize();
        void OnSpawned();
        void OnDeSpawned();
    }
}