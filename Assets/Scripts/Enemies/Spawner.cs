using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	[SerializeField] protected GameObject interceptor;
    [SerializeField] protected GameObject interceptorSpreadAttack;
    [SerializeField] protected GameObject interceptorConvoy;
	[SerializeField] protected GameObject destroyerStandard;
	[SerializeField] protected float spawnInterval = 5.0f;
	[SerializeField] protected float spawnBorderX = 40.0f;
	[SerializeField] protected float spawnBorderZ = 30.0f;

	protected List<EnemyScript> enemies = new List<EnemyScript>();

    public enum SpawnableShips
    {
        interceptor,
        destroyer,
        convoy,
        spreadAttack,
        spreadSpawn
    };
    private int spawnIndex;
    [System.Serializable] public class spawn
    {
        public SpawnableShips ship;
        public float timeTillNext;
    }
    public spawn[] level;

	void OnDrawGizmos () {
		Color regColor = Gizmos.color;
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(Vector3.zero, 2 * new Vector3(spawnBorderX, 1, spawnBorderZ));
		
		Gizmos.color = regColor;
	}

	// Use this for initialization
	void Start () {
        spawnIndex = 0;
		StartCoroutine(SpawnShips());
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.K)){
			foreach(EnemyScript enemy in enemies){
				if(enemy) { enemy.Hit(9001); }
			}
			enemies.Clear();
		}
	}

	protected IEnumerator SpawnShips () {
        yield return new WaitForSeconds(5f);
		while(isActiveAndEnabled)
        {
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(spawnInterval);
            //SpawnShip(Random.value > 0.75f);

            SpawnShip2(level[spawnIndex].ship);          
            yield return new WaitForSeconds(level[spawnIndex].timeTillNext);
		}
	}
    protected void SpawnShip2(SpawnableShips ship)
    {
        {
            float spawnX, spawnZ;

            if (Random.value > 0.5f)
            {
                spawnX = spawnBorderX * (Random.value > 0.5f ? 1 : -1);
                spawnZ = Random.Range(-spawnBorderZ, spawnBorderZ);
            }
            else
            {
                spawnX = Random.Range(-spawnBorderX, spawnBorderX);
                spawnZ = spawnBorderZ * (Random.value > 0.5f ? 1 : -1);
            }
            Vector3 spawnPos = new Vector3(spawnX, 0, spawnZ);

            switch (ship)
            {
                case SpawnableShips.interceptor:
                    enemies.Add((Instantiate(interceptor, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.destroyer:
                    enemies.Add((Instantiate(destroyerStandard, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.convoy:
                    GameObject big = Instantiate(destroyerStandard, spawnPos, Quaternion.identity) as GameObject;
                    GameObject small1, small2 = null;
                    if (Mathf.Abs(spawnPos.x) < Mathf.Abs(spawnPos.z))
                    {
                        small1 = Instantiate(interceptorConvoy, new Vector3(spawnPos.x + 4f, 0, spawnPos.z), Quaternion.identity) as GameObject;
                        small2 = Instantiate(interceptorConvoy, new Vector3(spawnPos.x - 4f, 0, spawnPos.z), Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        small1 = Instantiate(interceptorConvoy, new Vector3(spawnPos.x, 0, spawnPos.z + 2f), Quaternion.identity) as GameObject;
                        small2 = Instantiate(interceptorConvoy, new Vector3(spawnPos.x, 0, spawnPos.z - 2f), Quaternion.identity) as GameObject;
                    }


                    enemies.Add(big.GetComponent<EnemyScript>());
                    enemies.Add(small1.GetComponent<EnemyScript>());
                    enemies.Add(small2.GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.spreadAttack:
                    GameObject spread1 = Instantiate(interceptorSpreadAttack, new Vector3(spawnPos.x + 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
                    GameObject spread2 = Instantiate(interceptorSpreadAttack, new Vector3(spawnPos.x - 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
                    spread1.GetComponent<SpreadAttack>().wingman = spread2;
                    spread1.GetComponent<SpreadAttack>().squadLeader = true;
                    spread2.GetComponent<SpreadAttack>().wingman = spread1;
                    spread2.GetComponent<SpreadAttack>().squadLeader = false;
                    enemies.Add(spread1.GetComponent<EnemyScript>());
                    enemies.Add(spread2.GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.spreadSpawn:
                    enemies.Add((Instantiate(interceptor, new Vector3(-spawnX, 0, -spawnZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(-spawnX, 0, spawnZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(spawnX, 0, -spawnZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(spawnX, 0, spawnZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    break;

            }
            spawnIndex++;
        }
    }

	protected void SpawnShip (bool big) {
		float spawnX, spawnZ;

		if(Random.value > 0.5f){
			spawnX = spawnBorderX * (Random.value > 0.5f ? 1 : -1);
			spawnZ = Random.Range(-spawnBorderZ, spawnBorderZ);
		}
		else{
			spawnX = Random.Range(-spawnBorderX, spawnBorderX);
			spawnZ = spawnBorderZ * (Random.value > 0.5f ? 1 : -1);
		}
		Vector3 spawnPos = new Vector3(spawnX, 0, spawnZ);
		enemies.Add((Instantiate(big ? destroyerStandard : interceptor, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
        //if (big)
        //{
        //    enemies.Add((Instantiate(bigShip,spawnPos,Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
        //}
        //else
        //{
        //    GameObject small1 = Instantiate(smallShip2, new Vector3(spawnPos.x + 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
        //    GameObject small2 = Instantiate(smallShip2, new Vector3(spawnPos.x - 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
        //    small1.GetComponent<SpreadAttack>().wingman = small2;
        //    small1.GetComponent<SpreadAttack>().squadLeader = true;
        //    small2.GetComponent<SpreadAttack>().wingman = small1;
        //    small2.GetComponent<SpreadAttack>().squadLeader = false;
        //    enemies.Add(small1.GetComponent<EnemyScript>());
        //    enemies.Add(small2.GetComponent<EnemyScript>());
        //}
	}
}
