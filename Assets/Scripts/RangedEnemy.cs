using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 4f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;

    public float distancetoshoot = 6f;
    public float distancetostop = 3f;
    public Transform firingpoint;
    public float fireRate;
    private float timetofire;

    public GameObject bulletPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Get the target
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }
        if (target != null && Vector2.Distance(target.position, transform.position)>= distancetostop)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (timetofire <= 0f) 
        {
            Instantiate(bulletPrefab, firingpoint.position, firingpoint.rotation);
            timetofire = fireRate;
        }
        else
        {
            timetofire -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) <= distancetoshoot)
            {
                //Move forwards
                rb.velocity = transform.up * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void  RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position -transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed); 
        //Slerp allows us to interpolate between values allowing for a gradual turning
    }
    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.manager.GameOver();
            Destroy(other.gameObject);
            target = null;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            LevelManager.manager.IncreaseScore(3);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
