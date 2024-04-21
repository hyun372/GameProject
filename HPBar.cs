using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public GameObject[] HPStacks;

    private void Update()
    {
        foreach(GameObject stack in HPStacks)
        {
            stack.SetActive(false);
        }
        for(int i = 0; i < GameManagerGP.Instance._currentHP; i++)
        {
            HPStacks[i].SetActive(true);
        }
    }
}
