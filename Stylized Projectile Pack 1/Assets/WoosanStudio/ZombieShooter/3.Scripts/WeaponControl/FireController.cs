using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 몬스터가 영역에 들어왔을때 사격 통제
    /// 모든 사격 관련 부분을 컨트롤
    /// 실제 발사체 컨트롤도 여기서함.
    /// </summary>
    public class FireController : MonoBehaviour
    {
        /// <summary>
        /// 사격 우선순위
        /// </summary>
        public enum Priority
        {
            LowHealth,      //낯은 체력 우선
            MinDistance,    //최소 거리 우선
            MaxDistance,    //최대 거리 우선
        }

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

        [Header("[몬스터 타겟들]")]
        public List<Transform> targets = new List<Transform>();
        [Header ("[플레이어 트렌스폼]")]
        public Transform player;
        [Header ("[발사체 컨트롤 및 관력 이펙트]")]
        public projectileActor m_projectileActor;
        [Header("[총구 불꽃 프로젝터 컨트롤]")]


        //저장되어 있는 무기 리스트 [현재 아무것도 없음 json을 데이터 로드 구현 해야함]
        private List<Weapon> weapons = new List<Weapon>();
        private Weapon currentWeapon = null;

        /// <summary>
        /// 매 프레임 사격 가능 여부를 확인
        /// </summary>
        void FireControl()
        {
            //현재 사격 중인 확인
            if (!IsFire()) return;
            //사격 가능 여부 확인
            if (!FireAble()) return;
            //사격 우선 순위 타겟 확인
            GameObject target = FindPriorityTarget();
            //우선 순위 타겟에 사격
            Fire(target);
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
        /// 최우선 타겟을 가져온다
        /// </summary>
        /// <returns></returns>
        GameObject FindPriorityTarget()
        {
            return null;
        }


        /// <summary>
        /// 몬스터 사격
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

        void FixedUpdate()
        {
            //매 프레임 사격 가능 여부를 확인
            FireControl();
        }
    }
}
