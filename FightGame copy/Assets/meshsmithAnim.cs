using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshsmithAnim : MonoBehaviour
{

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {

        if (Input.GetKeyDown("1"))
        {
            anim.Play("shoot_single_ar");
        }

        if (Input.GetKeyDown("3"))
        {
            anim.Play("attack");
        }
    }
}
