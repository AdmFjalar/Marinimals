using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : MonoBehaviour
{
    public int damage = 80;
    public GameObject explosion;

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2);
        if (intersecting.Length == 0)
        {
            //code to run if nothing is intersecting as the length is 0
        }
        else
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            foreach (Collider2D col in intersecting)
            {
                if (col.transform.parent != null)
                col.transform.parent.GetComponent<Stats>()?.TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }
    }
}
