using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set In Inspector: Enemy_2")]
    public float sinEccenricaity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    public Vector3 p0;
    public Vector3 p1;
    public float birtTime;

    private void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndChek.camWidth - bndChek.radius;
        p0.y = Random.Range(-bndChek.camHeight, bndChek.camHeight);

        p1 = Vector3.zero;
        p1.x = bndChek.camWidth + bndChek.radius;
        p1.y = Random.Range(-bndChek.camHeight, bndChek.camHeight);

        if(Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        birtTime = Time.time; 
    }

    public override void Move()
    {
        float u = (Time.time - birtTime) / lifeTime;

        if(u>1)
        {
            Destroy(this.gameObject);
            return;
        }

        u = u + sinEccenricaity * (Mathf.Sin(u * Mathf.PI * 2));

        pos = (1 - u) * p0 + u * p1;
        
    }
}
