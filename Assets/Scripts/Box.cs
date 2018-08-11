using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Box : MonoBehaviour {

	public int Weight;
	public int Stackability;

	public bool Throwable;
	public float ThrowSpeedModifier;

	public float MovespeedModifier;
	private Vector3 movement;
	private Vector3 cv;

	private float rotation;
	private float rv;

	private bool _isAirborn;
	public bool IsAirborn {
		get {
			return _isAirborn;
		}
		set {
			_isAirborn = value;
			gameObject.layer = value ? LayerMask.NameToLayer("Airborn") : LayerMask.NameToLayer("Box");
		}
	}

	private SpriteRenderer outline;
	public Color OutlineColor {
		get {
			return outline.color;
		}
		set {
			outline.color = value;	
		}
	}

	private BoxCollider2D collider;

	[SerializeField]
	private LayerMask CollisionLayers;


	private void Awake() {
		outline = GetComponentsInChildren<SpriteRenderer>(true).First( g => g.gameObject != this.gameObject );	
		collider = GetComponent<BoxCollider2D>();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(Vector3 finalPosition, float speed) {
		collider.enabled = false;
		IsAirborn = true;
		StartCoroutine(MoveToStaging(finalPosition, speed));
	}

	IEnumerator MoveToStaging(Vector3 finalPosition, float speed) {
		while(transform.position.y < finalPosition.y) {
			transform.position += Time.deltaTime * Vector3.up * speed;
			yield return new WaitForEndOfFrame();
		}

		var hit = Physics2D.Raycast(transform.position, Vector2.zero, 0, CollisionLayers);
		Place(hit);
	}

	private void FixedUpdate() {
		if(IsAirborn) {
			movement = Vector3.SmoothDamp(movement, Vector3.zero, ref cv, 0.9f);

			rotation = Mathf.SmoothDamp(rotation, 0, ref rv, 0.9f);
			transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + rotation);

			var hits = Physics2D.BoxCastAll(transform.position, transform.localScale, transform.localRotation.z, movement, movement.magnitude, CollisionLayers);
			if(hits.Length > 0) {
				var dec = 0.3f;
				var abs = Mathf.Abs(hits[0].normal.x) > Mathf.Abs(hits[0].normal.y) ? new Vector2(-dec, dec) : new Vector2(dec, -dec);
				movement *= abs;
				cv *= abs;
			}

			transform.position += movement;
			

			if (movement.magnitude < 0.01f) {
				movement = Vector3.zero;
				IsAirborn = false;
			}
		}
	}

	public void PickedUp(Transform picker) {
		this.transform.parent = picker;
		collider.enabled = false;
		OutlineColor = Color.clear;
		IsAirborn = true;
	}

	public void Place(RaycastHit2D stack) {
		if(stack.collider != null) {
			// we're placing on a stack
		}
		else {
			// we're creating a new stack
		}

		this.transform.parent = null;
		collider.enabled = true;
		IsAirborn = false;
	}

	public void Throw( Vector3 throwerMovement, Vector3 throwDirection ) {
		if(!Throwable) {
			// play sound - can't throw
			return;
		}
		IsAirborn = true;
		movement = (throwerMovement + (throwDirection * ThrowSpeedModifier * Random.Range(8f, 10f))) * Time.fixedDeltaTime;
		rotation = Random.Range(-90f, 90f) * Time.fixedDeltaTime;
		this.transform.parent = null;
		collider.enabled = true;
	}
}
