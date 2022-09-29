using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetBomber : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private Vector2 finalPos;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject explosion;
    private float wantedRotation;
    private float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, finalPos, step);

            if (Vector2.Distance(targetPos, transform.position) < 10)
            {
                if (timer >= 0.2f)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);

                    Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, 4.5f);
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

                    timer = 0f;
                }
                else if (timer < 0.2f)
                {
                    timer += Time.deltaTime;
                }
            }
        }

        if (finalPos != null && Vector2.Distance(finalPos, transform.position) < 0.25f)
        {
            Destroy(gameObject);
        }
    }

    public void InitiateBombing(Vector2 target)
    {
        if (Random.Range(0, 2) == 0)
        {
            transform.position = new Vector2(-GameManager.instance.maxBoundary, Random.Range(-GameManager.instance.maxBoundary * 2, GameManager.instance.maxBoundary * 2));

            startPos = transform.position;

            targetPos = target;

            finalPos = (Vector2)transform.position + ((targetPos - (Vector2)transform.position).normalized * GameManager.instance.maxBoundary * 4);

            transform.rotation = Quaternion.Euler(0, 0, 360 - Mathf.Asin((transform.position.y - finalPos.y) / Vector2.Distance(startPos, finalPos)) * Mathf.Rad2Deg);
        }
        else
        {
            transform.position = new Vector2(GameManager.instance.maxBoundary * 2, Random.Range(-GameManager.instance.maxBoundary * 2, GameManager.instance.maxBoundary * 2));

            startPos = transform.position;

            targetPos = target;

            finalPos = (Vector2)transform.position + ((targetPos - (Vector2)transform.position).normalized * GameManager.instance.maxBoundary * 4);

            transform.rotation = Quaternion.Euler(0, 0, 180 - Mathf.Asin((finalPos.y - transform.position.y) / Vector2.Distance(startPos, finalPos)) * Mathf.Rad2Deg);
        }
    }
}
