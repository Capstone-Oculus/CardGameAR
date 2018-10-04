using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour {

    private Animator anim;                     


    void Start()
    {
        anim = GetComponent<Animator>();

    }

        void Update () 
    {
        if(Input.GetKeyDown("1"))
        {
            anim.Play("fallingback");
        }

        if(Input.GetKeyDown("3"))
        {
            anim.Play("attack");
        }
    }
}
