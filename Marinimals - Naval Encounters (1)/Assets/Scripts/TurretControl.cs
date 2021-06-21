using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed;

    private Rigidbody2D rb;

    private float startRotation;

    private float min;
    private float max;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.eulerAngles.z;

        min = startRotation - 90;
        max = startRotation + 90;

        if (min < 0)
        {
            min += 360;
        } else if (min > 360)
        {
            min -= 360;
        }

        if (max < 0)
        {
            max += 360;
        } else if (max > 360)
        {
            max -= 360;
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = (Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
