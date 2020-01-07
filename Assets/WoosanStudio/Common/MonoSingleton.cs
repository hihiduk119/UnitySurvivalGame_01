using UnityEngine;

namespace WoosanStudio.Common
{
    /// <summary>
    /// 기본이 되는 싱글톤 패턴
    /// Awake부분에서 Find Object를 사용하기 때문에 빨리 메모리에 할당해야하는 스크립트는 직접 만들어 사용 해야함.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance;

        void Awake()
        {
            if (Instance != null) return;
            Instance = FindObjectOfType(typeof(T)) as T;
            Instance.Init();
        }

        public virtual void Init() { }

        private void OnApplicateQuit()
        {
            Instance = null;
        }
    }
}
