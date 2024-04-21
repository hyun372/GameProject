using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float _time;

    private void Awake()
    {
        _time = .25f;
    }

    private void Start()
    {
        Destroy(gameObject, _time);
    }

}
