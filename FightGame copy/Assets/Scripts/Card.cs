using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.AI;

public class Card : Photon.MonoBehaviour, ITrackableEventHandler
{
    private NavMeshAgent agent;
    public GameObject markerGoal;
    public int value;
    //parent position
    Vector3 parentPos;
    protected TrackableBehaviour mTrackableBehaviour;

    private bool updated = false;
    private bool visible = false;
    private string enemyCard = "";

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    // Use this for initialization
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        agent = GetComponent<NavMeshAgent>();
        Debug.Log("Shabnam " + agent + " added " + markerGoal);
    }

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    Debug.LogError("photo instance is " + info.sender.IsLocal);

    //    if (info.sender.IsLocal) { return; }

    //    Debug.Log("photon instantiated " + info);
    //    Transform imageTargetTransform = GameObject.Find("ARCamera").transform;
    //    gameObject.name = gameObject.name + "-photon-clone";
    //    transform.parent = imageTargetTransform;
    //    transform.localPosition = new Vector3(-0.3f, 0f, 0.3f);
    //    transform.localRotation = Quaternion.identity;
    //    transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

    //    transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    //    gameObject.SetActive(true);
    //    //transform.parent = imageTargetTransform;
    //}

    //public void InstantiateOnPhoton() {
    //    var view = GetComponent<PhotonView>();
    //    Debug.Log("view is " + view.isMine);
    //    //if (view.isMine)
    //    //{
    //        var obj = gameObject;
    //        PhotonNetwork.Instantiate(obj.name, obj.transform.position,
    //                                  obj.transform.rotation, 0);
    //    //}
    //}

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click:" + this.value);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(" you clicked on " + hit.collider.gameObject.name);
                Debug.Log(hit.collider.gameObject.transform.GetChild(0));
                Debug.Log(hit.collider.gameObject.transform.GetChild(0).transform.GetChild(0));

                Transform obj = hit.collider.gameObject.transform.GetChild(0);

                var angles = obj.transform.rotation.eulerAngles;
                angles.x += 10 * 10;
                angles.y += 10 * 10;
                angles.z += 10 * 10;

                obj.transform.rotation = Quaternion.Euler(angles);

                if (hit.collider.gameObject.name == "Your 3D Model Name")
                {
                    // Write things you want to do when you click.
                }
            }
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("clicked");
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    ShootRay(ray);
        //}
    }

    void ShootRay(Ray ray)
    {
        RaycastHit rhit;
        bool objectHit = false;
        GameObject gObjectHit = null;
        Debug.Log("ray" + Physics.Raycast(ray, out rhit, 1000.0f));
        if (Physics.Raycast(ray, out rhit, 1000.0f))
        {
            Debug.Log("Ray Shot and hit!");
            objectHit = true;
            gObjectHit = rhit.collider.gameObject;


        }
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Card Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Card Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
        visible = true;
        Debug.Log("enemy " + enemyCard + " visible " + visible);
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().setMe(this.gameObject);
        photonView.RPC("ChangeCardState", PhotonTargets.Others, mTrackableBehaviour.TrackableName, true);
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        visible = false;
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().setMe(null);
        photonView.RPC("ChangeCardState", PhotonTargets.Others, mTrackableBehaviour.TrackableName, false);
    }

    [PunRPC]
    void ChangeCardState(string card, bool show, PhotonMessageInfo info)
    {
        enemyCard = card;
        var enemy = GameObject.Find(card);
        Debug.Log("enemy " + enemyCard + " visible " + visible);
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().setEnemy(show ? enemy : null);

        var rendererComponents = enemy.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = enemy.GetComponentsInChildren<Collider>(true);
        var canvasComponents = enemy.GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = show;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = show;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = show;
        Debug.Log("Show card " + card + " show " + show + " local " + info.sender.IsLocal + " info " + info);
    }

    //void FixedUpdate() {
    //    float moveHorizontal = Input.GetAxis("Horizontal");
    //    float moveVertical = Input.GetAxis("Vertical");
    //    //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    //    //rb.AddForce(movement * speed);

    //    Debug.Log("Shabnam " + agent + " move " + moveVertical + " move " + moveVertical);
    //}

    // Update is called once per frame
    /*
    void MoveToTarget()
    {


        if (markerGoal.active)
        {
            parentPos = markerGoal.transform.parent.position;
            agent.SetDestination(parentPos);
        }
    }
    float MapRange(float s, float a1, float a2, float b1, float b2)
    {
        if (s >= a2) return b2;
        if (s <= a1) return b1; 
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    void PitchCtrl()
    {
        transform.GetChild(0).eulerAngles = new Vector3(
            MapRange(agent.velocity.magnitude, 0f, 2f, 0f, 20f),
            transform.eulerAngles.y,
            transform.eulerAngles.z
            );
    }*/
}