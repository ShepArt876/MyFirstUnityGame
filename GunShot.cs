using UnityEngine;
using System.Collections;

public class GunShot : MonoBehaviour 
{
    public int gunDamage;
    public float fireRate;
    public float weaponRange;
    public float hitForce;    
    public Transform gunEnd;
    public Camera fpsCam;                                                
    public WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    public AudioSource gunAudio;
    public LineRenderer laserLine;
    private float nextFire;
    private bool isFired;

    void Start () 
    {
        laserLine = GetComponent<LineRenderer>();

        gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }

    void Update () 
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            laserLine.SetPosition (0, gunEnd.position);

            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                Enemy_AI health = hit.collider.GetComponent<Enemy_AI>();

                if (health != null)
                {
                    health.Damage (gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce (-hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, fpsCam.transform.forward * weaponRange);
            }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play ();

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}