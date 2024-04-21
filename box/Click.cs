using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer;   // 클릭 가능한 오브젝트의 레이어 마스크

    private List<GameObject> selectedObjects;   // 선택된 오브젝트들 리스트

    private Vector3 mousePos1, mousePos2;
    private Vector2 worldPos;

    [HideInInspector]
    public List<GameObject> selectableObjects;  // 선택할 수 있는 오브젝트들 리스트

    private void Awake()
    {
        selectedObjects = new List<GameObject>();   // 초기화
        selectableObjects = new List<GameObject>(); // 초기화
    }
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))    // 마우스 오른 클릭
        {
            ClearSelection();   // 현재 선택된 오브젝트들 클리어
        }
        else if (Input.GetMouseButtonDown(0))   // 마우스 왼 클릭
        {
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition); // 마우스의 시작 포지션
        }

        else if(Input.GetMouseButtonUp(0))  // 마우스 왼 클릭 뗄 때
        {
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition); // 마우스의 마지막 포지션

            if(Vector3.Distance(mousePos1, mousePos2) > 0.1)    // 마우스 드래그 확인
            {
                SelectObjects();    // 드래그 범위의 오브젝트들 선택
            }
            else
            {
                worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 포지션 월드 좌표계로 변경
                RaycastHit2D rayHit = 
                    Physics2D.Raycast(worldPos, transform.forward, Mathf.Infinity, clickableLayer); // 마우스 월드 좌표에서 앞으로 clickableLayer의 오브젝트만 필터링

                Debug.DrawRay(worldPos, transform.forward * 20, Color.red, 3.0f);   // 디버그 선 그리기

                if (rayHit) 
                {
                    // ray에 부딪힌 게임 오브젝트에서 ClickOn스크립트 불러오기
                    ClickOn clickOnScript = rayHit.collider.gameObject.GetComponent<ClickOn>();

                    if (Input.GetKey("left ctrl"))  // 왼 컨트롤 누른 상태면
                    {
                        if (rayHit.collider.GetComponent<ClickOn>().currentSelected == false)   // 오브젝트가 선택된 상태 아닐 때
                        {
                            selectedObjects.Add(rayHit.collider.gameObject);    // selectedObjects에 오브젝트 추가
                            
                            rayHit.collider.GetComponent<ClickOn>().currentSelected = true; // 해당 오브젝트 선택된 상태로 변경
                        }
                        else    // 오브젝트가 이미 선택 된 상태일 때
                        {
                            selectedObjects.Remove(rayHit.collider.gameObject); // selectedObjexts에서 제거
                            rayHit.collider.GetComponent<ClickOn>().currentSelected = false;    // 해당 오브젝트 선택 되지 않은 상태로 변경
                        }
                        rayHit.collider.GetComponent<ClickOn>().ClickMe();  // 색 변경
                    }
                    else
                    {
                        ClearSelection();   // 현재 선택된 오브젝트들 클리어

                        selectedObjects.Add(rayHit.collider.gameObject);    // selectedObjects에 오브젝트 추가
                        rayHit.collider.GetComponent<ClickOn>().currentSelected = true; // 해당 오브젝트 선택된 상태로 변경
                        rayHit.collider.GetComponent<ClickOn>().ClickMe();  // 색 변경
                    }
                }
            }
        }
    }

    void SelectObjects()
    {
        List<GameObject> remObjects = new List<GameObject>();   // 선택에서 제거할 오브젝트들 임시 리스트

        if(Input.GetKey("left ctrl") == false)  // 왼 컨트롤 누른 상태 아니면 클리어
        {
            ClearSelection();
        }
        Rect selectRect = new Rect(mousePos1.x, mousePos1.y,
            mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);

        foreach(GameObject selectObj in selectableObjects)
        {
            if(selectObj != null)   // 오브젝트가 존재 한다면
            {
                if(selectRect.Contains(Camera.main.
                    WorldToViewportPoint(selectObj.transform.position), true))  // 화면에서 오브젝트의 위치가 selectRect안에 있는지 확인
                {
                    if (selectObj.GetComponent<ClickOn>().currentSelected == false) // 오브젝트가 선택된 상태가 아닐 때
                    {
                        selectedObjects.Add(selectObj); // selectedObjects에 추가
                        selectObj.GetComponent<ClickOn>().currentSelected = true;   // 해당 오브젝트 선택된 상태로 변경
                        selectObj.GetComponent<ClickOn>().ClickMe();    // 색 변경
                    }
                    else    // 오브젝트가 이미 선택된 상태일 때
                    {
                        remObjects.Add(selectObj);  // remObjects에 추가
                        selectObj.GetComponent<ClickOn>().currentSelected = false;  // 해당 오브젝트 선택되지 않은 상태로 변경
                        selectObj.GetComponent<ClickOn>().ClickMe();    // 색 변경
                    }

                    
                }
            }
            else    // 오브젝트가 존재하지 않는다면
            {
                remObjects.Add(selectObj);  // remObjects에 추가
            }
        }

        if(remObjects.Count >0) // 제거할 오브젝트가 있는 경우
        {
            foreach (GameObject rem in remObjects)  // remObjects의 각 개체들을 제거
            {
                selectedObjects.Remove(rem);
            }
            remObjects.Clear(); // 리스트 클리어
        }
    }
    void ClearSelection()   // 현재 선택된 오브젝트들 클리어
    {
        if (selectedObjects.Count > 0)
        {
            foreach(GameObject obj in selectedObjects)
            {
                if(obj != null)
                {
                    obj.GetComponent<ClickOn>().currentSelected = false;    // 해당 오브젝트 선택되지 않은 상태로 변경
                    obj.GetComponent<ClickOn>().ClickMe();  // 색 변경
                }
            }
            selectedObjects.Clear();
        }
    }
}
