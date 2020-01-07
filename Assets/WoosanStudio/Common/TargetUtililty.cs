using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WoosanStudio.Common
{
    public class TargetUtililty
    {
        /// <summary>
        /// 제일 가까운 오브젝트 가져옴
        /// </summary>
        /// <param name="targets">가까운 녀석을 찾을 오브젝트들이 들어있는 리스트</param>
        /// <param name="me">비교할 내위치</param>
        /// <returns></returns>
        static public Transform GetNearestTarget(List<Transform> targets, Transform me)
        {
            List<float> distanceList = new List<float>();

            //두사이 거리를 distanceList에 집어 넣기
            for (int index = 0; index < targets.Count; index++)
            {
                distanceList.Add(Vector3.Distance(me.position, targets[index].position));
            }

            float minDistance = distanceList.Min();

            //Debug.Log("[TargetUtililty]" + targets[distanceList.FindIndex(value => value.Equals(minDistance))].name);

            return targets[distanceList.FindIndex(value => value.Equals(minDistance))];
        }
    }
}
