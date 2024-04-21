using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform selectSqImage;    // 드래그 박스 이미지

    Vector3 startPos;   // 드래그 시작 지점
    Vector3 endPos; // 드래그 끝 지점

    // Start is called before the first frame update
    void Start()
    {
        selectSqImage.gameObject.SetActive(false);  // 드래그 박스 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // 마우스 왼 클릭
        {
            startPos = Input.mousePosition; // 드래그 시작 지점 저장
        }

        if(Input.GetMouseButtonUp(0))   // 마우스 왼 클릭 뗄 때
        {
            selectSqImage.gameObject.SetActive(false);  // 드래그 박스 비활성화
        }

        if (Input.GetMouseButton(0))    // 마우스 왼 클릭 누르고 있을 때
        {
            if (!selectSqImage.gameObject.activeInHierarchy)    // 드래그 박스가 활성화돼있지 않다면
            {
                selectSqImage.gameObject.SetActive(true);   // 드래그 박스 활성화
            }

            endPos = Input.mousePosition;   // 드래그 끝 지점 저장

            Vector3 squareStart = startPos; // 드래그 시작 지점 저장

            Vector3 centre = (squareStart + endPos) / 2f;   // 드래그 박스 이미지의 중심 위치
            selectSqImage.position = centre;    // 드래그 박스 위치 설정

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);  // 드래그 박스 가로 길이
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);  // 드래그 박스 세로 길이

            selectSqImage.sizeDelta = new Vector2(sizeX, sizeY);    // 드래그 박스 사이즈 설정
        }
    }
}
