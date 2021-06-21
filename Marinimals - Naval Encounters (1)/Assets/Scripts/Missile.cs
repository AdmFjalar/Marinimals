using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour
{
    private float timer;

    public GameObject target;
    public GameObject explosion;

    public int damage = 15;

    public float speed = 5f;
    public float rotateSpeed = 200f;
    public float delay = 3f;

    public Rigidbody2D rb2d;

    public GameObject ClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && t.layer != this.gameObject.layer)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer >= delay)
        {
            Destroy(gameObject);
        }

        target = ClosestTarget();

        if (target != null)
        {
            Vector2 direction = (Vector2)target.transform.position - rb2d.position;

            direction.Normalize();

            float rotateAmount = -Vector3.Cross(direction, transform.up).z;

            rb2d.angularVelocity = rotateAmount * rotateSpeed;

            rb2d.velocity = transform.up * speed;
        }
        else if (target == null)
        {
            Destroy(gameObject);
        }

        Collider2D[] intersecting = Physics2D.OverlapCapsuleAll(transform.position, new Vector2(0.5f, 1.5f), CapsuleDirection2D.Vertical, 0f);
        if (intersecting.Length == 0)
        {
            //code to run if nothing is intersecting as the length is 0
        }
        else
        {
            foreach (Collider2D col in intersecting)
            {
                if (col.gameObject.layer != this.gameObject.layer)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);

                    if (col.transform.GetComponent<Stats>() != null)
                    {
                        col.transform.GetComponent<Stats>().TakeDamage(damage * GameManager.instance.damageModifier[gameObject.layer - 8]);
                    }

                    if (col.transform.parent != null)
                    {
                        col.transform.parent.GetComponent<Stats>()?.TakeDamage(damage * GameManager.instance.damageModifier[gameObject.layer - 8]);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
