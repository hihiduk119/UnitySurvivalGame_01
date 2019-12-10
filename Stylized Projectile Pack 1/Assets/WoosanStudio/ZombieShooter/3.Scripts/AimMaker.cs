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

        [Header("[타겟들]")]
        public List<Transform> targets = new List<Transform>();
        [Header("[조준된 타겟]")]
        public Transform aimedTarget = null;

        //실제 타겟 오브젝트
        public Transform model;

        //업데이트 대신 사용. 타겟 체크용
        Coroutine corTargetCheck;

        private void Awake()
        {
            Reset();
        }

        private void Start()
        {
            //코루틴으로 업데이트를 대신한다
            corTargetCheck = StartCoroutine(CorTargetCheck());
        }

        

        //업데이트 대신 사용
        IEnumerator CorTargetCheck()
        {
            WaitForSeconds WFS = new WaitForSeconds(0.1f);
            while (true)
            {
                //타겟들이 있다면
                if(targets.Count > 0) {
                    //가장 가까운 타겟 가져오기
                    aimedTarget = TargetUtililty.GetNearestTarget(targets, transform);

                    //if (aimedTarget != null)
                    //{
                    //    Debug.Log("[" + this.name + "]" + aimedTarget.name);
                    //}

                    //활성화
                    model.gameObject.SetActive(true);
                    //타겟 마커 세팅
                    SetValue(aimedTarget);

                    
                } else
                {
                    //비활성화
                    model.gameObject.SetActive(false);

                    aimedTarget = null;
                }

                yield return WFS;
            }
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

        /// <summary>
        /// 타겟 추가
        /// </summary>
        /// <param name="target">추가할 타겟</param>
        public void AddTarget(Transform target)
        {
            //리스트에서 기존에 있는지 없는지 확인[없다]
            if (!targets.Find(value => value.Equals(target.name)))
            {
                //없다면 추가
                targets.Add(target);
            }
        }

        /// <summary>
        /// 타겟 제거
        /// </summary>
        /// <param name="target">제거할 타겟</param>
        public void RemoveTarget(Transform target)
        {
            //리스트에서 기존에 있는지 없는지 확인
            if (targets.Find(value => value.name.Equals(target.name)))
            {
                //있다면 제거
                targets.RemoveAt(targets.FindIndex(value => value.name.Equals(target.name)));
            }
        }
    }
}
