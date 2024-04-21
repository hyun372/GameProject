using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickOn : MonoBehaviour
{
    private RawImage rawImage;

    [HideInInspector]
    public bool currentSelected = false;    // ���� ������Ʈ�� ���õ� �������� Ȯ���ϴ� �÷��� 

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        Camera.main.gameObject.GetComponent<Click>().selectableObjects.Add(this.gameObject);    // seletableObjects�� ���� ������Ʈ �߰�

        ClickMe();  // �� ����
    }

    // Update is called once per frame
    public void ClickMe()   // �� ���� �Լ�
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
