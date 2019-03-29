using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,1.5f,0));
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Reset()
    {
        //색 그린으로 초기화
        barFilled.color = Color.green;
        //값 초기화
        barFilled.fillAmount = 1f;
    }

    /// <summary>
    /// 퍼센트로 작업하며 0-1사이임
    /// </summary>
    /// <param name="hp">Hp. 0 - 1 사이 값</param>
    public void SetHp(float hp) {
        barFilled.DOFillAmount(hp, 0.2f);
        if(0.25f <= hp && hp <= 0.65f) {        //체력 노랑으로 변경
            barFilled.DOColor(Color.yellow, 0.4f);
        } else if(0.3f > hp) {                  //체력 레드로 변경
            barFilled.DOColor(Color.red, 0.4f);
        }
    }

    public void Disable() {
        bar.enabled = false;
    }

    public void Enable()
    {
        bar.enabled = true;
    }
}
