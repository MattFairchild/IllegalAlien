using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	[SerializeField] protected GameObject interceptor;
    //[SerializeField] protected GameObject interceptorSpreadAttack;
    //[SerializeField] protected GameObject interceptorConvoy;
	[SerializeField] protected GameObject destroyer;
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
            //yield return new WaitForSeconds(spawnInterval);
            //SpawnShip(Random.value > 0.75f);

            SpawnShipSophisticated(level[spawnIndex].ship);          
            yield return new WaitForSeconds(level[spawnIndex].timeTillNext);
		}
	}
	protected void SpawnShipSophisticated(SpawnableShips ship)
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
                    GameObject small = Instantiate(interceptor, spawnPos, Quaternion.identity) as GameObject;
                    small.GetComponent<AI>().changeState("ChasePlayer");
                    enemies.Add(small.GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.destroyer:
                    enemies.Add((Instantiate(destroyer, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.convoy:
                    GameObject big = Instantiate(destroyer, spawnPos, Quaternion.identity) as GameObject;
                    GameObject small1, small2 = null;

                    small1 = Instantiate(interceptor, big.transform.TransformPoint(new Vector3(-5f, 0, 0)), Quaternion.identity) as GameObject;
                    small2 = Instantiate(interceptor, big.transform.TransformPoint(new Vector3(5f, 0, 0)), Quaternion.identity) as GameObject;               
                    
                    small1.GetComponent<AI>().SetupConvoy(big, true);
                    small2.GetComponent<AI>().SetupConvoy(big, false);

                    enemies.Add(big.GetComponent<EnemyScript>());
                    enemies.Add(small1.GetComponent<EnemyScript>());
                    enemies.Add(small2.GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.spreadAttack:
                    //GameObject spread1 = Instantiate(interceptorSpreadAttack, new Vector3(spawnPos.x + 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
                    //GameObject spread2 = Instantiate(interceptorSpreadAttack, new Vector3(spawnPos.x - 2f, 0, transform.position.z), Quaternion.identity) as GameObject;
                    //spread1.GetComponent<SpreadAttack>().wingman = spread2;
                    //spread1.GetComponent<SpreadAttack>().squadLeader = true;
                    //spread2.GetComponent<SpreadAttack>().wingman = spread1;
                    //spread2.GetComponent<SpreadAttack>().squadLeader = false;
                    //enemies.Add(spread1.GetComponent<EnemyScript>());
                    //enemies.Add(spread2.GetComponent<EnemyScript>());
                    break;

                case SpawnableShips.spreadSpawn:
                    enemies.Add((Instantiate(interceptor, new Vector3(GameManager.player.transform.position.x, 0, -spawnBorderZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(GameManager.player.transform.position.x, 0, spawnBorderZ), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(spawnBorderX, 0, GameManager.player.transform.position.z), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    enemies.Add((Instantiate(interceptor, new Vector3(-spawnBorderX, 0, GameManager.player.transform.position.z), Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
                    break;

            }
            spawnIndex++;
			spawnIndex %= level.Length;
        }
    }

	protected void SpawnShipSimple (bool big) {
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
		enemies.Add((Instantiate(big ? destroyer : interceptor, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
	}
}
