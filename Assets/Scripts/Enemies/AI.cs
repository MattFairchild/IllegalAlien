using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour {

    public float minRange = 30.0f;

    private State currentState;
    private Transform player, station;
    private NavMeshAgent agent;
    private float playerDistance, stationDistance;

    stationaryBattle sb;

    void Start()
    {
        sb = new stationaryBattle();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        station = GameObject.FindGameObjectWithTag("SpaceStation").transform;
        agent = GetComponent<NavMeshAgent>();

        //update variables needed to decide on next state
        playerDistance = Vector3.Magnitude(player.position - transform.position);
        stationDistance = Vector3.Magnitude(station.position - transform.position);

        currentState = new DoNothing();
    }

	// Update is called once per frame
	void Update ()
    {
        decide();
        run();
	}

    public void run()
    {
        currentState.run();
    }

    public void decide()
    {
        if (playerDistance < minRange)
            changeState(sb);
    }

    public void changeState(State desiredState)
    {
        System.Type type = desiredState.GetType();

        if (!currentState.Equals(type))
        {
            currentState = desiredState;
        }
    }
}

