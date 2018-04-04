using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float FireRate = 0;
    public float Damage = 10;
    public LayerMask WhatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    float TimeToSpawnEffect = 0;
    public float EffectSpawnRate = 10;

    float TimeToFire = 0;
    Transform FirePoint;

	// Use this for initialization (???)
	void Awake ()
    {
        FirePoint = transform.Find("FirePoint"); //"Find" em vez de "FindChild" porque "FindChild" já não existe (foi subsitituido por "Find")

        if (FirePoint == null)
        {
            Debug.LogError("No FirePoint? WHAT!?");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (FireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > TimeToFire)
            {
                TimeToFire = Time.time + 1 / FireRate;
                Shoot();
            }
        }
	}

    void Shoot ()
    {
        Vector2 MousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, MousePosition-firePointPosition, 100, WhatToHit);
        if (Time.time >= TimeToSpawnEffect)
        {
            Effect();
            TimeToSpawnEffect = Time.time + 1/EffectSpawnRate;
        }
        
        Debug.DrawLine(firePointPosition, (MousePosition-firePointPosition)*100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage");
        }
    }

    void Effect ()
    {
        Instantiate (BulletTrailPrefab, FirePoint.position, FirePoint.rotation);
        Transform clone = Instantiate (MuzzleFlashPrefab, FirePoint.position, FirePoint.rotation) as Transform;
        clone.parent = FirePoint;
        float size = Random.Range (0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy (clone.gameObject, 0.02f);
    }
}