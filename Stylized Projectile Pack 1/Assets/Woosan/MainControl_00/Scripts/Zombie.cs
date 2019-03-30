using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Woosan.SurvivalGame
{
    /// <summary>
    /// 좀비를 컨트롤 한다.
    /// 좀비 체력 및 타격 트리깅 발생
    /// </summary>
    public class Zombie : MonoBehaviour
    {
        public class ZombieModel {
            public int MaxHP = 100;
            public int HP = 100;
            public bool isDead = false;
        }

        //좀비의 내부 데이터
        public ZombieModel model = new ZombieModel();
        HpBar hpBar;
        RagdollController ragdollController;

        Coroutine corDie;

        private void Awake()
        {
            hpBar = GetComponent<HpBar>();
            ragdollController = GetComponentInChildren<RagdollController>();
        }

        public void Reset()
        {
            //모델 데이터 초기화
            model = new ZombieModel();
            //UI 초기화
            hpBar.Reset();
            //에니메이션 컨트롤 및 Nav 초기화
            ragdollController.DisableRagdoll();
        }

        //projectileActor.cs에서 호출 되며 탄에 맞음을 표시
        public void Hit()
        {
            //Debug.Log("I'm hit!!");
            //실제 체력 계산 부분
            int tmpHP = model.HP;
            bool die = false;
            tmpHP -= 10;
            //Debug.Log("tmpHP = " + tmpHP);
            //좀비 HP가 0이먄 죽게 만들기
            if(tmpHP <= 0) {
                die = true;
                tmpHP = 0;
            }

            model.HP = tmpHP;

            //실제 체력 UI에 표시 
            float hp = (float)model.HP / (float)model.MaxHP;
            //hp UI활성화 및 값 세팅
            hpBar.Enable();
            hpBar.SetHp(hp);
            //Debug.Log(hp);
            //좀비가 죽었다면
            if (die) {
                Die();
            }
        }

        public void Die()
        {
            if (corDie != null) { StopCoroutine(corDie); }
            corDie = StartCoroutine(CorDie());
        }

        IEnumerator CorDie() 
        {
            //현재 좀비의 정보 제공 => 좀비 리스트에서 제거하라고 지시a
            Character.instance.TargetDead(transform);
            //데이터상 데드로 변경
            model.isDead = true;
            //레그돌 관련 활성화
            ragdollController.EnableRagdoll();
            //리지드 바디에 포스 전달
            ragdollController.Die();
            //UI 비활성화
            //hpBar.Disable();
            yield return new WaitForSeconds(4f);
            //공중에 떠서 사라지는 연출
            ragdollController.Recall();
            yield return new WaitForSeconds(4f);
            //모든것 리셋 
            Reset();
            //ragdollController.DisableRagdoll();
        }
    }
}
