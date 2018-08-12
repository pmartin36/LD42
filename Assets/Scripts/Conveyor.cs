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

	private float lastSpeedChange;
	private float startTime;

	private SpriteRenderer spriteRenderer;
	private float offset;

	AudioSource audio;

	// Use this for initialization
	void Start () {
		GameManager.Instance.Conveyor = this;
		lastSpeedChange = Time.time - 5f;
		LastBoxSpawnTime = Time.time + 2f;
		startTime = Time.time;

		spriteRenderer = GetComponent<SpriteRenderer>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(CanSpawn) {		
			offset += Speed / 6f * Time.deltaTime;
			spriteRenderer.material.SetFloat("_Offset", -offset);
			audio.pitch = Mathf.Lerp(1f, 1.85f, (Speed/10f)-1f);
		}
		else {
			audio.mute = true;
		}

		if(CanSpawn && Time.time - LastBoxSpawnTime > randomSpawn) {
			//spawn
			//choose spawn location
			var spawnNormal = new Vector2(SpawnX[Random.Range(0, 3)], SpawnY);
			var random = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(0f, 0.1f));
			var position = spawnNormal + random;

			//choose box type
			var boxPrefab = BoxPrefabs[ Random.Range(0,3) ];

			Box b = Instantiate(boxPrefab, position, Quaternion.Euler(0,0,Random.Range(0,360)));	
			b.Spawn( new Vector2( random.x, -2f + random.y) ); // -2f is the normal ending spot

			randomSpawn = Random.Range(4f, 6f) * (1/Mathf.Pow(Speed, 0.5f));
			LastBoxSpawnTime = Time.time;
		}	
		
		if( Time.time - lastSpeedChange > 10f ) {
			// generate new speed
			float rawSpeed = Mathf.Round((Time.time - startTime - 10) / 30) + Random.Range(-1,2);
			rawSpeed = Mathf.Clamp(rawSpeed, 1, 10);
			Debug.Log("Generating new speed: " + rawSpeed);
			Speed = rawSpeed;	
			GameManager.Instance.SpeedDisplay.UpdateSpeed(rawSpeed);
			lastSpeedChange = Time.time;
		}	
	}
}
