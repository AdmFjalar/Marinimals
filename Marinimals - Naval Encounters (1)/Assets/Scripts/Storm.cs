using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [SerializeField] private LayerMask targets;

    [SerializeField] private int damage = 1;

    [SerializeField] private float speed = 1f;

    private Vector2 startPos;
    private Vector2 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = -startPos;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, 2.5f, targets);
        if (intersecting.Length == 0)
        {
            //code to run if nothing is intersecting as the length is 0
        }
        else
        {
            foreach (Collider2D col in intersecting)
            {
                if (col.transform.GetComponent<Stats>() != null)
                {
                    col.transform.GetComponent<Stats>()?.TakeDamage(damage);
                }

                if (col.transform.parent != null)
                {
                    col.transform.parent.GetComponent<Stats>()?.TakeDamage(damage);
                }
            }
        }

        if (Vector2.Distance(transform.position, targetPos) < 0.5f)
        {
            Destroy(gameObject);
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
    }
}
