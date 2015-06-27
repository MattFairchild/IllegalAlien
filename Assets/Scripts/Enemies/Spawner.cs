using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	[SerializeField] protected GameObject smallShip;
	[SerializeField] protected GameObject bigShip;
	[SerializeField] protected float spawnInterval = 5.0f;
	[SerializeField] protected float spawnBorderX = 40.0f;
	[SerializeField] protected float spawnBorderZ = 30.0f;

	protected List<EnemyScript> enemies = new List<EnemyScript>();

	void OnDrawGizmos () {
		Color regColor = Gizmos.color;
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(Vector3.zero, 2 * new Vector3(spawnBorderX, 1, spawnBorderZ));
		
		Gizmos.color = regColor;
	}

	// Use this for initialization
	void Start () {
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
		while(isActiveAndEnabled){
			yield return new WaitForSeconds(spawnInterval);
			SpawnShip(Random.value > 0.75f);
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
		enemies.Add((Instantiate(big ? bigShip : smallShip, spawnPos, Quaternion.identity) as GameObject).GetComponent<EnemyScript>());
	}
}
