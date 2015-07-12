using UnityEngine;
using System.Collections;

public class Convoy : State {

    protected GameObject convoyLeader;
    protected bool wingman;
    protected DestroyerScript ds;
    protected bool combat;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    public override void run()
    {
        if (convoyLeader == null)
        {
            //change behaviour to chase player         
            parent.GetComponent<AI>().changeState("ChasePlayer");
        }
        else
        {
            if (ds.enemiesInRange.Count > 0)
            {
                Attack();
            }
            else
            {
                Escort();
            }
        }
    }

    void Escort()
    {
        if (wingman)
        {
            agent.destination = convoyLeader.transform.TransformPoint(new Vector3(-5f, 0, 0));
        }
        else
        {
            agent.destination = convoyLeader.transform.TransformPoint(new Vector3(5f, 0, 0));
        }
        
        agent.stoppingDistance = 0.5f;
        agent.autoBraking = true;
    }

    void Attack()
    {
        GameObject target = ChooseTarget();
        agent.autoBraking = true;
        agent.destination = target.transform.position;
        agent.stoppingDistance = 6f;
    }

    GameObject ChooseTarget()
    {
        GameObject pickedEnemy = null;
        
        float distance = 100f;
        for (int i = 0; i < ds.enemiesInRange.Count; i++)
        {
            if (Vector3.Distance(convoyLeader.transform.position, ds.enemiesInRange[i].transform.position) < distance)
            {
                distance = Vector3.Distance(convoyLeader.transform.position, ds.enemiesInRange[i].transform.position);
                pickedEnemy = ds.enemiesInRange[i];
            }
        }
        return pickedEnemy;
    }

    public void SetLeader(GameObject leader)
    {
        convoyLeader = leader;
        ds = convoyLeader.GetComponent<DestroyerScript>();
    }

    public void SetWingman(bool wingman)
    {
        this.wingman = wingman;
    }

    
}
