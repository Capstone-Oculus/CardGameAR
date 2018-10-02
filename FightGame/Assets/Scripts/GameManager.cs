using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public GameObject playerPrefab;
    //public PlayerManager[] players;
    public GameObject player1;
    public GameObject player2;

    public int P1Life;
    public int P2Life;

    public GameObject gameOver;
    // create two player objects
    // PlayerManager [] array of players

    void Update()
    {
        if (P1Life <= 0)
        {
            player1.SetActive(false);
            gameOver.SetActive(true); //turn on our gameover screen
        }

        if (P2Life <= 0)
        {
            player2.SetActive(false);
            gameOver.SetActive(true); //turn on our gameover screen
        }
    }

    // some methods that will do some damage to players (connect this to losing card or player)
    // whenever this function is called (called by losing card)
    public void HurtP1()
    {
        P1Life -= 1;
    }

    public void HurtP2()
    {
        P2Life -= 1;
    }
}
