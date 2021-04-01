using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    static private Slingshot S;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimInMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }


    private void Awake()
    {
        S = this;
        Transform launcPointTrans = transform.Find("LaunchPoint");
        launchPoint = launcPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launcPointTrans.position;
        
    }

    private void OnMouseDown()
    {
        aimInMode = true;
        projectile = Instantiate<GameObject>(prefabProjectile);
        projectile.transform.position = launchPos;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;

    }

    private void Update()
    {
        if (!aimInMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            aimInMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCamera.POI = projectile;
            MissionDemodition.ShotFired();
            ProjectileLine.S.poi = projectile;
            projectile = null;
        }


    }
}
