using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WoosanStudio.Common;
namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 총구 화염 이펙트를 컨트롤
    /// BlobLightProjector를 컨트롤함
    /// LWRP에서는 사용 불가.
    /// 스탠다드에서만 사용가능.
    /// </summary>
    public class MuzzleEffectController : MonoSingleton<MuzzleEffectController>
    {
        //활성,비활성화 시킬 총구 이펙트
        public GameObject muzzleBlobLights;
        //총구 화염 유지 시간
        public float delay = 0.2f;

        //코루팀 저장용
        Coroutine corActive;

        private void Reset()
        {
            muzzleBlobLights.SetActive(false);
        }

        /// <summary>
        /// 화염 유지 시간 동안 활성화 시킨후 비활성화 시킴
        /// </summary>
        public void Active()
        {
            if (corActive != null) StopCoroutine(corActive);
            corActive = StartCoroutine(CorActive());
        }

        IEnumerator CorActive()
        {
            muzzleBlobLights.SetActive(true);
            WaitForSeconds WFS = new WaitForSeconds(delay);
            yield return WFS;
            muzzleBlobLights.SetActive(false);
        }
    }
}
