using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    public Image selectedButton;
    public Image[] buttons = new Image[3];
    public Image[] buttons2 = new Image[2];
    public int y = 0;
    public bool moved = false;
    public PlayerControls controls;

    void Start()
    {
        selectedButton = buttons[0];
    }

    void LateUpdate()
    {
        if (!GameManager.instance.inDialogue)
        {
            float inputY = -Input.GetAxis(controls.vertical);

            if (inputY == 0f)
            {
                moved = false;
            }

            if (GameManager.instance.playermode.activeSelf)
            {
                if (inputY > 0.5f && !moved)
                {
                    if (y - 1 >= 0)
                    {
                        y--;
                        moved = true;
                    }
                    else
                    {
                        y = 1;
                        moved = true;
                    }
                }
                else if (inputY < -0.5f && !moved)
                {
                    if (y + 1 <= 1)
                    {
                        y++;
                        moved = true;
                    }
                    else
                    {
                        y = 0;
                        moved = true;
                    }
                }

                selectedButton = buttons2[y];

                foreach (Image i in buttons2)
                {
                    Color c = i.GetComponentInChildren<TextMeshProUGUI>().color;

                    if (i == selectedButton)
                    {
                        c.a = 1f;
                    }
                    else
                    {
                        c.a = 0.5f;
                    }

                    i.GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }
            else if (GameManager.instance.startMenu.activeSelf)
            {
                if (inputY > 0.5f && !moved)
                {
                    if (y - 1 >= 0)
                    {
                        y--;
                        moved = true;
                    }
                    else
                    {
                        y = 2;
                        moved = true;
                    }
                }
                else if (inputY < -0.5f && !moved)
                {
                    if (y + 1 <= 2)
                    {
                        y++;
                        moved = true;
                    }
                    else
                    {
                        y = 0;
                        moved = true;
                    }
                }

                selectedButton = buttons[y];

                foreach (Image i in buttons)
                {
                    Color c = i.GetComponentInChildren<TextMeshProUGUI>().color;

                    if (i == selectedButton)
                    {
                        c.a = 1f;
                    }
                    else
                    {
                        c.a = 0.5f;
                    }

                    i.GetComponentInChildren<TextMeshProUGUI>().color = c;
                }
            }


            //if (Input.GetButtonDown(controls.aButton) && !GameManager.instance.hasStarted/* && GameManager.instance.startMenu.activeSelf*/)
            //{
            //    switch (y)
            //    {
            //        case 0:
            //            GameManager.instance.startMenu.SetActive(false);
            //            GameManager.instance.playermode.SetActive(true);
            //            GameManager.instance.lobby.SetActive(false);
            //            GameManager.instance.settings.SetActive(false);
            //            break;
            //        case 1:
            //            GameManager.instance.startMenu.SetActive(false);
            //            GameManager.instance.playermode.SetActive(false);
            //            GameManager.instance.lobby.SetActive(false);
            //            GameManager.instance.settings.SetActive(true);
            //            break;
            //        case 2:
            //            GameManager.instance.ExitGame();
            //            break;
            //    }
            //}
            /*else */if (Input.GetButtonDown(controls.aButton) && !GameManager.instance.hasStarted /*&& GameManager.instance.playermode.activeSelf*/)
            {
                switch (y)
                {
                    case 0:
                        GameManager.instance.startMenu.SetActive(false);
                        GameManager.instance.playermode.SetActive(false);
                        GameManager.instance.singleplayerLobby.SetActive(false);
                        GameManager.instance.lobby.SetActive(true);
                        GameManager.instance.settings.SetActive(false);
                        break;
                    case 1:
                        GameManager.instance.startMenu.SetActive(false);
                        GameManager.instance.playermode.SetActive(false);
                        GameManager.instance.singleplayerLobby.SetActive(false);
                        GameManager.instance.lobby.SetActive(false);
                        GameManager.instance.settings.SetActive(true);
                        break;
                    case 2:
                        GameManager.instance.ExitGame();
                        break;
                }
            }
        }
    }
}

