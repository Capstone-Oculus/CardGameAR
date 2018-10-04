using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private GameObject enemy = null;
    private GameObject me = null;

    public void setMe(GameObject me)
    {
        this.me = me;
        this.maybeChooseWinner();
    }

    public void setEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        this.maybeChooseWinner();
    }

    public void maybeChooseWinner()
    {
        if (enemy == null || me == null)
        {
            return;
        }
        int enemyVal = enemy.GetComponent<Card>().value;
        int myVal = me.GetComponent<Card>().value;
        Debug.Log("Enemy Value: " + enemyVal + " Val: " + myVal + " ");
        if (enemyVal > myVal)
        {
            Debug.Log("You are looser!");
            if (PhotonNetwork.isMasterClient)
            {
                GameObject.Find("Player1").GetComponent<Health>().health--;
            }
            else
            {
                GameObject.Find("Player2").GetComponent<Health>().health--;
            }
        }
        else
        {
            Debug.Log("You are winner!");
            if (PhotonNetwork.isMasterClient)
            {
                GameObject.Find("Player2").GetComponent<Health>().health--;
            }
            else
            {
                GameObject.Find("Player1").GetComponent<Health>().health--;
            }

        }
    }
}
