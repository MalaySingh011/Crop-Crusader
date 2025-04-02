using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a bullet object
public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    Boss boss;
    float xSpeed;

    //Initializes a bullet
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        boss = FindObjectOfType<Boss>();
        xSpeed = boss.transform.localScale.x * bulletSpeed;
    }

    //Updates the bullet with its speed
    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    //Damages the player
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        if (other.gameObject.tag == "Player")
        {
            StaminaHealth.instance.TakeDamage("Boss");
        }
    }

}
