using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	[SerializeField] protected GameObject smallShip;
	[SerializeField] protected GameObject bigShip;
	[SerializeField] protected float spawnInterval = 5.0f;
	[SerializeField] protected float spawnBorderX = 40.0f;
	[SerializeField] protected float spawnBorderZ = 30.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnShips());
	}

	protected IEnumerator SpawnShips () {
		while(true){
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
		Instantiate(big ? bigShip : smallShip, spawnPos, Quaternion.identity);
	}
}
