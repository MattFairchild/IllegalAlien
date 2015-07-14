﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private float time = 0.0f;
    private bool change = false;

    public float minRange = 30.0f;

    public State[] allStates;
    public State currentState;

    private Transform player, station;
    private float playerDistance, stationDistance;

    void Awake()
    {
        currentState = allStates[1];
    }
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //station = GameObject.FindGameObjectWithTag("SpaceStation").transform;
		player = GameManager.player.transform;
		station = GameManager.spaceStation.transform;
        agent = GetComponent<NavMeshAgent>();

        //update variables needed to decide on next state
        playerDistance = Vector3.Magnitude(player.position - transform.position);
        stationDistance = Vector3.Magnitude(station.position - transform.position);

        //currentState = allStates[1];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5.0f)
        {
            time = 0.0f;
            change = !change;
        }

        //decide();
        run();
        turnToTarget();
    }

    public void run()
    {
        currentState.run();
    }

    public void decide()
    {
        if (change)
        {
            Debug.Log("STATE CHANGED");
            changeState("DoNothing");
        }
        else
        {
            Debug.Log("STATE CHANGED");
            changeState("ChaseTarget");
        }

    }

    public void changeState(string stateName)
    {
        if (currentState.gameObject.name == stateName)
        {
            return;
        }

        foreach (State state in allStates)
        {
            if (state.gameObject.name == stateName)
            {
                currentState = state;
            }
        }
    }

    public void SetupConvoy(GameObject leader, bool wingman)
    {
        Convoy convoyState = allStates[0] as Convoy;
        convoyState.SetLeader(leader);
        convoyState.SetWingman(wingman);
        currentState = convoyState;
    }
    void turnToTarget()
    {
        //turns to target if destination is reached

        if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.25f)
        {
            agent.updateRotation = false;
			Quaternion newRot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.destination - transform.position), Time.deltaTime * agent.angularSpeed * 0.005f);
			newRot.eulerAngles = new Vector3(0, newRot.eulerAngles.y, 0);
			transform.rotation = newRot;
        }
        else
        {
            agent.updateRotation = true;
        }
    }

}

