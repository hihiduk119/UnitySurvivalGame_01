
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Turret
{
    /// <summary>
    /// 터렛 메니저는 해당 터렛의 전체 수량을 조절한다.
    /// 실제는 메모리 풀 역활을 한다
    /// </summary>
    public class TurretManager : MonoBehaviour
    {
        public GameObject pfTurret;
        public Transform turretRoot;
        public int maxTurret = 3;
        public List<GameObject> turretList = new List<GameObject>();
        public int turretIndex = 0;

        private void Awake()
        {
            this.MakeTurret();
        }

        /// <summary>
        /// 실제 터렛을 생성 하는 부분
        /// </summary>
        void MakeTurret()
        {
            GameObject clone;
            for (int i = 0;  i < this.maxTurret;i++) {
                clone = Instantiate(this.pfTurret);
                this.turretList.Add(clone);
                clone.SetActive(false);
                clone.transform.parent = this.turretRoot;
                clone.transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// 생성된 터렛을 액티브 시키는 부분
        /// </summary>
        void CreateTurret() 
        {
            //머신건 하나 활성화
            this.turretList[turretIndex].SetActive(true);
            Turret turret = TurretFactory.GetTurret(TurretType.MachineGun, this.turretList[turretIndex]);
            turret.Initialized();
            turretIndex++;

            //머신건 하나 활성화
            this.turretList[turretIndex].SetActive(true);
            turret = TurretFactory.GetTurret(TurretType.LongRangeGun, this.turretList[turretIndex]);
            turret.Initialized();
            turretIndex++;

            //머신건 하나 활성화
            this.turretList[turretIndex].SetActive(true);
            turret = TurretFactory.GetTurret(TurretType.HeavyGun, this.turretList[turretIndex]);
            turret.Initialized();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 150), "터셋 생성"))
            {
                this.CreateTurret();
            }
        }
    }
}
