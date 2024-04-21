using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickOn : MonoBehaviour
{
    private RawImage rawImage;

    [HideInInspector]
    public bool currentSelected = false;    // 현재 오브젝트가 선택된 상태인지 확인하는 플래그 

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        Camera.main.gameObject.GetComponent<Click>().selectableObjects.Add(this.gameObject);    // seletableObjects에 현재 오브젝트 추가

        ClickMe();  // 색 변경
    }

    // Update is called once per frame
    public void ClickMe()   // 색 변경 함수
    {
        if(currentSelected == false)
        {
            rawImage.color = Color.red;
        }
        else
        {
            rawImage.color = Color.green;
        }
    }
}
