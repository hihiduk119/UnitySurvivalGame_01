using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoosanStudio.Player
{
    /// <summary>
    /// 작성자 : 애드 마
    /// 버전 : 1.0.0 [2019.11.12]
    /// 조이스틱을 이용해서 플레이어를 컨트롤 하기 위한 스크립트
    /// 캠 트랜스 폼을 받아서 화면 이동 보간 기능
    /// 
    /// </summary>
    public class Move : MonoBehaviour
    {
        [Header("[움지임 파워 조절]")]
        public float power = 5f;
        [Header("[해당 방향으로 움직임 보간]")]
        public new Transform camera;

        ///캐슁용 변수들
        //버티컬
        float v;
        //호라이즌
        float h;
        //캠의 방향 벡터
        private Vector3 camForward;
        //캠의 방향 벡터를 더한 최종 벨로시티
        private Vector3 desiredVelocity;
        //캐슁용 리지드바디
        private new Rigidbody rigidbody;

        void Start()
        {
            //내 자신의 리지드바디 캐슁
            rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            //조이스틱 방향 가져오기
            h = UltimateJoystick.GetHorizontalAxis("Move");
            v = UltimateJoystick.GetVerticalAxis("Move");

            if (camera != null)
            {
                //카메라 기준으로 조이스틱 방향성 바꿔줌
                camForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;

                //벡터 연산은 x , y 를 따로 대입하지 않고 덧셈으로 가능하다
                desiredVelocity = v * camForward * power + h * camera.right * power;
                //위 연산은 아래 연산과 같다
                /*  desiredVelocity.z = v * camForward * power;
                    desiredVelocity.x = h * camera.right * power; */
            }
            else
            {
                //카메라가 없다면 기본 방향
                desiredVelocity = v * Vector3.forward * power + h * Vector3.right * power;
            }

            //최종 값을 리지드 바디에 더한다.
            rigidbody.velocity = desiredVelocity;
        }
    }
}
