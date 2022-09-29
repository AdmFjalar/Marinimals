using System.Collections;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public PlayerControls controls; //Button mappings for the respective player.
    public int throttleSteps = 3; //The amount of "steps" the player can switch between, each higher step being faster than the last.
    public int currentThrottle = 0;
    public float speed = 1f;
    public float maxRotateSpeed = 200f;
    public float velocityDrag = 1f;
    public float rotationDrag = 1f;
    public float verticalAcceleration = 1f;
    public float rotationAcceleration = 10f;
    public float speedModifier = 1f;
    public Transform rudder;
    //public Rigidbody2D rb2d;
    public Vector3 velocity;
    public float rotationVelocity;
    public bool charging = false;

    private bool isBoosted = false;
    private float boostTimer;
    private float chargeTimer;
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        charging = false;
        speedModifier = 1f;
        startPos = transform.position;
        //rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.gameObject.layer = transform.parent.gameObject.layer;

        SpeedControl();
        Shoot();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBoosted)
        {
            if (boostTimer < 5)
            {
                boostTimer += Time.deltaTime;
            }
            else if (boostTimer >= 5)
            {
                boostTimer = 0;
                isBoosted = false;
                speedModifier = 1;
            }
        }

        velocity = velocity * (1 - Time.deltaTime * velocityDrag);

        if (velocity.magnitude < 0.2 && currentThrottle == 0)
        {
            velocity.x = 0;
            velocity.y = 0;
            velocity.z = 0;
        }

        velocity = Vector3.ClampMagnitude(velocity, speed * 3 * speedModifier);

        Vector3 targetPosition = transform.position + velocity * Time.deltaTime;

        //gameObject.GetComponent<Rigidbody2D>().MovePosition(targetPosition);

        transform.position = targetPosition;

        if (!charging)
        {
            RotateBoat();
        }

        if (charging && chargeTimer < 3)
        {
            rotationVelocity = 0;
            chargeTimer += Time.deltaTime;
        }

        if (chargeTimer >= 3)
        {
            chargeTimer = 0;
            charging = false;
        }
        //KeepShipOnMap();

        //float x = Input.GetAxis(horizontal);
        //float y = Input.GetAxis(vertical);
    }

    /// <summary>
    /// Rotates the boat after the rudder.
    /// </summary>
    public void RotateBoat()
    {
        Vector2 direction = (Vector2)rudder.position - (Vector2)transform.position;

        direction.Normalize();

        float rotationAmount = Vector3.Cross(direction, transform.up).z;

        float turnAcceleration = -rotationAmount * rotationAcceleration;

        rotationVelocity += turnAcceleration * Time.deltaTime;

        rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotateSpeed, maxRotateSpeed);

        rotationVelocity = rotationVelocity * (1 - Time.deltaTime * rotationDrag);


        transform.Rotate(0, 0, rotationVelocity * Time.deltaTime);

        //gameObject.GetComponent<Rigidbody2D>().angularVelocity = -rotationAmount * rotateSpeed;

        //transform.Rotate(new Vector3(0, 0, -rotationAmount));
    }

    /// <summary>
    /// Clamps the position of the ship withing the borders of the map.
    /// </summary>
    public void KeepShipOnMap()
    {
        float _x = Mathf.Clamp(transform.position.x, -GameManager.instance.maxBoundary, GameManager.instance.maxBoundary);
        float _y = Mathf.Clamp(transform.position.y, -GameManager.instance.maxBoundary, GameManager.instance.maxBoundary);

        transform.position = new Vector3(_x, _y);
    }

    /// <summary>
    /// Sets the speed of the ship and also moves the ship.
    /// </summary>
    public void SpeedControl()
    {
        if (Input.GetButtonDown(controls.leftBumper) && currentThrottle - 1 >= -throttleSteps)
        {
            currentThrottle--;
        }

        if (Input.GetButtonDown(controls.rightBumper) && currentThrottle + 1 <= throttleSteps)
        {
            currentThrottle++;
        }
        //rb2d.AddForce(transform.up * speed * currentThrottle);

        Vector3 acceleration = currentThrottle * transform.up * verticalAcceleration;

        velocity += acceleration * Time.deltaTime;
    }

    /// <summary>
    /// Shoots all turrets on the ship when the respective button is pressed.
    /// </summary>
    public void Shoot()
    {
        if (Input.GetAxis(controls.rightTrigger) > 0)
        {
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            foreach (Gun g in guns)
            {
                g.Shoot();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag * 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            collision.gameObject.GetComponent<Powerup>().Use(gameObject);
        }
    }

    public void Charge()
    {
        speedModifier = 5f;
        velocity *= 5f;
        charging = true;
    }

    public void AddSpeedBoost()
    {
        speedModifier = 2f;
        isBoosted = true;
    }
}
