using UnityEngine;

public class IceBreaker : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Stats>() != null)
        {
            collision.gameObject.GetComponent<Stats>()?.TakeDamage(damage * 5 * GameManager.instance.damageModifier[transform.parent.gameObject.layer - 8] * Mathf.RoundToInt(transform.parent.GetComponent<ShipControl>().speedModifier));

            if (transform.parent.GetComponent<ShipControl>().charging)
            {
                //collision.transform.GetComponent<ShipControl>().velocity += transform.up * 500000;
                collision.transform.GetComponent<Rigidbody2D>().AddForce(transform.up * 150);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Stats>() != null)
        {
            collision.gameObject.GetComponent<Stats>()?.TakeDamage(1 * GameManager.instance.damageModifier[transform.parent.gameObject.layer - 8] * Mathf.RoundToInt(transform.parent.GetComponent<ShipControl>().speedModifier));

            if (transform.parent.GetComponent<ShipControl>().charging)
            {
                //collision.transform.GetComponent<ShipControl>().velocity += transform.up * 500000;
                collision.transform.GetComponent<Rigidbody2D>().AddForce(transform.up * 150);
            }
        }
    }
}
