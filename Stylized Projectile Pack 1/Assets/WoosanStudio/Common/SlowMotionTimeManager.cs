using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Common
{
    public class SlowMotionTimeManager : MonoBehaviour
    {
        //get 의 초기화를 위해 사용.
        public float slowdownFactor { get; private set; } = 0.1f;
        public float recoverFactor{ get; private set; } = 1.5f;

        /// <summary>
        /// 슬로우 모션을 발생 시키는 부분
        /// </summary>
        void DoSlowMotion() {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            AudioManager.instance.SlowMotion(Time.timeScale);
        }

        /// <summary>
        /// 슬로우 모션 회복하는 메서드.
        /// </summary>
        /// <returns><c>true</c>, if recover motion was done, <c>false</c> otherwise.</returns>
        bool DoRecoverMotion() {
            bool _recoverComplete = false;
            Time.timeScale += (1f / recoverFactor) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            if (Time.timeScale >= 1f) { _recoverComplete = true; }

            //Debug.Log("Time = " + Time.timeScale);
            AudioManager.instance.SlowMotion(Time.timeScale);
            return _recoverComplete;
        }

        /// <summary>
        /// 슬로우 모션 회복하는 메서드. 코루틴
        /// </summary>
        /// <returns>The do reciver motion.</returns>
        IEnumerator CorDoReciverMotion()
        {
            WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
            while(!DoRecoverMotion()) {
                yield return waitFrame;
            }

        }

        /*private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) {
                DoSlowMotion();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(CorDoReciverMotion());
            }

            if(Input.GetKeyDown(KeyCode.P)) {
                AudioManager.instance.MusicLoop(SoundLoop.LetsRock);
            }
        }*/
    }
}
