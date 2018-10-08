using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PhotonConnect : MonoBehaviour
{
    public string versionName = "0.1";
    public Button joinButton;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        joinButton.enabled = false;
        PhotonNetwork.ConnectUsingSettings(versionName);
        Debug.Log("connecting to photon");
    }

    private void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        joinButton.enabled = true;
        Debug.Log("On Joined Lobby");
    }

    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("disconnected from photon");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Connected. Players in room: " + PhotonNetwork.playerList.Length);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Connected. Players in room: " + PhotonNetwork.playerList.Length);
    }

    public void Update()
    {
        if (PhotonNetwork.connected)
        {
            joinButton.enabled = true;
        }
    }
}
