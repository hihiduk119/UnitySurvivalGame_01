using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoosanStudio.Common;

namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 타겟에 에임을 설정하는 부분
    /// </summary>
    public class AimMaker : MonoBehaviour,WoosanStudio.Common.Object
    {
        //기본 되는 표적 포지션
        readonly private Vector3 defaultPosition = new Vector3(0f, 0.07f, 0f);
        readonly private Vector3 defaultRotation = new Vector3(90f, 0, 0);
        //3가지 종류 표적 사이즈 [ZombieKinds와 인덱스가 같다]
        readonly private List<Vector3> scales = new List<Vector3>() { new Vector3(1.2f, 1.2f, 1.2f), new Vector3(1.5f, 1.5f, 1.5f), new Vector3(2f, 2f, 2f) };

        //실제 타겟 오브젝트
        public Transform model;

        private void Awake()
        {
            Reset();
        }

        /// <summary>
        /// 초기화 코드
        /// </summary>
        public void Reset()
        {
            //해당 타겟 모델 디스에이븕
            model.gameObject.SetActive(false);
        }


        /// <summary>
        /// 타겟을 설정한다.
        /// </summary>
        /// <param name="target"></param>
        public void SetValue(Transform target)
        {
            //이미 타겟 되어있다면 종료
            if (model.transform.parent.Equals(target)) return;

            model.transform.parent = target;
            model.transform.localPosition = defaultPosition;
            model.transform.localRotation = Quaternion.Euler(defaultRotation);
            //추후 몬스터의 크기에 따라서 스케일 이 바뀌어야 한다
            model.transform.localScale = scales[0];
        }
    }
}
