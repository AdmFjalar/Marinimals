using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    private Vector3 velocity;
    private float timer;

    public int verticalAcceleration = 1;
    public int maxSpeed = 5;
    public int damage = 45;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        velocity = transform.up * 3;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 7.5f)
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

    void FixedUpdate()
    {
        Vector3 acceleration = transform.up * verticalAcceleration;

        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        Vector3 targetPosition = transform.position + velocity * Time.deltaTime;

        gameObject.GetComponent<Rigidbody2D>().MovePosition(targetPosition);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (/*collision.gameObject.layer != gameObject.layer*/ true)
    //    {
    //        Instantiate(explosion, transform.position, Quaternion.identity);

    //        collision.gameObject.GetComponent<Stats>()?.TakeDamage(damage);

    //        if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}
