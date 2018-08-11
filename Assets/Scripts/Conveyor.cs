using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

	public float Speed;
	public Box[] BoxPrefabs;

	private float SpawnY = -6.5f;
	private float[] SpawnX = new [] { -1.25f, 0, 1.25f };

	private float LastBoxSpawnTime;

	private float randomSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - LastBoxSpawnTime > (1 / Speed) * randomSpawn) {
			//spawn
			//choose spawn location
			var spawnNormal = new Vector2(SpawnX[Random.Range(0, 2)], SpawnY);
			var random = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
			var position = spawnNormal + random;

			//choose box type
			var boxPrefab = BoxPrefabs[ Random.Range(0,2) ];

			Box b = Instantiate(boxPrefab, position, Quaternion.Euler(0,0,Random.Range(0f,360f)));	
			b.Spawn( new Vector2( random.x, -2f + random.y), Speed/10f ); // -2f is the normal ending spot

			randomSpawn = Random.Range(3f, 5f);
		}		
	}
}
