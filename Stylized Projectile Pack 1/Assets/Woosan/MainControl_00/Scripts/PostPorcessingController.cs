using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEditor.SceneManagement;
using UnityEngine.PostProcessing;
namespace Woosan.SurvivalGame
{
    /// <summary>
    /// Post porcessing controller.
    /// 포스트 프로세싱의 컨트롤을 담당 한다.
    /// SlowMotion 연출시 화면이 붉어지는 현상 만들어줌.
    /// </summary>


    //해당 컴포넌트 필요
    [RequireComponent(typeof(PostProcessingBehaviour))]
    public class PostPorcessingController : MonoBehaviour
    {
        /// <summary>
        /// 사용할 컬러 그라딩 옵션
        /// </summary>
        private enum ColorGradingBasicSettingElement
        {
            Temperature = 0,
            Tint,
            HueShift,
            Saturation,
            Contrast,
            Reset,
        }

        public PostProcessingBehaviour postProcessingBehaviour { get; private set; }

        private void Start()
        {
            postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
        }

        private void SetColorGrading(ColorGradingBasicSettingElement element) {

            switch (element) {
                case ColorGradingBasicSettingElement.Temperature:
                    StartCoroutine(CorSetColorGrading(1f,100f));
                    break;
                case ColorGradingBasicSettingElement.Tint:
                    StartCoroutine(CorSetColorGrading(1f, 0f));
                    break;
            }
        }


        /// <summary>
        /// 해당 값으로 변경은 되나 100 에서 0으로 가는 것 안됌.
        /// </summary>
        /// <returns>The set color grading.</returns>
        /// <param name="delay">Delay.</param>
        /// <param name="destination">Destination.</param>
        IEnumerator CorSetColorGrading(float delay,float destination) {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            var settings = postProcessingBehaviour.profile.colorGrading.settings;

            float deltaTime = 0;
            //초기값 설정
            float value = settings.basic.temperature;
            while(true) {
                yield return wait;
                deltaTime += Time.deltaTime;
                //수정 필요
                value = Mathf.Clamp(((deltaTime * destination) / delay), -100f, 100f);
                //Debug.Log("deltaTime = " + deltaTime + "     value = " + value);
                settings.basic.temperature = value;
                postProcessingBehaviour.profile.colorGrading.settings = settings;
                //탈출
                if (deltaTime >= delay) {
                    yield break;
                }
            }
        }

        public void DoHighTemperatureEffect()
        {
            SetColorGrading(ColorGradingBasicSettingElement.Temperature);
        }

        public void Reset() 
        {
            SetColorGrading(ColorGradingBasicSettingElement.Reset);
        }


        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 100), "A")) {
                DoHighTemperatureEffect();
            }

            if (GUI.Button(new Rect(0, 100, 200, 100), "B"))
            {
                Reset();
            }
        }
    }
}
