using UnityEngine;
using System.Collections;

public class ProjectorTrack : MonoBehaviour {
	
	public float maxStretch = 3f;
	public LineRenderer cataplutLineFront,cataplutLineBack;

	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse,leftCataplutToProjectile;
	private float maxStretchSqr,circleRadius;
	private bool clickedOn;
	private Vector2 prevVelocity;
	void Awake()
	{
		spring = GetComponent<SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
		maxStretchSqr = maxStretch * maxStretch;
	}

	void LineRendererSetup()
	{
		cataplutLineFront.SetPosition (0, cataplutLineFront.transform.position);
		cataplutLineBack.SetPosition (0, cataplutLineBack.transform.position);
		cataplutLineFront.sortingLayerName = "Foreground";
		cataplutLineBack.sortingLayerName = "Foreground";
		cataplutLineBack.sortingOrder = 1;
		cataplutLineFront.sortingOrder = 3;

	}
	void OnMouseDown()
	{
		spring.enabled = false;
		clickedOn = true;

	}
	void OnMouseUp()
	{
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		clickedOn = false;
	}
	// Use this for initialization
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCataplutToProjectile = new Ray (cataplutLineFront.transform.position, Vector3.zero);
		CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
		circleRadius = circle.radius;
	}
	void LineRendererUpdate()
	{
		Vector2 catapultToProjectile = transform.position = cataplutLineFront.transform.position;
		leftCataplutToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCataplutToProjectile.GetPoint (catapultToProjectile.magnitude);
		cataplutLineFront.SetPosition (1, holdPoint);

	}
	void Dragging()
	{
		Vector3 mouseWorldPoint = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if(catapultToMouse.sqrMagnitude > maxStretchSqr)
		{
			rayToMouse.direction=catapultToMouse;
			mouseWorldPoint=rayToMouse.GetPoint(maxStretch);
		}
		mouseWorldPoint.z = 1;
		transform.position = mouseWorldPoint;
	}
	void Unknown()
	{
		spring.enabled = true;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		clickedOn = false;
	}
	// Update is called once per frame
	void Update () {
		if(clickedOn)
		{
			Dragging();
		}
		if(spring!=null)
		{
			/*if(!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude>GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
			{
				Destroy(spring);
				GetComponent<Rigidbody2D>().velocity=prevVelocity;
			}
			if(!clickedOn)
			{
				prevVelocity=GetComponent<Rigidbody2D>().velocity;
			}
			LineRendererUpdate();*/
		}
		else
		{
			//cataplutLineBack.enabled=false;
			//cataplutLineFront.enabled=false;
		}
	}
}
