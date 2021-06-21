using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField] private int playerID;

    public Color[] playerColors = new Color[4];
    public GameObject player;
    public GameObject tag;
    public PlayerControls controls;
    public LayerMask targetLayers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        tag.GetComponent<SpriteRenderer>().color = playerColors[playerID];

        if (gameObject.GetComponentInChildren<ShipControl>() != null)
        {
            tag.SetActive(true);
            tag.transform.position = new Vector3(GetComponentInChildren<ShipControl>().transform.position.x, GetComponentInChildren<ShipControl>().transform.position.y + 5.5f, 0);
        } else
        {
            tag.SetActive(false);
        }
    }

    public void SetControls()
    {
        if (gameObject.GetComponentInChildren<ShipControl>() != null)
        {
            gameObject.GetComponentInChildren<Rudder>().controls = controls;

            gameObject.GetComponentInChildren<ShipControl>().controls = controls;
            gameObject.GetComponentInChildren<Sight>().controls = controls;

            gameObject.GetComponentInChildren<ShipControl>().currentThrottle = 0;

            gameObject.GetComponentInChildren<Rudder>().transform.rotation = Quaternion.Euler(0, 0, -90);
            gameObject.GetComponentInChildren<ShipControl>().transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.GetComponentInChildren<Sight>().transform.rotation = Quaternion.Euler(0, 0, 0);

            gameObject.GetComponentInChildren<ShipControl>().transform.position = transform.position;
            gameObject.GetComponentInChildren<Sight>().transform.GetChild(0).transform.position = new Vector2(gameObject.GetComponentInChildren<Sight>().transform.position.x, gameObject.GetComponentInChildren<Sight>().transform.position.y + 3f);

            gameObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().material = GameManager.instance.materials[playerID];

            if (gameObject.GetComponentInChildren<Gun>().type == Gun.WeaponType.ram)
            {
                gameObject.GetComponentInChildren<TurretControl>().target.gameObject.SetActive(false);
            }
            else
            {
                gameObject.GetComponentInChildren<TurretControl>().target.gameObject.SetActive(true);
            }

            switch (playerID)
            {
                case 0:
                    gameObject.GetComponentInChildren<ShipControl>().gameObject.layer = LayerMask.NameToLayer("Player1");
                    break;
                case 1:
                    gameObject.GetComponentInChildren<ShipControl>().gameObject.layer = LayerMask.NameToLayer("Player2");
                    break;
                case 2:
                    gameObject.GetComponentInChildren<ShipControl>().gameObject.layer = LayerMask.NameToLayer("Player3");
                    break;
                case 3:
                    gameObject.GetComponentInChildren<ShipControl>().gameObject.layer = LayerMask.NameToLayer("Player4");
                    break;
            }
        }
    }
}
