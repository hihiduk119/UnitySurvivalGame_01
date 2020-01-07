using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Common
{
    /// <summary>
    /// 스스로 디스에이블 시키는 오브젝트
    /// </summary>
    public class DisableMe : MonoBehaviour
    {
        float timer;
        private float disableTimer = 4f;

        private void OnEnable()
        {
            timer = 0;
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;

            if (timer >= disableTimer)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
