using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;

    private void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;
        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
        rigid.velocity = vel;

        LookAhead();

        Color randColor = Color.black;
        while(randColor.r + randColor.g +randColor.b <1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();

        foreach(Renderer r in rends)
        {
            r.material.color = randColor;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TinColor", randColor);
    }

    void LookAhead()
    {
        transform.LookAt(pos + rigid.velocity);

    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Spawner spn = Spawner.S;

        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if(tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.velocity;
        }

        Vector3 velAlign = neighborhood.avgVel;
        if(velAlign!=Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *=spn.velocity;
        }

        Vector3 velCentert = neighborhood.avgPos;
        if(velCentert != Vector3.zero)
        {
            velCentert -= transform.position;
            velCentert.Normalize();
            velCentert *= spn.velocity;
        }

        Vector3 delta = Attractor.POS - pos;
        bool attracted = delta.magnitude>spn.attractPushDist;
        Vector3 velAttract = delta.normalized * spn.velocity;

        float fdt = Time.fixedDeltaTime;

        if(attracted)
        {
            vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
        }
        else
        {
            vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);

        }

        vel = vel.normalized * spn.velocity;
        rigid.velocity = vel;
        LookAhead();
    }
}
