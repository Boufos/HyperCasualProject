using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 100;
    public int score = 100;

    private BoundsCheck bndChek;

    private void Awake()
    {
        bndChek = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    private void Update()
    {
        Move();
        if(bndChek !=null && bndChek.offDown)
        {
                Destroy(gameObject);
            
        }
    }

    public virtual void Move()
    {
        Vector3 tempos = pos;
        tempos.y -= speed * Time.deltaTime;
        pos = tempos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        if(otherGO.tag == "ProjectileHero")
        {
            Destroy(otherGO);
            Destroy(gameObject);
        }
        else
        {
            print("Enemy hit by non-ProjectileHero" + otherGO.name);
        }
    }
}
