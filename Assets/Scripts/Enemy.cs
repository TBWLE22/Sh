using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;

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
        //Rotate towards the target
    }

    private void FixedUpdate()
    {
        //Move forwards
        rb.velocity = transform.up * speed;
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
            LevelManager.manager.IncreaseScore(1);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
