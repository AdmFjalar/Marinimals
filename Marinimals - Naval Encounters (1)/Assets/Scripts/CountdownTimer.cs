using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.StartMultiGame();        
    }
}
