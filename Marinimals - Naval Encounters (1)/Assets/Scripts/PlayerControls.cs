using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Control Layout", menuName = "Controls/Controls")]
public class PlayerControls : ScriptableObject
{
    public string horizontal;
    public string vertical;

    public string horizontal2;
    public string vertical2;

    public string rightBumper;
    public string leftBumper;

    public string rightTrigger;

    public string aButton;
    public string bButton;
    public string xButton;
    public string yButton;

    public string start;
    public string back;
    public string xboxButton;
}
