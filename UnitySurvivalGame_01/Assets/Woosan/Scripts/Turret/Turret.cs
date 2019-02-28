
using System.Collections.Generic;
using UnityEngine;


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
    /// 터렛의 모든 공용 기능을 가짐
    /// </summary>
    public class Turret : MonoBehaviour
    {
        //MVC Pattern
        public TurretModel model = new TurretModel();
        public TurretView view = new TurretView();
        public TurretController controller = new TurretController();

        #region 코루틴 람다용
        //IEnumerator WaitAndDo(float time, Action action)
        //{
        //    yield return new WaitForSeconds(time);
        //    action();
        //}

        //StartCoroutine(
        //    WaitAndDo(3f, () => {

        //}));
        # endregion
        /// <summary>
        /// 초기화하기
        /// </summary>
        public void Initialized()
        {
            //현재 트랜스 폼의 하위 모든 자식들을 tfAllChildList 에 찾아 넣음
            List<Transform> tfAllChildList = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
            //뷰의 최상위 오브젝트를 설정
            this.view.TypeObject = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals(this.model.TurretType.ToString()))].gameObject;

            //그외 모든 터랫 비활성화
            Transform tfRotation = this.view.TypeObject.transform.parent;
            for (int i = 0; i < tfRotation.childCount; i++) { tfRotation.GetChild(i).gameObject.SetActive(false); }
            //해당 터랫만 활성화
            this.view.TypeObject.SetActive(true);
            //바디 부분 뷰에 세팅
            this.view.Body = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Body"))];
            //헤드 부분 뷰에 세팅
            this.view.Head = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Head"))];
            //사거리 세팅하는 부분
            this.view.Range = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Range"))].GetComponent<Range>();
            this.view.Range.SetRange(model.Range);
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
        private Transform head;
        private Transform body;
        private Range range;

        public GameObject RootObject { get { return rootObject; } set { rootObject = value; } }
        public GameObject TypeObject { get { return typeObject; } set { typeObject = value; } }
        public Transform Head { get { return head; } set { head = value; } }
        public Transform Body { get { return body; } set { body = value; } }
        public Range Range { get { return range; } set { range = value; } }
    }

    /// <summary>
    /// 모든 Action 관련 기능만 가짐
    /// </summary>
    public class TurretController : ITurretController
    {
        #region ITurretController 
        /// <summary>
        /// 사격
        /// </summary>
        public void Fire()
        {

        }
        /// <summary>
        /// 경계
        /// </summary>
        public void Guard()
        {

        }
        /// <summary>
        /// 데미지를 받음
        /// </summary>
        public void TakeDamage(TurretModel turretModel)
        {

        }
        #endregion
    }
}
