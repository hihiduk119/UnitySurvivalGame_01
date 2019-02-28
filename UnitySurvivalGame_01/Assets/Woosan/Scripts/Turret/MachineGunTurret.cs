using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Turret
{
    public class MachineGunTurret : Turret
    {
        //해당 테이블은 제이슨 데이터로 로드해서 사용하자
        //이것은 임시로 만듬
        private readonly float[] damageTable = new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
        private readonly int[] hpTable = new int[] { 100, 105, 110, 115, 120 };
        private readonly float range = 14;
        private readonly float atkSpd = 10f;

        private void Awake()
        {
            this.Create();
        }

        //이곳은 데이터만 세팅한다. 실제 적용은 아니다.
        public void Create(int level = 1)
        {
            base.model.SetData(this.hpTable[level], this.damageTable[level], this.range, this.atkSpd, level, TurretType.MachineGun);
        }
    }
}
