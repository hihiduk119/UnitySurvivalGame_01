using UnityEngine;

namespace WoosanStudio.Turret
{
    /// <summary>
    /// 터렛 모델 인터페이스
    /// </summary>
    public interface ITurretModel
    {
        /// <summary>
        /// 터렛의 형태 구분
        /// </summary>
        /// <value>The type of the turret.</value>
        TurretType TurretType { get; set; }
        /// <summary>
        /// 터렛 체력
        /// </summary>
        /// <value>The hp.</value>
        int Hp { get; set; }
        /// <summary>
        /// 터렛 데미지
        /// </summary>
        /// <value>The damage.</value>
        float Damage { get; set; }
        /// <summary>
        /// 터렛 레벨
        /// </summary>
        /// <value>The level.</value>
        int Level { get; set; }

        /// <summary>
        /// 데이터 세팅하는 부분
        /// </summary>
        /// <param name="hp">Hp.</param>
        /// <param name="damage">Damage.</param>
        /// <param name="level">Level.</param>
        /// <param name="turretType">Turret type.</param>
        void SetData(int hp,float damage,int level, TurretType turretType);
    }

    /// <summary>
    /// 터렛 뷰 인터페이스
    /// </summary>
    public interface ITurretView
    {
        //void SetObject(TurretType turretType);
        //최상위 오브젝트
        GameObject RootObject { get; set; }
        //타입으로 결정된 오브젝트
        GameObject TypeObject { get; set; }
        //바디 부분 트랜스 폼
        Transform Body { get; set; }
        //헤드부분 트랜스 폼
        Transform Head { get; set; }
    }

    /// <summary>
    /// 터렛 컨트롤 인터페이스
    /// </summary>
    public interface ITurretController
    {
        /// <summary>
        /// 사격
        /// </summary>
        void Fire();

        /// <summary>
        /// 경계
        /// </summary>
        void Guard();

        /// <summary>
        /// 데미지를 받음
        /// </summary>
        void TakeDamage();
    }
}
