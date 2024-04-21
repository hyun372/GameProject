using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private RectTransform touchPad;
    private int _touchID = -1;
    private Vector3 _startPos = Vector3.zero;
    public float _dragRadius = 60.0f;

    public Player player;
    private bool _buttonPressed = false;

    private void Start()
    {
        touchPad = GetComponent<RectTransform>();
        _startPos = touchPad.position;
    }

    private void FixedUpdate()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        HandleTouchInput();

#if UNITY_EDITOR|| UNITY_STANDALONE || PLATFORM_STANDALONE_WIN
        HandleInput(Input.mousePosition);
#endif
    }

    public void ButtonDown()
    {
        _buttonPressed = true;
    }
    public void ButtonUp()
    {
        _buttonPressed = false;
        HandleInput(_startPos);
    }

    private void HandleInput(Vector3 input)
    {
        if (_buttonPressed)
        {
            Vector3 diffVec = input - _startPos;
            if (diffVec.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diffVec.Normalize();
                touchPad.position = _startPos + diffVec * _dragRadius;
            }
            else
            {
                touchPad.position = input;
            }
        }
        else
        {
            touchPad.position = _startPos;
        }

        Vector3 diff = touchPad.position - _startPos;   
        Vector2 normDiff= new Vector2(diff.x, diff.y) / _dragRadius;

        if(player != null)
        {
            player.OnStickChanged(normDiff);
        }
    }

    private void HandleTouchInput()
    {
        int i = 0;

        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                i++;

                Vector3 touchPos =  new Vector3(touch.position.x, touch.position.y);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x <= (_startPos.x + _dragRadius))
                    {
                        _touchID = i;
                    }
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (_touchID == i)
                    {
                        HandleInput(touchPos);
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (_touchID == i)
                    {
                        _touchID = -1;
                    }
                }
            }
        }
    }
}

