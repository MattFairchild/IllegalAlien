using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class DoNothing : State
{
    public override void run()
    {
        Debug.Log("Nothing is being done");
    }

}