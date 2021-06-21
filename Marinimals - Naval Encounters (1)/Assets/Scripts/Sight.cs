using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    private float timer = 1f;

    private float acceleration = 1.5f;

    public PlayerControls controls;

    public float sightRange = 5f;

    public float speed = 0.15f;

    public float worldUnitsPerScreenPixel { get { return (GameManager.instance.mainCamera.orthographicSize * 2f) / GameManager.instance.mainCamera.pixelHeight; } }
    public float cameraWidth { get { return worldUnitsPerScreenPixel * GameManager.instance.mainCamera.pixelWidth; } }
    public float cameraHeight { get { return worldUnitsPerScreenPixel * GameManager.instance.mainCamera.pixelHeight; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis(controls.horizontal2);
        float y = Input.GetAxis(controls.vertical2);

        if (x == 0 && y == 0)
        {
            timer = 1;
        }
        else if (timer < 5 && (x != 0 || y != 0))
        {
            timer += Time.deltaTime * acceleration;
        }

        transform.GetChild(0).transform.Translate(new Vector3(x, -y, 0) * speed * Mathf.Clamp(timer * 0.75f, 1, 1.6f));

        //float y_ = Mathf.Clamp(transform.GetChild(0).transform.position.y, transform.position.y - (2*sightRange), transform.position.y + (2*sightRange));
        //float x_ = Mathf.Clamp(transform.GetChild(0).transform.position.x, transform.position.x - (2*sightRange), transform.position.x + (2*sightRange));

        //transform.GetChild(0).transform.position = new Vector3(x_, y_, transform.GetChild(0).transform.position.z);

        float clampedX = Mathf.Clamp(transform.GetChild(0).transform.position.x, GameManager.instance.mainCamera.transform.position.x - (cameraWidth / 2) + 1.5f, GameManager.instance.mainCamera.transform.position.x + (cameraWidth / 2) - 1.5f);
        float clampedY = Mathf.Clamp(transform.GetChild(0).transform.position.y, GameManager.instance.mainCamera.transform.position.y - (cameraHeight / 2) + 1.5f, GameManager.instance.mainCamera.transform.position.y + (cameraHeight / 2) - 1.5f);

        Vector2 newPos = new Vector2(clampedX, clampedY);

        transform.GetChild(0).transform.position = newPos;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(-transform.parent.rotation.x, -transform.parent.rotation.y, -transform.parent.rotation.z);
    }
}
