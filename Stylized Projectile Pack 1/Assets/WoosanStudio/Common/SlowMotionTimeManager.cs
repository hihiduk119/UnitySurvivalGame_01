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

        //인터페이스로 만든 오디
        public new WoosanStudio.Common.Audio.IAudio audio;

        /// <summary>
        /// 슬로우 모션을 발생 시키는 부분
        /// </summary>
        void DoSlowMotion() {
            Time.timeScale = slowdownFactor;
            //Time.fixedDeltaTime 는 0.05시간에 한번씩 계산하기 때문에 변환 time scale 변경시 같이 해줘야함
            //안그러면 오브젝트가 끊기는 것처럼 날아감 보임
            Time.fixedDeltaTime = Time.timeScale * 0.05f;

            //느린 사운드 적용 부분 [둘중에 하나 써야]
            //인터페이스로 구현
            //audio.Pitch(Time.timeScale);
            //실제 호출
            //AudioManager.instance.SlowMotion(Time.timeScale);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) {
                //씬안의 모든 리지드 바디 가져와서 벨로시티 저장
                DoSlowMotion();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Time.timeScale = 1f;
                //Time.fixedDeltaTime 는 0.05시간에 한번씩 계산하기 때문에 변환 time scale 변경시 같이 해줘야함
                //안그러면 오브젝트가 끊기는 것처럼 날아감 보임
                Time.fixedDeltaTime = Time.timeScale * 0.05f;
                //Debug.Log("[3] time = " + Time.timeScale + "   Time.fixedDeltaTime = " + Time.fixedDeltaTime);
            }

            if(Input.GetKeyDown(KeyCode.P)) {
                AudioManager.instance.MusicLoop(SoundLoop.LetsRock);
            }
        }
    }
}
