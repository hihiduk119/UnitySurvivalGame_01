using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Turret {
    public static class TurretFactory
    {
        public static Turret GetTurret(TurretType turretType,GameObject gameObject) 
        {
            switch(turretType) {
                case TurretType.MachineGun:
                    gameObject.AddComponent<MachineGunTurret>(); 
                    return gameObject.GetComponent<MachineGunTurret>();
                case TurretType.HeavyGun:
                    gameObject.AddComponent<HeavyGunTurret>();
                    return gameObject.GetComponent<HeavyGunTurret>();
                case TurretType.LongRangeGun:
                    gameObject.AddComponent<LongRangeGunTurret>();
                    return gameObject.GetComponent<LongRangeGunTurret>();
                default:
                    gameObject.AddComponent<MachineGunTurret>();
                    return gameObject.GetComponent<MachineGunTurret>();
            }
        }
    }
}
