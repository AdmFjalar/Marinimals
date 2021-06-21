using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    private Vector3 centre;
    private Vector3 velocity;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    //private float yRatio;
    //private float xRatio;
    private float smoothTime = 0.5f;

    public float maxZoom = 10f;
    public float minZoom = 40f;

    /// <summary>
    /// Adjusts the camera to zoom in and out as to display all ships and place the camera in the center of all ships.
    /// </summary>
    public void CameraZooming()
    {
        GameManager.instance.FindPlayers();

        List<GameObject> players = GameManager.instance.players;

        maxX = players[0].transform.position.x;
        minX = players[0].transform.position.x;
        maxY = players[0].transform.position.y;
        minY = players[0].transform.position.y;

        foreach (GameObject p in players)
        {
            if (p.transform.position.x > maxX)
            {
                maxX = p.transform.position.x;
            }

            if (p.transform.position.x < minX)
            {
                minX = p.transform.position.x;
            }

            if (p.transform.position.y > maxY)
            {
                maxY = p.transform.position.y;
            }

            if (p.transform.position.y < minY)
            {
                minY = p.transform.position.y;
            }
        }

        centre = new Vector3(minX + ((maxX - minX) / 2), minY + ((maxY - minY) / 2), -10);

        float width = maxX - minX;
        float height = maxY - minY;

        float greatestDistance = 0f;

        if (width >= height)
        {
            greatestDistance = width;
        }
        else if (height > width)
        {
            greatestDistance = height;
        }

        transform.position = Vector3.SmoothDamp(transform.position, centre, ref velocity, smoothTime);

        float newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / (10f + (GameManager.instance.maxBoundary * 2)));

        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, newZoom, Time.deltaTime);
    }
}
