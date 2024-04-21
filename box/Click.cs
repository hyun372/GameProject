using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer;   // Ŭ�� ������ ������Ʈ�� ���̾� ����ũ

    private List<GameObject> selectedObjects;   // ���õ� ������Ʈ�� ����Ʈ

    private Vector3 mousePos1, mousePos2;
    private Vector2 worldPos;

    [HideInInspector]
    public List<GameObject> selectableObjects;  // ������ �� �ִ� ������Ʈ�� ����Ʈ

    private void Awake()
    {
        selectedObjects = new List<GameObject>();   // �ʱ�ȭ
        selectableObjects = new List<GameObject>(); // �ʱ�ȭ
    }
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))    // ���콺 ���� Ŭ��
        {
            ClearSelection();   // ���� ���õ� ������Ʈ�� Ŭ����
        }
        else if (Input.GetMouseButtonDown(0))   // ���콺 �� Ŭ��
        {
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition); // ���콺�� ���� ������
        }

        else if(Input.GetMouseButtonUp(0))  // ���콺 �� Ŭ�� �� ��
        {
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition); // ���콺�� ������ ������

            if(Vector3.Distance(mousePos1, mousePos2) > 0.1)    // ���콺 �巡�� Ȯ��
            {
                SelectObjects();    // �巡�� ������ ������Ʈ�� ����
            }
            else
            {
                worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���콺 ������ ���� ��ǥ��� ����
                RaycastHit2D rayHit = 
                    Physics2D.Raycast(worldPos, transform.forward, Mathf.Infinity, clickableLayer); // ���콺 ���� ��ǥ���� ������ clickableLayer�� ������Ʈ�� ���͸�

                Debug.DrawRay(worldPos, transform.forward * 20, Color.red, 3.0f);   // ����� �� �׸���

                if (rayHit) 
                {
                    // ray�� �ε��� ���� ������Ʈ���� ClickOn��ũ��Ʈ �ҷ�����
                    ClickOn clickOnScript = rayHit.collider.gameObject.GetComponent<ClickOn>();

                    if (Input.GetKey("left ctrl"))  // �� ��Ʈ�� ���� ���¸�
                    {
                        if (rayHit.collider.GetComponent<ClickOn>().currentSelected == false)   // ������Ʈ�� ���õ� ���� �ƴ� ��
                        {
                            selectedObjects.Add(rayHit.collider.gameObject);    // selectedObjects�� ������Ʈ �߰�
                            
                            rayHit.collider.GetComponent<ClickOn>().currentSelected = true; // �ش� ������Ʈ ���õ� ���·� ����
                        }
                        else    // ������Ʈ�� �̹� ���� �� ������ ��
                        {
                            selectedObjects.Remove(rayHit.collider.gameObject); // selectedObjexts���� ����
                            rayHit.collider.GetComponent<ClickOn>().currentSelected = false;    // �ش� ������Ʈ ���� ���� ���� ���·� ����
                        }
                        rayHit.collider.GetComponent<ClickOn>().ClickMe();  // �� ����
                    }
                    else
                    {
                        ClearSelection();   // ���� ���õ� ������Ʈ�� Ŭ����

                        selectedObjects.Add(rayHit.collider.gameObject);    // selectedObjects�� ������Ʈ �߰�
                        rayHit.collider.GetComponent<ClickOn>().currentSelected = true; // �ش� ������Ʈ ���õ� ���·� ����
                        rayHit.collider.GetComponent<ClickOn>().ClickMe();  // �� ����
                    }
                }
            }
        }
    }

    void SelectObjects()
    {
        List<GameObject> remObjects = new List<GameObject>();   // ���ÿ��� ������ ������Ʈ�� �ӽ� ����Ʈ

        if(Input.GetKey("left ctrl") == false)  // �� ��Ʈ�� ���� ���� �ƴϸ� Ŭ����
        {
            ClearSelection();
        }
        Rect selectRect = new Rect(mousePos1.x, mousePos1.y,
            mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);

        foreach(GameObject selectObj in selectableObjects)
        {
            if(selectObj != null)   // ������Ʈ�� ���� �Ѵٸ�
            {
                if(selectRect.Contains(Camera.main.
                    WorldToViewportPoint(selectObj.transform.position), true))  // ȭ�鿡�� ������Ʈ�� ��ġ�� selectRect�ȿ� �ִ��� Ȯ��
                {
                    if (selectObj.GetComponent<ClickOn>().currentSelected == false) // ������Ʈ�� ���õ� ���°� �ƴ� ��
                    {
                        selectedObjects.Add(selectObj); // selectedObjects�� �߰�
                        selectObj.GetComponent<ClickOn>().currentSelected = true;   // �ش� ������Ʈ ���õ� ���·� ����
                        selectObj.GetComponent<ClickOn>().ClickMe();    // �� ����
                    }
                    else    // ������Ʈ�� �̹� ���õ� ������ ��
                    {
                        remObjects.Add(selectObj);  // remObjects�� �߰�
                        selectObj.GetComponent<ClickOn>().currentSelected = false;  // �ش� ������Ʈ ���õ��� ���� ���·� ����
                        selectObj.GetComponent<ClickOn>().ClickMe();    // �� ����
                    }

                    
                }
            }
            else    // ������Ʈ�� �������� �ʴ´ٸ�
            {
                remObjects.Add(selectObj);  // remObjects�� �߰�
            }
        }

        if(remObjects.Count >0) // ������ ������Ʈ�� �ִ� ���
        {
            foreach (GameObject rem in remObjects)  // remObjects�� �� ��ü���� ����
            {
                selectedObjects.Remove(rem);
            }
            remObjects.Clear(); // ����Ʈ Ŭ����
        }
    }
    void ClearSelection()   // ���� ���õ� ������Ʈ�� Ŭ����
    {
        if (selectedObjects.Count > 0)
        {
            foreach(GameObject obj in selectedObjects)
            {
                if(obj != null)
                {
                    obj.GetComponent<ClickOn>().currentSelected = false;    // �ش� ������Ʈ ���õ��� ���� ���·� ����
                    obj.GetComponent<ClickOn>().ClickMe();  // �� ����
                }
            }
            selectedObjects.Clear();
        }
    }
}
