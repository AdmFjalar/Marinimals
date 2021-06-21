using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public int healingAmount;
    public float delay;

    private float timer;

    void Update()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        } else if (timer >= delay)
        {
            timer = 0f;
            Heal(healingAmount);
        }
    }

    public void Heal(int healingAmount)
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);
        if (intersecting.Length == 0)
        {
            //code to run if nothing is intersecting as the length is 0
        }
        else
        {
            foreach (Collider2D col in intersecting)
            {
                col.transform.GetComponent<Stats>()?.Heal(healingAmount);

                if (col.transform.parent != null && col.transform.parent.GetComponent<Stats>() != null)
                col.transform.parent.GetComponent<Stats>()?.Heal(healingAmount);
            }
        }
    }
}
