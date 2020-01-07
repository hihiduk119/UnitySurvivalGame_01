using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 무기 데이터를 정의함
    /// </summary>
    [Serializable]
    public class Weapon
    {
        /// <summary>
        /// 속
        /// </summary>
        public enum Property
        {
            Cannon,     //기본
            Lazer,      //레이저
            Electric,   //전기
            Plasma,     //플라즈마
            Gauss,      //가우 ? Gamma
        }

        public enum Type
        {
            Pistol,         //원샷    ,장탄수 적음     ,발사속도 평균    ,리로딩 빠름
            ShotGun,        //멀티샷   ,장탄수 적음     ,발사속도 느린    ,리로딩 느림
            SubmachineGun,  //원샷    ,장탄수 많음     ,발사속도 매우 빠름 ,리로딩 평균
            AssaultRifle,   //원샷    ,장탄수 많음     ,발사속도 매우 빠름 ,리로딩 평균
            Rifle,          //원샷    ,장탄수 매우 많음  ,발사속도 매우 느림,리로딩 느림
        }

        //이름
        public string name;
        //고유아이디
        public int id;
        //무기 데미지
        public float demage;
        //projectileActor에서 사용된 projectile의 인덱스
        public int projectileIndex;

        private BulletClip bulletClip;


        //추가적으로 속성이 업데이트 됬을때를 위한 데이터 셋
        Dictionary<string, string> extraProperty;

        public Weapon(string name, int id, float demage, int projectileIndex, BulletClip bulletClip)
        {
            this.name = name;
            this.id = id;
            this.demage = demage;
            this.projectileIndex = projectileIndex;
            this.bulletClip = bulletClip;
        }

        public BulletClip BulletClip { get => bulletClip; set => bulletClip = value; }
    }

    [Serializable]
    public class Weapons
    {
        //모든 무기를 저장할 데이터
        public List<Weapon> weapons = new List<Weapon>();
    }
}
