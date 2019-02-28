using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Turret
{
    public class HeavyGunTurret : Turret
    {
        //해당 테이블은 제이슨 데이터로 로드해서 사용하자
        //이것은 임시로 만듬
        private readonly int[] damageTable = new int[] { 5, 6, 7, 8, 9 };
        private readonly int[] hpTable = new int[] { 100, 105, 110, 115, 120 };

        private void Awake()
        {
            this.Alloc();
        }

        public void Alloc(int level = 1)
        {
            base.model.SetData(this.hpTable[level], this.damageTable[level], level, TurretType.HeavyGun);
        }
    }
}
