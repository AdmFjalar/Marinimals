using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
    public Transform ship;
    public PlayerControls controls;
    public float x;
    public float y;
    public float rotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ship != null)
        {
            transform.position = new Vector3(ship.position.x, ship.position.y, ship.position.z);
        }
        else if (ship == null)
        {
            gameObject.SetActive(false);
        }

        if (controls != null)
        {
            x = Input.GetAxis(controls.horizontal);
            y = Input.GetAxis(controls.vertical);

            float heading = Mathf.Atan2(x, y);

            if (x == 0 && y == 0)
            {

            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg + 90);
            }
        }
    }
}
        //transform.Rotate(transform.rotation.x, transform.rotation.y, -(x * rotationSpeed));
