using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Country", menuName = "Characters/Countries")]
public class Country : ScriptableObject
{
    public Sprite Flag;
    public Sprite Weapon;
    public Sprite Portrait;
    public string Name;
    public string AdmiralName;
    public int Attack;
    public int Defense;
    public int Speed;
}
