using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
