using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform selectSqImage;    // �巡�� �ڽ� �̹���

    Vector3 startPos;   // �巡�� ���� ����
    Vector3 endPos; // �巡�� �� ����

    // Start is called before the first frame update
    void Start()
    {
        selectSqImage.gameObject.SetActive(false);  // �巡�� �ڽ� ��Ȱ��ȭ
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // ���콺 �� Ŭ��
        {
            startPos = Input.mousePosition; // �巡�� ���� ���� ����
        }

        if(Input.GetMouseButtonUp(0))   // ���콺 �� Ŭ�� �� ��
        {
            selectSqImage.gameObject.SetActive(false);  // �巡�� �ڽ� ��Ȱ��ȭ
        }

        if (Input.GetMouseButton(0))    // ���콺 �� Ŭ�� ������ ���� ��
        {
            if (!selectSqImage.gameObject.activeInHierarchy)    // �巡�� �ڽ��� Ȱ��ȭ������ �ʴٸ�
            {
                selectSqImage.gameObject.SetActive(true);   // �巡�� �ڽ� Ȱ��ȭ
            }

            endPos = Input.mousePosition;   // �巡�� �� ���� ����

            Vector3 squareStart = startPos; // �巡�� ���� ���� ����

            Vector3 centre = (squareStart + endPos) / 2f;   // �巡�� �ڽ� �̹����� �߽� ��ġ
            selectSqImage.position = centre;    // �巡�� �ڽ� ��ġ ����

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);  // �巡�� �ڽ� ���� ����
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);  // �巡�� �ڽ� ���� ����

            selectSqImage.sizeDelta = new Vector2(sizeX, sizeY);    // �巡�� �ڽ� ������ ����
        }
    }
}
