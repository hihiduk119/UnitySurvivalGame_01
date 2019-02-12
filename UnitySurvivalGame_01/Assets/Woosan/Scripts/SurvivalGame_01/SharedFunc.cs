using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SharedFuncManager {
	public static SharedFunc instance = new SharedFunc();
	public static SharedFunc Instance{ get{ if (instance == null) instance = new SharedFunc(); return instance; } }
}

public class SharedFunc {
	//언더바가 존재하는 스트링에서 언더바를 공백으로 전환시켜서 리턴
	public static string GetStringOnUnderbarExcepted(string frontText,string text,string EndText) {
		char[] param = new char[]{'_'};
		StringBuilder sb = new StringBuilder ();

		int a = text.IndexOf("_S_");
		if(a != -1) {
//			Debug.Log("a = " + a + " text = " + text);
			text = text.Remove(a,3);
			text = text.Insert(a,"'s ");
		}

		string[] accArr = text.Split(param);
		sb.Append(frontText);
		for(int index = 0; index < accArr.Length;index++) {
			sb.Append(accArr[index]);
			sb.Append(' ');
		}
		sb.Remove(sb.Length-1,1);
		sb.Append(EndText);
		return sb.ToString();
	}

	public string GetStringToStringArr(ref string[] strArr) {
		StringBuilder sb = new StringBuilder ();
		for(int index = 0; index < strArr.Length;index++) { sb.Append(strArr[index]);	}
		return sb.ToString();
	}

	public string GetStringToStringArr(ref List<string> strList) {
		StringBuilder sb = new StringBuilder ();
		for(int index = 0; index < strList.Count;index++) { sb.Append(strList[index]);	}
		return sb.ToString();
	}

	//(0,4,4)이면 4개 생성
	public static List<int> GetNoOverlapRandomValue(int min , int max,int count) {
		List<int> list = new List<int>();
		int value;

		for(int i = 0; i < count;i++) {
			while(true) {
				value = UnityEngine.Random.Range(min,max);
				if(!list.Exists(s => {
					if(s.Equals(value))
						return true;
					else 
						return false;
				}))
					break;
			}

			list.Add(value);
		}

		return list;
	}

	//비트 마스크를 이용하여 값 추가 삭제 및 존재 확인
	public static void AddBitmask<T>(ref int target,T bitmask) {
		target |= (int)(object)bitmask;
	}

	public static void RemoveBitmask<T>(ref int target,T bitmask) {
		target &= ~(int)(object)bitmask;
	}

	public static bool BitmaskExists<T>(int target,T bitmask) {
		int tmpMask = target & (int)(object)bitmask;
		if(tmpMask == (int)(object)bitmask)  return true; 
		else return false;
	}


	public static T StringToEnum<T> (string text) {
		return (T)System.Enum.Parse(typeof(T),text.ToUpper());
	}

	public static T StringToEnumNoUpper<T> (string text) {
		return (T)System.Enum.Parse(typeof(T),text);
	}

	public static string MakeUniqueID_ByTime() {
		return System.DateTime.UtcNow.Minute +""+ System.DateTime.UtcNow.Second+""+System.DateTime.UtcNow.Millisecond;
	}


	//3D캠위치를 2D로 변환
	public Vector3 ThreeD_CamToTwoD_CamConverter(Camera cam3d,Camera camUI, Vector3 pos) {
		pos = cam3d.WorldToViewportPoint(pos);
		pos = camUI.ViewportToWorldPoint(pos);
		return pos;
	}

	public static string GetName(string name) {//3항 연산자 연습
		return name != null ? name : "NA";//이름이 널이면 “NA” 가 리턴 됨
	}
}
