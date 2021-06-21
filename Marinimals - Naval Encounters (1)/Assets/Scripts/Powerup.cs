using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerType { damageBuff, speedBoost, shield }
    public PowerType type = PowerType.damageBuff;

    public void Use(GameObject target)
    {
        switch(type)
        {
            case PowerType.damageBuff:
                GameManager.instance.damageModifier[target.layer - 8] = 2;
                GameManager.instance.startedTimer[target.layer - 8] = true;
                Destroy(gameObject);
                break;
            case PowerType.speedBoost:
                target.GetComponent<ShipControl>().AddSpeedBoost();
                Destroy(gameObject);
                break;
            case PowerType.shield:
                gameObject.GetComponent<Stats>().Shield();
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
