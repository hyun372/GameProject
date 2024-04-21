using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private void Update()
    {
        if (!GameManagerGP.Instance._isPlaying) { return; }

        transform.Translate(Vector2.down * Time.deltaTime * 10f);

        if (transform.position.y < -12)
        {
            transform.position = Vector2.zero;
        }
    }
}
