using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonLevel : MonoBehaviour {

	private void Awake () {
        DontDestroyOnLoad(this.transform);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void loadScene (string scene) {
        Debug.LogError("Loading scene " + scene);

        PhotonNetwork.LoadLevel(scene);
	}

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.LogError("test " + scene.name + " mode " + mode);
        Debug.Log("scene name: " + scene.name);
        if (scene.name == "First Scene")
        {
            Debug.Log("got fiiiiiiiiirst");
        }
    }
}
