using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WoosanStudio.Turret
{
    public enum TurretType {
        MachineGun = 0,             //기관총           물리/일반
        HeavyGun,               //단거리 권총       물리/해비
        LongRangeGun,           //스나이퍼          물리/관통
        //ExplosionGun,         //폭발탄           스플레쉬
        //LaserGun,             //레이저           레이저
    }

    public enum TargetPriority {
        Closest,            //가까운것 부터
        MaxHp,              //최대 체력이 가장 많은 녀석 부터
    }

    /// <summary>
    /// 터렛의 모든 공용 기능을 가짐. 이게 -> 컨트롤러의 역활이며 프리젠터이며 뷰모델이기도 하다.
    /// </summary>
    public class Turret : MonoBehaviour //, ITurret
    {
        //MVVM Pattern 
        public TurretModel model = new TurretModel();
        public TurretView view = new TurretView();

        #region 적 출현시 사용
        //
        //룩엣 사용시 쳐다볼 타겟
        public Transform lookAtTarget = null;
        //룩엣 사용시 쳐다보기 코루틴
        Coroutine corLookAtTarget;
        Animator animatorHeadRot;

        #endregion

        #region 코루틴 람다용
        //IEnumerator WaitAndDo(float time, Action action)
        //{
        //    yield return new WaitForSeconds(time);
        //    action();
        //}

        //StartCoroutine(
        //    WaitAndDo(3f, () => {

        //}));
        #endregion
        /// <summary>
        /// 초기화하기
        /// </summary>
        public void Initialized()
        {
            //현재 트랜스 폼의 하위 모든 자식들을 tfAllChildList 에 찾아 넣음
            List<Transform> tfAllChildList = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
            //뷰의 최상위 오브젝트를 설정
            this.view.TypeObject = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals(this.model.TurretType.ToString()))].gameObject;
            Debug.Log(this.model.TurretType.ToString());
            //레이어 터렛으로 모두 세팅
            tfAllChildList.ForEach(value => { value.gameObject.layer = LayerMask.NameToLayer("Turret"); });

            //그외 모든 터랫 비활성화
            Transform tfRotation = this.view.TypeObject.transform.parent;
            for (int i = 0; i < tfRotation.childCount; i++) { tfRotation.GetChild(i).gameObject.SetActive(false); }
            //해당 터랫만 활성화
            this.view.TypeObject.SetActive(true);

            //한번더 가져오는 이유는 처음에는 모든 터렛을 다 활성화 시켜놨기에 해당 터렛의 나머지 Body,Head 만 따로 가져오기 위해 다시 가져옴
            tfAllChildList = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
            //바디 부분 뷰에 세팅
            this.view.BodyPos = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Body"))];
            //바디 부분 뷰에 세팅
            this.view.BodyRot = this.view.BodyPos.GetChild(0);
            //헤드 부분 뷰에 세팅
            this.view.HeadPos = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Head"))];
            //헤드 부분 뷰에 세팅
            this.view.HeadRot = this.view.HeadPos.GetChild(0);
            this.animatorHeadRot = this.view.HeadRot.gameObject.AddComponent<Animator>();
            this.animatorHeadRot.runtimeAnimatorController = Resources.Load("Animator/TurretHead") as RuntimeAnimatorController;

            //사거리 세팅하는 부분
            this.view.Range = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Range"))].GetComponent<Range>();
            this.view.Range.SetRange(model.Range);
            this.view.Range.triggerEnterEvent.AddListener(LookAtTarget);
            this.view.Range.triggerExitEvent.AddListener(LookOut);
        }

        /// <summary>
        /// 룩엣 사용시 호출
        /// </summary>
        public void LookAtTarget(Transform target) 
        {
            //경계테세 해제하고 공격
            this.Fire(true);
            this.Guard(false);

            //현재 룩엣 세팅
            this.lookAtTarget = target;
            //쳐다보는 코루틴 생성 target이 없다면 죽음
            if (this.corLookAtTarget != null) StopCoroutine(this.corLookAtTarget);
            this.corLookAtTarget = StartCoroutine(CorLookAtTarget());
        }

        /// <summary>
        /// 테랫이 범위 벗어 났을때 호출
        /// </summary>
        public void LookOut()
        {
            //현재 룩엣 세팅
            this.lookAtTarget = null;
            view.HeadRot.DOLocalRotate(Vector3.zero, 0.2f).OnComplete(() =>
            {
                //경계테세 공격 중지
                this.Fire(false);
                this.Guard(true);
            });
        }

        IEnumerator CorLookAtTarget()
        {
            IEnumerator waitTime = new WaitForSecondsRealtime(0.1f);
            //트윈이 있다면 일단 초기화
             view.HeadRot.DOKill();
            while (this.lookAtTarget != null) {
                view.HeadRot.DOLookAt(this.lookAtTarget.position, 0.1f);
                Debug.Log(view.HeadRot.ToString());
                yield return waitTime;
            }
        }


        /// <summary>
        /// 사격
        /// </summary>
        void Fire(bool activate) 
        {

        }

        /// <summary>
        /// 경계
        /// </summary>
        void Guard(bool activate) 
        {
            this.animatorHeadRot.enabled = activate;
        }

        /// <summary>
        /// 데미지를 받음
        /// </summary>
        public void TakeDamage(TurretModel turretModel)
        {

        }
    }

    /// <summary>
    /// 모든 데이터만 가지고 있음
    /// </summary>
    public class TurretModel : ITurretModel 
    {
        TurretType turretType;
        int hp;
        float damage;
        float range;
        float atkSpd;
        int level;

        public TurretType TurretType { get { return turretType; } set { turretType = value; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public float Damage { get { return damage; } set { damage = value; } }
        public float Range { get { return range; } set { range = value; } }
        public float AtkSpd { get { return atkSpd; } set { atkSpd = value; } }
        public int Level { get { return level; } set { level = value; } }

        public void SetData(int hp, float damage, float range, float atkSpd, int level,TurretType turretType)
        {
            this.hp = hp;
            this.damage = damage;
            this.range = range;
            this.atkSpd = atkSpd;
            this.level = level;
            this.turretType = turretType;
        }
    }

    /// <summary>
    /// 모든 보여주는 것에 관련된 Object를 가짐
    /// </summary>
    public class TurretView : ITurretView 
    {
        private GameObject rootObject;
        private GameObject typeObject;
        private Transform headPos;
        private Transform headRot;
        private Transform bodyPos;
        private Transform bodyRot;
        private Range range;

        public GameObject RootObject { get { return rootObject; } set { rootObject = value; } }
        public GameObject TypeObject { get { return typeObject; } set { typeObject = value; } }
        public Transform HeadPos { get { return headPos; } set { headPos = value; } }
        public Transform HeadRot { get { return headRot; } set { headRot = value; } }
        public Transform BodyPos { get { return bodyPos; } set { bodyPos = value; } }
        public Transform BodyRot { get { return bodyRot; } set { bodyRot = value; } }
        public Range Range { get { return range; } set { range = value; } }
    }
}
