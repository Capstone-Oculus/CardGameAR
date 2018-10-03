using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {

    public Button joinButton;
    public Button createButton;
    public InputField roomName;

    public void onClickJoinRoom() {
        if (roomName.text.Length == 0)
        {
            return;
        }

        Debug.Log("join room " + roomName.text);
    }

    public void onClickCreateRoom() {
        if (roomName.text.Length == 0)
        {
            return;
        }
        Debug.Log("create room " + roomName.text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, null);
    }

}
