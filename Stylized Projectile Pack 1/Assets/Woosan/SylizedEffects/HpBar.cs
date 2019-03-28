using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject barPrefab;
    protected Image bar;
    protected Image barFilled;

    private void Awake()
    {
        //생성시 부모 바로 넣기
        bar = Instantiate(barPrefab, FindObjectOfType<Canvas>().transform).transform.GetChild(0).GetComponent<Image>();
        //리스트 배열을 바로 어레이로 넣기
        barFilled = new List<Image>(bar.GetComponentsInChildren<Image>()).Find(img => img != bar);

        //if (bar == null)
        //    Debug.Log("null");
        //Image[] aa = bar.GetComponentsInChildren<Image>();
        //Debug.Log(aa.Length);
        

    }


    private void Update()
    {
        bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,2f,0));
        barFilled.fillAmount = 0.5f;
    }
}
