using UnityEngine;
using System.Collections;

public class ProjectorTrack : MonoBehaviour {
	
	public float maxStretch = 3f;
	public LineRenderer cataplutLineFront,cataplutLineBack;

	private SpringJoint2D spring;
    private Rigidbody2D rigidbodyMF;
	private Vector2 catapult;
	private Ray rayToMouse,leftCataplutToProjectile,rightCataplutToProjectile;
	private float maxStretchSqr,circleRadius;
	private bool clickedOn;
	private Vector2 prevVelocity;
	void Awake()
	{
        spring = GetComponent<SpringJoint2D>();
        rigidbodyMF = GetComponent<Rigidbody2D>();
		catapult = spring.connectedAnchor*2+(Vector2)spring.connectedBody.position;
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
		rigidbodyMF.isKinematic = false;
		clickedOn = false;
	}
	// Use this for initialization
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (catapult, Vector3.zero);
		leftCataplutToProjectile = new Ray (cataplutLineFront.transform.position, Vector3.zero);
		rightCataplutToProjectile = new Ray (cataplutLineBack.transform.position, Vector3.zero);
		CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
		circleRadius = circle.radius;
	}
	void LineRendererUpdate(LineRenderer line ,Ray ray)
	{
        Vector3 catapultToProjectile = transform.position - (Vector3)catapult;
        catapultToProjectile *= 1f + circleRadius / catapultToProjectile.magnitude;
        //ray.direction = catapultToProjectile + line.transform.position - (Vector3)catapult;
		//Vector3 holdPoint = ray.GetPoint (catapultToProjectile.magnitude);
        catapultToProjectile.z = 0;
        line.SetPosition(1, catapultToProjectile + (Vector3)catapult);
	}
	void Dragging()
	{
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = (Vector2)mouseWorldPoint - catapult;
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
			if(!rigidbodyMF.isKinematic && prevVelocity.sqrMagnitude>rigidbodyMF.velocity.sqrMagnitude)
			{
				Destroy(spring);
				rigidbodyMF.velocity=prevVelocity;
			}
			if(!clickedOn)
			{
				prevVelocity=rigidbodyMF.velocity;
			}
			LineRendererUpdate(cataplutLineFront,leftCataplutToProjectile);
			LineRendererUpdate(cataplutLineBack,rightCataplutToProjectile);
		}
		else
		{
			cataplutLineBack.enabled=false;
			cataplutLineFront.enabled=false;
		}
	}
}
