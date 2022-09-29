using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipSelectionWheelControl : MonoBehaviour
{
    public float wantedRotation;

    public Image[] buttons = new Image[6];
    public Image selectedButton;
    public Image weaponSymbol;
    public Image portrait;
    public TextMeshProUGUI _name;
    public TextMeshProUGUI admiral;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI speed;
    public PlayerControls controls;
    public int x = 0;
    public bool moved = false;
    public bool isToLeft = false;
    public bool rotating = false;
    public float rotateSpeed = 5f;

    private void Start()
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        for(int i = 0; i < 6; i++)
        {
            buttons[i] = children[i].GetComponentInChildren<Image>();
            buttons[i].sprite = GameManager.instance.countries[i].Flag;
            portrait.sprite = GameManager.instance.countries[i].Portrait;
            _name.text = GameManager.instance.countries[i].Name;
            admiral.text = "Admiral: " + GameManager.instance.countries[i].AdmiralName;
            attack.text = "Attack: " + GameManager.instance.countries[i].Attack + "/10";
            defense.text = "Defense: " + GameManager.instance.countries[i].Defense + "/10";
            speed.text = "Speed: " + GameManager.instance.countries[i].Speed + "/10";
        }

        selectedButton = buttons[0];
    }

    private void FixedUpdate()
    {
        if (wantedRotation < 0)
        {
            wantedRotation += 360;
        }
        else if (wantedRotation >= 360)
        {
            wantedRotation -= 360;
        }

        if (gameObject.transform.rotation.eulerAngles.z < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + 360);
        }
        else if (gameObject.transform.rotation.eulerAngles.z >= 360)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - 360);
        }

        float inputX = -Input.GetAxis(controls.horizontal);

        if (inputX == 0f)
        {
            moved = false;
        }

        if (inputX > 0.5f && !moved)
        {
            if (x + Mathf.RoundToInt(inputX) < 6)
            {
                x++;
                moved = true;
            }
            else
            {
                x = 0;
                moved = true;
            }

            wantedRotation -= 360 / gameObject.transform.childCount;
        }
        else if (inputX < -0.5f && !moved)
        {
            if (x + Mathf.RoundToInt(inputX) >= 0)
            {
                x--;
                moved = true;
            }
            else
            {
                x = 5;
                moved = true;
            }
            wantedRotation += 360 / gameObject.transform.childCount;
        }

        selectedButton = buttons[x];

        foreach(Image i in buttons)
        {
            Color c = i.color;

            if(i == selectedButton)
            {
                c.a = 1f;
            }
            else
            {
                c.a = 0.75f;
            }

            i.color = c;
        }

        //gameObject.transform.rotation = Quaternion.Euler(0,0,wantedRotation);

        if (Mathf.Round(gameObject.transform.rotation.eulerAngles.z) == wantedRotation)
        {

        } else if (gameObject.transform.rotation.eulerAngles.z > wantedRotation)
        {
            if (gameObject.transform.rotation.eulerAngles.z - wantedRotation < 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 1 * rotateSpeed);
            } else if (gameObject.transform.rotation.eulerAngles.z - wantedRotation > 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 1 * rotateSpeed);
            }
        } else if (wantedRotation > gameObject.transform.rotation.eulerAngles.z)
        {
            if (wantedRotation - gameObject.transform.rotation.eulerAngles.z < 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 1 * rotateSpeed);
            } else if (wantedRotation - gameObject.transform.rotation.eulerAngles.z > 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 1 * rotateSpeed);
            }
        }

        //if (gameObject.transform.rotation.eulerAngles.z - wantedRotation > 0.01f && Mathf.Sqrt((gameObject.transform.rotation.eulerAngles.z - wantedRotation) * (gameObject.transform.rotation.eulerAngles.z - wantedRotation)) <= 180)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 1 * rotateSpeed);
        //}
        //else if (Mathf.Sqrt((gameObject.transform.rotation.eulerAngles.z - wantedRotation) * (gameObject.transform.rotation.eulerAngles.z - wantedRotation)) > 0.01f)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 1 * rotateSpeed);
        //}

        weaponSymbol.sprite = GameManager.instance.countries[x].Weapon;
        portrait.sprite = GameManager.instance.countries[x].Portrait;
        _name.text = GameManager.instance.countries[x].Name;
        admiral.text = "Admiral: " + GameManager.instance.countries[x].AdmiralName;
        attack.text = "Attack: " + GameManager.instance.countries[x].Attack + "/10";
        defense.text = "Defense: " + GameManager.instance.countries[x].Defense + "/10";
        speed.text = "Speed: " + GameManager.instance.countries[x].Speed + "/10";
    }
}