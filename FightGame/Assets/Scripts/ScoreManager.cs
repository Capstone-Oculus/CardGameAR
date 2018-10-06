using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public string mainMenu;

    private GameObject enemy = null;
    private GameObject me = null;

    public Health p1Health = null;
    public Health p2Health = null;

    public GameObject p1Wins = null;
    public GameObject p2Wins = null;

    private bool gameOver = false;
    private int round = 1;

    public GameObject countDownObj = null;
    public Text countDownText = null;
    private bool inCountDown = false;
    private double countDownStartTime = 0;

    private bool iLost = false;

    private Dictionary<int, bool> seen = new Dictionary<int, bool>();

    public void Start()
    {
        //p1Health = GameObject.Find("Player1").GetComponent<Health>();
        //p2Health = GameObject.Find("Player2").GetComponent<Health>();

        p2Wins.SetActive(false);
        p1Wins.SetActive(false);
        countDownObj.SetActive(false);
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

        //enemy.transform.

        //transform.localPosition = new Vector3(0, -1, 2);
        //transform.localRotation = Quaternion.Euler(180, 0, 0);
        //enemy.transform.localPosition = new Vector3(0, -1, 2);
        //enemy.transform.localRotation = Quaternion.Euler(180, 0, 0);


        int enemyVal = enemy.GetComponent<Card>().value;
        int myVal = me.GetComponent<Card>().value;
        if (seen.ContainsKey(enemyVal))
        {
            Debug.Log("enemy previously seen");
            return;

        }
        if (seen.ContainsKey(myVal))
        {
            Debug.Log("I have been previously seen");
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
            Debug.Log("You are looser!");
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
            Debug.Log("You are winner!");
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
            Debug.Log("next round is round #" + round);
        }

    }


    public void Update()
    {
        if (enemy != null && me != null)
        {
            //me.transform.localPosition = new Vector3(0, -1, 2);
            //me.transform.localRotation = Quaternion.Euler(180, 0, 0);
            enemy.transform.localPosition = new Vector3(0, -1, 2);
            enemy.transform.localRotation = Quaternion.Euler(180, 0, 0);

            //var rotationSpeed = 5.0; //degrees per second
            //enemy.transform.RotateAround(me.transform.position, new Vector3(0, 1, 0), (float)rotationSpeed * Time.deltaTime);
            enemy.transform.LookAt(me.transform);

            //var enemyRotation = Quaternion.LookRotation(me.transform.position - enemy.transform.position);
            //enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemyRotation, 1);
        }
        double timeNow = Time.realtimeSinceStartup;
        var myWin = PhotonNetwork.isMasterClient ? p1Wins : p2Wins;
        var enemyWin = PhotonNetwork.isMasterClient ? p2Wins : p1Wins;

        if (gameOver)
        {
            countDownText.text = "";
            countDownObj.SetActive(false);
            inCountDown = false;

            if (timeNow - countDownStartTime < 6)
            {
                if (iLost)
                {
                    enemyWin.SetActive(true);
                }
                else
                {
                    myWin.SetActive(true);
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
        if (timeNow - countDownStartTime < 4)
        {
            // do nothing.
        }
        else if (timeNow - countDownStartTime < 5)
        {
            countDownText.text = "3";
        }
        else if (timeNow - countDownStartTime < 6)
        {
            countDownText.text = "2";
        }
        else if (timeNow - countDownStartTime < 7)
        {
            countDownText.text = "1";
        }
        else
        {
            countDownText.text = "";
            countDownObj.SetActive(false);
            inCountDown = false;
        }
    }

}
