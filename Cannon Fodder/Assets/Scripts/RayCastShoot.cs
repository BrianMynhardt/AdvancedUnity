using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShoot : MonoBehaviour
{
    public int gunDamage = 10;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public GameObject firstPersonGO;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laser;
    private float nextFire;
     
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        fpsCam = firstPersonGO.GetComponent<Camera>();

    }

    
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laser.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laser.SetPosition(1, hit.point);

                Shootable health = hit.collider.GetComponent<Shootable>();

                if(health != null)
                {
                    health.Damage(gunDamage);
                }

                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                laser.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laser.enabled = true;
        yield return shotDuration;
        laser.enabled = false;
    }
}
