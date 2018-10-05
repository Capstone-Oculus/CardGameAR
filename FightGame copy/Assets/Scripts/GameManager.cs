using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //public GameObject playerPrefab;
    //public PlayerManager[] players;
    public GameObject player1;
    public GameObject player2;

    public int P1Life; // int = whole numbers
    public int P2Life;

    public GameObject p1Wins;
    public GameObject p2Wins;

    public GameObject[] p1Hearts;
    public GameObject[] p2Hearts;

    void Update()
    {
        if (P1Life <= 0)
        {
            player1.SetActive(false);
            p2Wins.SetActive(true); //turn on our gameover screen
        }

        if (P2Life <= 0)
        {
            player2.SetActive(false);
            p1Wins.SetActive(true); //turn on our gameover screen
        }
    }

    // methods that will do damage to players (connect this to losing card or player)
    // whenever this function is called (called by losing card)
    public void HurtP1()
    {
        P1Life -= 1;

        // loop thru the array and one by one decide whether to turn on or off heart
        for (int i = 0; i < p1Hearts.Length; i++)
        {
            if (P1Life > i)
            {
                p1Hearts[i].SetActive(true); // can see heart
            } else {
                p1Hearts[i].SetActive(false); // turn off heart after damage
            }
        }
    }

    public void HurtP2()
    {
        P2Life -= 1;

        for (int i = 0; i < p2Hearts.Length; i++)
        {
            if (P2Life > i)
            {
                p2Hearts[i].SetActive(true);
            } else {
                p2Hearts[i].SetActive(false);
            }
        }
    }
}
