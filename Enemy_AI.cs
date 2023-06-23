using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    public int gunDamage = 1;
    public float distance;
    public float fireRate = 0.285f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Transform BadGuy;
    public LineRenderer laserLine;
    public AudioSource gunAudio;
    public NavMeshAgent enemyAgent;
    public Animator enemyAnimator;
    public int currentHealth = 10;
    private float nextFire;
    public Transform player;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        distance = Vector3.Distance(transform.position, player.position);

        if (Time.time > nextFire && distance < 25 && distance > 4)
        {
            BadGuy.LookAt(player);
            enemyAnimator.Play("Rifle_Aiming_Shoot");
            Shoot();
        }

        if (distance > 35)
        {
            enemyAgent.enabled = true;
            enemyAgent.SetDestination(player.position);
            enemyAnimator.Play("Rifle_Aiming_Walk_F");
        }
    }

    private void Shoot()
    {
        nextFire = Time.time + fireRate;

        StartCoroutine(ShotEffect());

        Vector3 rayOrigin = gunEnd.position;

        RaycastHit hit;

        laserLine.SetPosition(0, rayOrigin);

        if (Physics.Raycast(rayOrigin, gunEnd.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);

            Health health = hit.collider.GetComponent<Health>();

            if (health != null)
            {
                health.Damage(gunDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + gunEnd.forward * weaponRange);
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && distance < 100) 
        {
            enemyAnimator.Play("Death_A");
            Destroy(laserLine);
            Destroy(gunEnd);
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return new WaitForSeconds(0.07f);
        laserLine.enabled = false;
    }
}
