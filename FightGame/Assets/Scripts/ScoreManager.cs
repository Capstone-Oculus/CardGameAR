using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public string mainMenu;

    private GameObject enemy = null;
    private GameObject me = null;

    public Text p1 = null;
    public Text p2 = null;

    public Health p1Health = null;
    public Health p2Health = null;

    public GameObject p1Wins = null; // or just p1Wins;
    public GameObject p2Wins = null; // or just p2Wins;

    public GameObject usedCard = null; // or just usedCard;
    private double usedCardStartTime = 0;

    private bool gameOver = false;
    private int round = 1;

    public GameObject countDownObj = null;
    public Text countDownText = null;
    public Text roundText = null;
    private bool inCountDown = false;
    private double countDownStartTime = 0;

    private bool iLost = false;

    private Dictionary<int, bool> seen = new Dictionary<int, bool>();

    public void Start()
    {
        p2Wins.SetActive(false);
        p1Wins.SetActive(false);
        countDownObj.SetActive(false);
        p1.text = PhotonNetwork.isMasterClient ? "You" : "Opponent";
        p2.text = PhotonNetwork.isMasterClient ? "Opponent" : "You";
    }

    public void setMe(GameObject me)
    {
        this.me = me;
        this.maybeChooseWinner();
    }

    public void setEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        if (enemy != null)
        {
            enemy.transform.localPosition = new Vector3(0, -1, 2);
            enemy.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }
        this.maybeChooseWinner();
    }

    private void kill(GameObject player)
    {
        foreach (Transform child in player.transform)
        {
            child.gameObject.GetComponent<Animator>().Play("death");
        }
    }

    private void attack(GameObject player)
    {
        foreach (Transform child in player.transform)
        {
            child.gameObject.GetComponent<Animator>().Play("attack");
        }
    }

    public void maybeChooseWinner()
    {
        if (enemy == null || me == null || gameOver)
        {
            return;
        }

        int enemyVal = enemy.GetComponent<Card>().value;
        int myVal = me.GetComponent<Card>().value;
        if (seen.ContainsKey(enemyVal))
        {
            //Debug.Log("enemy previously seen");
            usedCard.SetActive(true); 
            usedCardStartTime = Time.realtimeSinceStartup;
            return;

        }
        if (seen.ContainsKey(myVal))
        {
            //Debug.Log("I have been previously seen");
            usedCard.SetActive(true);
            usedCardStartTime = Time.realtimeSinceStartup;
            return;
        }
        seen.Add(enemyVal, true);
        seen.Add(myVal, true);
        Debug.Log("Enemy Value: " + enemyVal + " Val: " + myVal + " ");

        var myHealth = PhotonNetwork.isMasterClient ? p1Health : p2Health;
        var enemeyHealth = PhotonNetwork.isMasterClient ? p2Health : p1Health;
        var myWin = PhotonNetwork.isMasterClient ? p1Wins : p2Wins;
        var enemyWin = PhotonNetwork.isMasterClient ? p2Wins : p1Wins;
        iLost = enemyVal > myVal;
        if (iLost)
        {
            attack(enemy);
            kill(me);
            myHealth.health--;
            if (myHealth.health == 0)
            {
                gameOver = true;
            }
        }
        else
        {
            kill(enemy);
            attack(me);
            enemeyHealth.health--;
            if (enemeyHealth.health == 0)
            {
                gameOver = true;
            }
        }

        if (gameOver)
        {
            Debug.Log("game is over " + iLost + " win " + enemyWin + " my win " + myWin);
            countDownStartTime = Time.realtimeSinceStartup;
        }
        else
        {
            round++;
            inCountDown = true;
            countDownObj.SetActive(true);
            countDownStartTime = Time.realtimeSinceStartup;
        }

    }


    public void Update()
    {
        if (Time.realtimeSinceStartup - usedCardStartTime > 2) {
            usedCard.SetActive(false);
        } 
        if (enemy != null && me != null)
        {
            //me.transform.localPosition = new Vector3(0, -1, 2);
            //me.transform.localRotation = Quaternion.Euler(180, 0, 0);
            //enemy.transform.localPosition = new Vector3(0, -1, 2);
            //enemy.transform.localRotation = Quaternion.Euler(180, 0, 0);
            enemy.transform.position = me.transform.position + me.transform.forward * 2;
            enemy.transform.LookAt(me.transform);


            //var rotationSpeed = 5.0; //degrees per second
            //enemy.transform.RotateAround(me.transform.position, new Vector3(0, 1, 0), (float)rotationSpeed * Time.deltaTime);
            //enemy.transform.LookAt(me.transform);

            //var enemyRotation = Quaternion.LookRotation(me.transform.position - enemy.transform.position);
            //enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemyRotation, 1);
        }
        double timeNow = Time.realtimeSinceStartup;
        //var myWin = PhotonNetwork.isMasterClient ? p1Wins : p2Wins;
        //var enemyWin = PhotonNetwork.isMasterClient ? p2Wins : p1Wins;

        if (gameOver)
        {
            countDownText.text = "";
            countDownObj.SetActive(false);
            inCountDown = false;

            if (timeNow - countDownStartTime < 6)
            {
                if (iLost)
                {
                    //enemyWin.SetActive(true);
                    p2Wins.SetActive(true); // added this!! have to check if this works
                }
                else
                {
                    //myWin.SetActive(true);
                    p1Wins.SetActive(true); // added this!! have to check if this works
                }
            }
            else
            {
                SceneManager.LoadScene(mainMenu);
                //PhotonNetwork.LeaveRoom();
            }
            return;
        }

        if (!inCountDown)
        {
            return;
        }
        var delta = timeNow - countDownStartTime;
        if (delta < 2)
        {
            // do nothing.
        }
        else if (delta < 3)
        {
            countDownText.fontSize = fontSize(delta - 2);
            countDownText.text = "Ready?";
        }
        else if (delta < 4)
        {
            countDownText.fontSize = fontSize(delta - 3);
            countDownText.text = "Set...";
        }
        else if (delta < 5)
        {
            countDownText.fontSize = fontSize(delta - 4);
            countDownText.text = "Go!";
        }
        else
        {
            countDownText.text = "";
            roundText.text = "Round " + round.ToString();
            countDownObj.SetActive(false);
            inCountDown = false;
        }
    }

    int fontSize(double time) {
        return (int)(200 * time * time);
    }

}
