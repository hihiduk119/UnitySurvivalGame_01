using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// Json 불러오기용
    /// </summary>
    public class JsonLoad : MonoBehaviour
    {
        [Header("[로드된 무기 데이터]")]
        public Weapons weapons;

        private void Awake()
        {
            //최초 불러오기
            Load();
        }

        /// <summary>
        /// Json파일 로드
        /// </summary>
        public void Load()
        {
            string json = PlayerPrefs.GetString("Weapons");
            Debug.Log("Load = " + json);
            weapons = JsonUtility.FromJson<Weapons>(json);

        }
    }
}
