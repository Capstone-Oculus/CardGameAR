using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour {

  public GameObject PlayerUnitPrefab;
	// Use this for initialization
	void Start () {
    if (!isLocalPlayer) {
      return;
    }
    Debug.Log("PlayerObject::Start -- Spawning my own personal unit.");
		//Instantiate(PlayerUnitPrefab);
    CmdSpawnMyUnit();
	}
	
	// Update is called once per frame
	void Update () {
    if (!isLocalPlayer) {
      return;
    }
    
		
	}

  GameObject myPlayerUnit;
  
  [Command]
  void CmdSpawnMyUnit() {
    GameObject go = Instantiate(PlayerUnitPrefab);
    myPlayerUnit = go;
    NetworkServer.Spawn(go);
  }

  void OnGUI() {
    if (isServer)
      GUILayout.Label("Running as a server");
    else
      if (isClient)
        GUILayout.Label("Running as a client");
    }

}
