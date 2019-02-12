using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSManager : MonoSingleton<FPSManager> {
	public Text lbFps;
	float deltaTime = 0.0f;

	float fps;

	void Update() {
		this.Fps();
	}

	void Fps()
	{
		this.fps = 1.0f / deltaTime;
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
//		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
//		this.lbFps.text = string.Format("{0:0.0} ms  {1:0.} fps", 0f, fps);
		this.lbFps.text = string.Format("fps {0:0.}",fps);
	}
}
