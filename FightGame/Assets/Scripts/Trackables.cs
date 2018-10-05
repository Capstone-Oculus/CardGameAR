using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using System.IO;
using System.Linq;



public class Trackables : MonoBehaviour
{
    //bool showWin = false;
    //private Rect mButtonRect = new Rect(200, 200, 400, 600);
    //private string winner = "";
    //void Start()
    //{
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    // Get the Vuforia StateManager
    //    StateManager sm = TrackerManager.Instance.GetStateManager();

    //    // Query the StateManager to retrieve the list of
    //    // currently 'active' trackables 
    //    //(i.e. the ones currently being tracked by Vuforia)
    //    IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

    //    if (activeTrackables.Count() < 2)
    //    {
    //        showWin = false;
    //        return;
    //    }

    //    int max = -1;
    //    winner = "";
    //    // Iterate through the list of active trackables
    //    foreach (TrackableBehaviour tb in activeTrackables)
    //    {
    //        int score = tb.GetComponent<Card>().value;
    //        if (max < score)
    //        {
    //            max = score;
    //            winner = tb.TrackableName;
    //        }
    //    }
    //    showWin = true;
    //}
    //void OnGUI()
    //{
    //    if (showWin)
    //    {
    //        GUIStyle style = new GUIStyle();
    //        style.fontSize = 44;
    //        // draw the GUI button
    //        if (GUI.Button(mButtonRect, "Winner is: " + winner, style))
    //        {
    //            // do something on button click 
    //        }
    //    }
    //}

}
