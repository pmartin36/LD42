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
	public bool CanSpawn = true;

	// Use this for initialization
	void Start () {
		GameManager.Instance.Conveyor = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(CanSpawn && Time.time - LastBoxSpawnTime > (1 / Speed) * randomSpawn) {
			//spawn
			//choose spawn location
			var spawnNormal = new Vector2(SpawnX[Random.Range(0, 3)], SpawnY);
			var random = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.05f, 0.05f));
			var position = spawnNormal + random;

			//choose box type
			var boxPrefab = BoxPrefabs[ Random.Range(0,3) ];

			Box b = Instantiate(boxPrefab, position, Quaternion.Euler(0,0,Random.Range(0,360)));	
			b.Spawn( new Vector2( random.x, -2f + random.y), Speed/2f ); // -2f is the normal ending spot

			randomSpawn = Random.Range(5f, 8f);
			LastBoxSpawnTime = Time.time;
		}		
	}
}
