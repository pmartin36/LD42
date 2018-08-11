using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

	public Box CarriedBox;
	private float carriedBoxDistance;

	public float MaxMovespeed;
	private Vector3 moveDirection;
	private float targetSpeed;
	private float currentSpeed;

	public bool CanMove = true;

	[SerializeField]
	private LayerMask CollisionLayers;
	[SerializeField]
	private LayerMask BoxHighlingLayers;

	private float cv;
	private CircleCollider2D collider;

	private Box highlightedBox;

	private void Awake() {
		collider = GetComponent<CircleCollider2D>();
	}

	// Use this for initialization
	void Start () {
		GameManager.Instance.Player = this;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate() {
		if( CanMove ) {
			Move();
		}
	}

	public void Move() {
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref cv, 0.1f);
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, collider.radius, moveDirection, currentSpeed, CollisionLayers);

		if(hits.Length > 0) {
			// valid collision
			currentSpeed = targetSpeed = 0;//((hits[0].point - (Vector2)transform.position)*collider.radius).magnitude;
			transform.position = hits[0].point + hits[0].normal * collider.radius;
		}
		else {
			transform.position += moveDirection * currentSpeed;
		}
		
	}

	public void ProcessInputs(InputPackage p) {
		var dir = new Vector2(p.Horizontal, p.Vertical);
		targetSpeed = dir.magnitude * MaxMovespeed * (CarriedBox?.MovespeedModifier ?? 1) * Time.fixedDeltaTime;
		moveDirection = dir.normalized;

		bool canPickup = false;

		var box = Physics2D.RaycastAll(p.MouseLocation, Vector2.one, 0, BoxHighlingLayers)
						.Where( g => g.collider != null && g.collider.gameObject != CarriedBox?.gameObject)
						.Select( g => g.collider.GetComponent<Box>() )
						.FirstOrDefault( g => g.Interactable );

		if (box != null) {
			var mousedBox = box;
			if(mousedBox != null && !mousedBox.IsAirborn) {
				if (highlightedBox != mousedBox) {
					if (highlightedBox != null) highlightedBox.OutlineColor = Color.clear;
					highlightedBox = mousedBox;
				}
				if(CarriedBox != null) {
					canPickup = Vector2.Distance(CarriedBox.transform.position, highlightedBox.transform.position) < 0.3f;
				}
				else {
					canPickup = Vector2.Distance(transform.position, highlightedBox.transform.position) < 1.25f;
				}	
				highlightedBox.OutlineColor = canPickup ? Color.green : Color.red;
			}
		}
		else {
			if (highlightedBox != null) highlightedBox.OutlineColor = Color.clear;
			highlightedBox = null;
		}

		if (CarriedBox == null) {		
			if (p.MouseClick != MouseInput.NONE && canPickup) {
				//pick up box
				CarriedBox = highlightedBox;
				highlightedBox = null;
				carriedBoxDistance = Vector3.Distance(transform.position, CarriedBox.transform.position);
				CarriedBox.PickedUp(this.transform);
			}
		}
		else {
			CarriedBox.transform.localPosition = ((Vector3)p.MouseLocation - transform.position).normalized * carriedBoxDistance;
			if (p.MouseClick == MouseInput.RIGHT) {
				// throw box
				CarriedBox.Throw( moveDirection * currentSpeed, ((Vector3)p.MouseLocation - transform.position).normalized  );
				CarriedBox = null;
			}
			else if(p.MouseClick == MouseInput.LEFT) {
				//place box
				CarriedBox.Place(box);
				CarriedBox = null;
			}		
		}
	}
}
