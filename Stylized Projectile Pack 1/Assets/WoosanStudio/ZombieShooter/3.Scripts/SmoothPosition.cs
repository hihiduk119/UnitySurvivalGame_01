using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.ZombieShooter
{
	public class SmoothPosition : MonoBehaviour
	{
		private void FixedUpdate()
		{
			//네비 매쉬가 캐릭터 컨트롤러로 부터 떨어지는 것을 방지하기 위함
			//네비 매쉬와 캐릭터 컨트롤러를 함께 쓸수가 없어서 트랜스 폼을 따로 했을때 충돌체에 부칮힐때 자식 오브젝트가 부모와 멀어지는 현상을
			//개선하기 위한 코드
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10f);
		}
	}
}
