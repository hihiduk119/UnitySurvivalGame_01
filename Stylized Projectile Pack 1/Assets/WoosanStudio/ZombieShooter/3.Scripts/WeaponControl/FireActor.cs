using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using WoosanStudio.Common;

namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 몬스터가 영역에 들어왔을때 사격 통제
    /// 모든 사격 관련 부분을 컨트롤
    /// 실제 발사체 컨트롤도 여기서함.
    /// </summary>
    public class FireActor : MonoBehaviour 
    {
        /// <summary>
        /// 사격 상태
        /// </summary>
        public enum State
        {
            FireAble,   //사격 가능
            Firing,     //사격 중
            Reloading,  //재장전중
            CasingJam,  //탄 걸림
        }

        [Header("[사격할 타겟들]")]
        public List<Transform> targets = new List<Transform>();
        [Header ("[플레이어 트렌스폼]")]
        public Transform player;
        [Header ("[발사체 컨트롤 및 관력 이펙트]")]
        public projectileActor m_projectileActor;
        [Header("[타겟의 바닥에 생기는 조준 마커]")]
        public AimMaker aimMaker;

        //저장되어 있는 무기 리스트 [현재 아무것도 없음 json을 데이터 로드 구현 해야함]
        private List<Weapon> weapons = new List<Weapon>();
        private Weapon currentWeapon = null;
        //현재 타게팅된 오브젝트
        private Transform currentTarget;

        //업데이트를 대신할 코루틴
        Coroutine corTargetCheck;

        //타겟팅이 발생했을때 발생하는 이벤트
        public UnityEvent targetingEvent;

        //캐쉬용
        GameObject fireTarget;



        private void Start()
        {
            //코루틴으로 업데이트를 대신한다
            //corTargetCheck = StartCoroutine(CorTargetCheck());
        }

        /// <summary>
        /// 매 프레임 사격 가능 여부를 확인
        /// </summary>
        void FireControl()
        {
            
        }

        /// <summary>
        /// 몬스터 사격 가능여부 확인
        /// </summary>
        bool FireAble()
        {
            return false;
        }

        /// <summary>
        /// 사격 중인지 확인
        /// </summary>
        /// <returns></returns>
        bool IsFire()
        {
            return false;
        }


        /// <summary>
        /// 해당 타겟에 사격
        /// </summary>
        void Fire(GameObject target)
        {

        }

        /// <summary>
        /// projectileActor에서 발사체 발사 통보
        /// </summary>
        public void FireProjectile()
        {
            //총 종류에 따른 다름 화염 연출 부분 필요
            //To Do..
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

        /// <summary>
        /// 무기를 해당 인덱스로 변경
        /// </summary>
        /// <param name="index"></param>
        public void SwitchWeapon(int index)
        {
            //해당 인덱스로 현재 무기를 세팅하는 부분이 필요함
            //To Do..

            //프로젝타일 엑터에서 무기 변환 [실제 발사체,총구화염,충돌 이펙트를 담당]
            m_projectileActor.Switch(index);
        }

        /// <summary>
        /// 타겟팅이 바뀌었는지 확인하는 부분
        /// </summary>
        /// <returns>바뀐게 없거나 타겟이 없다면 null 리턴</returns>
        Transform IsChangeTarget()
        {
            return null;
        }

        //업데이트 대신 사용
        IEnumerator CorTargetCheck()
        {
            WaitForSeconds WFS = new WaitForSeconds(0.1f);
            while (true)
            {
                //타겟이 바뀌었는지 확인
                currentTarget = IsChangeTarget();
                //타겟이 바뀠다면 재설정
                if (currentTarget != null)
                {
                    //마커 재설정
                    aimMaker.SetValue(currentTarget);
                }

                Debug.Log("0");
                //현재 사격 중인 확인
                //if (!IsFire()) break;
                //사격 가능 여부 확인
                //if (!FireAble()) break;
                //제일 가까운 오브젝트
                fireTarget = TargetUtililty.GetNearestTarget(targets,transform).gameObject;

                if (fireTarget != null)
                {
                    Debug.Log("[" + this.name + "]" + fireTarget.name);
                }

                //우선 순위 타겟에 사격
                Fire(fireTarget);

                //매 프레임 사격 가능 여부를 확인
                //FireControl();

                yield return WFS;
            }
        }

        //타겟 추가 됫는지 아닌지 확인용 테스트 코드
        /*private void Update()
        {
            //디버그용 타겟 체커
            if (GameManager.Instance.onDebug.OnDebugFireActor)
            {
                if (targets.Count == 0) return;

                if(GameManager.Instance.onDebug.OnDebugFireActor)
                {
                    //Debug.Log("[" + this.name + "] 타겟 수 = " + targets.Count);
                }
                
                //타겟에 저장된 모든 오브젝트에 라인그려서 표시
                for (int index = 0; index < targets.Count; index++)
                {
                    Debug.DrawLine(transform.position, targets[index].transform.position, Color.red);
                }

                Debug.DrawLine(transform.position, TargetUtililty.GetNearestTarget(targets, transform).position, Color.yellow);
            }
        }*/
    }
}
