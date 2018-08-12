using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Box : MonoBehaviour {

	public int Weight;
	public bool Stackable;

	public bool Throwable;
	public float ThrowSpeedModifier;

	public float MovespeedModifier;
	private Vector3 movement;
	private Vector3 cv;

	private float rotation;
	private float rv;

	private static int BoxesCreated;
	public int BoxNum;
	public int StackIndex;

	public bool Interactable = true;
	public bool Spawning = true;
	public bool StopSpawn = false;
	public bool Carried = false;

	public static BoxAudioManager BoxAudioManager;

	private bool _isAirborn;
	public bool IsAirborn {
		get {
			return _isAirborn;
		}
		set {
			_isAirborn = value;
			gameObject.layer = value ? LayerMask.NameToLayer("Airborn") : LayerMask.NameToLayer("Box");
			Interactable = !value;
		}
	}

	public SpriteRenderer spriteRenderer;

	private SpriteRenderer outline;
	public Color OutlineColor {
		get {
			return outline.color;
		}
		set {
			outline.color = value;	
		}
	}

	public BoxCollider2D collider;

	[SerializeField]
	private LayerMask CollisionLayers;


	private void Awake() {
		outline = GetComponentsInChildren<SpriteRenderer>(true).First( g => g.gameObject != this.gameObject );	
		spriteRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();	
	}

	private void InStack() {
		// this is the base of the stack
		if(!GameManager.Instance.BoxStacks.ContainsKey(StackIndex)) {
			CreateNewBoxStack();
		}
		else {		
			var stack = GameManager.Instance.BoxStacks[StackIndex];
			var tempStack = new Stack<Box>();
			while ( stack.Count > 0 && stack.Peek().BoxNum > this.BoxNum ) {
				tempStack.Push(stack.Pop());
			}
			GameManager.Instance.BoxStacks[StackIndex].Push(this);
			while(tempStack.Count > 0) {
				stack.Push(tempStack.Pop());
			}

		}
		Spawning = false;
	}

	private void CreateNewBoxStack() {
		
		var boxstacks = GameManager.Instance.BoxStacks;
		if (boxstacks.ContainsKey(StackIndex)) {
			Debug.Log("Pushing to Stack Index " + this.StackIndex);
			boxstacks[StackIndex].Push(this);
		}
		else {
			Debug.Log("Creating new stack with Stack Index " + this.StackIndex);
			var boxStack = new BoxStack();
			boxStack.Push(this);
			GameManager.Instance.BoxStacks.Add(StackIndex, boxStack);
		}
	}

	void Start () {
		if (BoxNum < 0) {
			BoxNum = BoxesCreated++;
		}
		else {
			//initial boxes
			BoxesCreated++;
			InStack();
		}

		if(BoxAudioManager == null) {
			BoxAudioManager = FindObjectOfType<BoxAudioManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(Vector3 finalPosition) {
		collider.enabled = false;
		IsAirborn = true;
		StartCoroutine(MoveToStaging(finalPosition));
	}

	IEnumerator MoveToStaging(Vector3 finalPosition) {
		spriteRenderer.sortingOrder = 9;
		while (transform.position.y < finalPosition.y) {
			if(StopSpawn) break;
			transform.position += Time.deltaTime * Vector3.up * Mathf.Pow(GameManager.Instance.Conveyor.Speed, 0.9f) / 2f;
			yield return new WaitForEndOfFrame();
		}

		if(!StopSpawn) {
			var hit = Physics2D.Raycast(transform.position, Vector2.zero, 0, CollisionLayers);
			Place(hit.collider?.GetComponent<Box>(), false);
			Spawning = false;
		}
	}

	private void FixedUpdate() {
		if(IsAirborn && !Spawning && !Carried) {
			movement = Vector3.SmoothDamp(movement, Vector3.zero, ref cv, 0.9f);

			rotation = Mathf.SmoothDamp(rotation, 0, ref rv, 0.9f);
			transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + rotation);

			var hits = Physics2D.BoxCastAll(transform.position, transform.localScale, transform.localRotation.z, movement, movement.magnitude, CollisionLayers);
			if(hits.Length > 0) {
				Debug.Log(movement.sqrMagnitude);
				if (!Throwable) {
					if (movement.sqrMagnitude > 0.007f) {
						//play glass crashing sound
						GameManager.Instance.PlayerLost("You broke a fragile box!");
					}
				}

				var dec = 0.3f;
				var abs = Mathf.Abs(hits[0].normal.x) > Mathf.Abs(hits[0].normal.y) ? new Vector2(-dec, dec) : new Vector2(dec, -dec);
				movement *= abs;
				cv *= abs;		
			}

			transform.position += movement;
			

			if (movement.magnitude < 0.01f) {
				movement = Vector3.zero;
				Place(null, playSound: false);
			}
		}
	}

	public void PickedUp(Transform picker) {
		GameManager.Instance.BoxStacks[StackIndex].Pop();
		//this.transform.parent = picker;
		collider.enabled = false;
		OutlineColor = Color.clear;
		IsAirborn = true;
		Carried = true;

		GameManager.Instance.BoxDisplaced(!StopSpawn ? 0 : -1);
		StopSpawn = true;

		spriteRenderer.sortingOrder = 10;
	}

	public void Place(Box stack, bool countAsStacked = true, bool playSound = true) {
		if(stack != null) {
			// we're adding to a stack
			var stackBox = stack.collider.GetComponent<Box>();
			if(stackBox != null) {
				Debug.Log("Pushing to Stack Index " + stackBox.StackIndex);
				GameManager.Instance.BoxStacks[stackBox.StackIndex].Push(this);
				this.StackIndex = stackBox.StackIndex;
			}
			else {
				StackIndex = BoxNum;
				CreateNewBoxStack();
			}		
		}
		else {
			// we're creating a new stack
			StackIndex = BoxNum;
			CreateNewBoxStack();
		}

		this.transform.parent = null;
		collider.enabled = true;
		IsAirborn = false;
		Carried = false;

		GameManager.Instance.BoxDisplaced(countAsStacked ? 1 : 0);

		if(playSound)
			BoxAudioManager.PlayBoxPlaced();
	}

	public void Throw( Vector3 throwerMovement, Vector3 throwDirection ) {
		IsAirborn = true;
		movement = (throwerMovement + (throwDirection * ThrowSpeedModifier * Random.Range(8f, 10f))) * Time.fixedDeltaTime;
		rotation = Random.Range(-90f, 90f) * Time.fixedDeltaTime;
		this.transform.parent = null;
		collider.enabled = true;
		Carried = false;
	}
}
