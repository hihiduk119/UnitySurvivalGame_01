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

    public class Turret : MonoBehaviour
    {
        //MVC Pattern
        public TurretModel model = new TurretModel();
        public TurretView view = new TurretView();
        public TurretController controller = new TurretController();


        //IEnumerator WaitAndDo(float time, Action action)
        //{
        //    yield return new WaitForSeconds(time);
        //    action();
        //}

        /// <summary>
        /// 초기화하기
        /// </summary>
        public void Initialized()
        {
            //StartCoroutine(
            //    WaitAndDo(3f, () => {

            //}));
            Debug.Log(this.model.TurretType.ToString());
            //현재 트랜스 폼의 하위 모든 자식들을 tfAllChildList 에 찾아 넣음
            List<Transform> tfAllChildList = new List<Transform>(this.transform.GetComponentsInChildren<Transform>());
            //뷰의 최상위 오브젝트를 설정
            this.view.obj = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals(this.model.TurretType.ToString()))].gameObject;

            //그외 모든 터랫 디스에이블
            Transform tfRotation = this.view.obj.transform.parent;
            for (int i = 0; i < tfRotation.childCount; i++) { tfRotation.GetChild(i).gameObject.SetActive(false); }
            //해당 터랫 활성화
            this.view.obj.SetActive(true);
            //바디 부분 뷰에 세팅
            this.view.body = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Body"))];
            //헤드 부분 뷰에 세팅
            this.view.head = tfAllChildList[tfAllChildList.FindIndex(value => value.name.Equals("Head"))];
        }

        /// <summary>
        /// 실제 메모리 할당
        /// </summary>
        //protected void Alloc()
        //{
        //    this.model = new TurretModel();
        //    this.view = new TurretView();
        //    this.controller = new TurretController();
        //}

    }

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

    public class TurretView : ITurretView 
    {
        public GameObject obj;
        public Transform head;
        public Transform body;
    }

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
        public void TakeDamage()
        {

        }
        #endregion
    }
}
