using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WoosanStudio.Turret
{
    public enum TurretType {
        MachineGun = 0,             //기관총           물리/일반
        HeavyGun,               //단거리 권총       물리/해비
        LongRangeGun,           //스나이퍼          물리/관통
        //ExplosionGun,         //폭발탄           스플레쉬
        //LaserGun,             //레이저           레이저
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
        int level;

        public TurretType TurretType { get { return turretType; } set { turretType = value; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public float Damage { get { return damage; } set { damage = value; } }
        public int Level { get { return level; } set { level = value; } }

        public void SetData(int hp, float damage, int level, TurretType turretType)
        {
            this.hp = hp;
            this.damage = damage;
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

        public GameObject RootObject { get { return rootObject; } set { rootObject = value; } }
        public GameObject TypeObject { get { return typeObject; } set { typeObject = value; } }
        public Transform Head { get { return head; } set { head = value; } }
        public Transform Body { get { return body; } set { body = value; } }
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
