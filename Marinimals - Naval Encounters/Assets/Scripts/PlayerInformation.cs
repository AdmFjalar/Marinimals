using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInformation : MonoBehaviour
{
    public GameObject player;
    public Image portrait;
    public Image throttle;
    public TextMeshProUGUI points;
    public Slider cooldownSlider;
    public Slider healthSlider;
    public float cooldown;
    public float health;
    public int ID;
    public int currentSpeed;

    public Sprite[] speeds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerContainers[ID].GetComponentInChildren<Gun>() != null)
        {
            cooldown = GameManager.instance.playerContainers[ID].GetComponentInChildren<Gun>().timer / GameManager.instance.playerContainers[ID].GetComponentInChildren<Gun>().delay;
            cooldownSlider.value = cooldown;
        }

        if (player.GetComponentInChildren<ShipControl>() != null)
        {
            currentSpeed = player.GetComponentInChildren<ShipControl>().currentThrottle + 3;

            healthSlider.value = (float)player.GetComponentInChildren<Stats>().health / player.GetComponentInChildren<Stats>().maxHealth;
        }

        throttle.sprite = speeds[currentSpeed];

        portrait.sprite = GameManager.instance.countries[GameManager.instance.shipSelection[ID].x].Portrait;

        //health.text = "HP : " + player.GetComponentInChildren<Stats>()?.health + " / " + player.GetComponentInChildren<Stats>()?.GetComponent<Stats>().maxHealth;
        points.text = "POINTS : " + player.GetComponentInChildren<Stats>()?.points;
    }
}
