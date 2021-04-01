using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 10;
    public float xPhase= 10;
    public float yPhase = 10;
    public float zPhase = 10;

    private void FixedUpdate()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(xPhase * Time.time*-0.2f) * radius * scale.x;
        tPos.y = Mathf.Sin(yPhase * Time.time * -0.2f) * radius * scale.y;
        tPos.z = Mathf.Sin(zPhase * Time.time * -0.2f) * radius * scale.z;
        transform.position = tPos;
        POS = tPos;
    }
}
