using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// Json 저장용
    /// </summary>
    public class JsonSave : MonoBehaviour
    {
        //저장할 무기 데이터
        public Weapons weapons = new Weapons();

        [Header("[추가할 무기 세팅]")]
        public Weapon weapon = new Weapon("권총",1,10f,0,new BulletClip(10));

        //무기 추가
        public void AddWeapon()
        {
         //   weapons.weapons.Add((Weapon)weapon.Clone());
        }

        /// <summary>
        /// content를 제이슨 파일로 저장
        /// </summary>
        public void Save()
        {
            string json = JsonUtility.ToJson(weapons);
            Debug.Log("Save = " + json);
            PlayerPrefs.SetString("Weapons", json);
        }
    }
}
