using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public Transform startPoint;
    public Transform controlPoint;
    public Transform endPoint;


    private void OnDrawGizmos()
    {
        if (startPoint != null && controlPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, controlPoint.position);
            Gizmos.DrawLine(controlPoint.position, endPoint.position);
        }

        Gizmos.color = Color.red;
        for(float t = 0; t <= 1; t += 0.05f)
        {
            Vector2 pixel = Mathf.Pow(1 - t, 2) * startPoint.position + 2 * (1 - t) * t * controlPoint.position + Mathf.Pow(t, 2) * endPoint.position;
            Gizmos.DrawSphere(pixel, 0.1f);
        }
    }
}
