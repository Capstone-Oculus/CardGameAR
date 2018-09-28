using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Linq;



public class Trackables : MonoBehaviour {
    Dictionary<string, int> map = new Dictionary<string, int>();
    public Text WinText;
    bool showWin = false;
    private Rect mButtonRect = new Rect(200, 200, 400, 600);
    private string winner = "";
    void Start() {
        for (int i = 1; i <= 10; i++) {
            map.Add("card" + i, i);
        }
        map.Add("cardJ", 11);
        map.Add("cardQ", 12);
        map.Add("cardK", 12);
        map.Add("cardA", 14);
    }
    // Update is called once per frame
    void Update()
    {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        if (activeTrackables.Count() < 2) {
            showWin = false;
            return;
        }

        int max = -1;
        winner = "";
        // Iterate through the list of active trackables
        //Debug.Log("List of trackables currently active (tracked): ");
        foreach (TrackableBehaviour tb in activeTrackables)
        {
            int score;
            if (map.TryGetValue(tb.TrackableName, out score)) {
                if (max < score) {
                    max = score;
                    winner = tb.TrackableName;
                }
            }
            Debug.Log("Trackable: " + tb.TrackableName);
        }
        showWin = true;
    }
    void OnGUI()
    {
        if (showWin)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 44;
            // draw the GUI button
            if (GUI.Button(mButtonRect, "Winner is: " + winner, style))
            {
                // do something on button click 
            }
        }
    }

}