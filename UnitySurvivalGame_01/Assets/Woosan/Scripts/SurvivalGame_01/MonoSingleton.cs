using UnityEngine;

public abstract class MonoSingleton <T>: MonoBehaviour where T : MonoSingleton <T>{
	public static T instance;

	void Awake() {
		instance = GameObject.FindObjectOfType (typeof(T)) as T;
		instance.Init ();
	}

	public virtual void Init() { }

	private void OnApplicateQuit() {
		instance = null;
	}
}
