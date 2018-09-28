using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Card10 : MonoBehaviour {
    private NavMeshAgent agent;
    public GameObject markerGoal;
    //parent position
    Vector3 parentPos;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("Shabnam " + agent + " added " + markerGoal);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(" you clicked on " + hit.collider.gameObject.name);
                Debug.Log(hit.collider.gameObject.transform.GetChild(0));
                Debug.Log(hit.collider.gameObject.transform.GetChild(0).transform.GetChild(0));

                Transform obj = hit.collider.gameObject.transform.GetChild(0);

                //.gameObject.transform.GetChild(0);
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


    //void FixedUpdate() {
    //    float moveHorizontal = Input.GetAxis("Horizontal");
    //    float moveVertical = Input.GetAxis("Vertical");
    //    //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    //    //rb.AddForce(movement * speed);

    //    Debug.Log("Shabnam " + agent + " move " + moveVertical + " move " + moveVertical);
    //}

    // Update is called once per frame
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
    }
}