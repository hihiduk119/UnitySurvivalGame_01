using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using WoosanStudio.Common;
namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 게임 최상위 컨트롤러
    /// [역활]
    /// 해당 씬으로 이동
    /// 씬 단위의 컨트롤러 제어
    /// main함수 역활
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        static public GameManager Instance;
        [Serializable]
        public class OnDebug
        {
            [Header ("[Range.cs 디버그 모드 활성화]")]
            public bool OnDebugForRange = true;
            [Header("[FireActor.cs 디버그 모드 활성화]")]
            public bool OnDebugFireActor = true;
            [Header("[Character.cs 디버그 모드 활성화]")]
            public bool OnDebugCharacter = true;
        }

        [Header("[디버그 전용 클래스]")]
        public OnDebug onDebug = new OnDebug();

        private void Awake()
        {
            Instance = this;

            Application.targetFrameRate = 60;
        }
    }
}
