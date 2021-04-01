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
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColor;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool notifiedOfDestruction = false;

    protected BoundsCheck bndChek;

    private void Awake()
    {
        bndChek = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColor = new Color[materials.Length];
        for( int i =0; i<materials.Length; i++)
        {
            originalColor[i] = materials[i].color;
        }
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

        if(showingDamage&&Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
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
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                ShowDamage();
                Projectile p = otherGO.GetComponent<Projectile>();
                if(!bndChek.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health<=0)
                {
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero" + otherGO.name);
                break;
        }
    }

    void ShowDamage()
    {
        foreach(Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for(int i =0; i<materials.Length; i++)
        {
            materials[i].color = originalColor[i];
        }
        showingDamage = false; 
    }
}
