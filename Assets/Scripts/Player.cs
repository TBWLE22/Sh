using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    //Gun variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingpoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;

    private float fireTimer;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x -transform.position.x) * Mathf.Rad2Deg -90f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(mx, my).normalized * speed;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firingpoint.position, firingpoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            LevelManager.manager.GameOver();
            Destroy(gameObject);
        }
    }
}
