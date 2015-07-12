using UnityEngine;
using System.Collections;

public class ChasePlayer : State {


    public override void run()
    {
        agent.destination = player.transform.position;
    }
}
