using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class CarControl : MonoBehaviour {

	public float rotateSpeed;
	public Color[] colorBody; 
	public Color[] colorBox; 
	public Transform carRoot;
	public GameObject[] bodyObj;
	public GameObject[] boxObj;
	public GameObject allPartObj;
	public List<Vector3> AnimationInitialPositionZ;
	public GameObject allEndPosition;
	public float moveXDelta;
	public float animationTime;
	public static CarControl instance;
	bool isBreak;
	bool inAutoRotation;
	Vector2 mouseDelta;
	Vector2 mouseLastPosition;
	float distance;
	public float minimumDistance;
	public float maximumDistance;
	public float pinchSpeed;
	float lastDist;
	float curDist;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		inAutoRotation = true;
		mouseLastPosition = Input.mousePosition;
		//ChangeColor (0);
	}
	
	// Update is called once per frame
	void Update () {
//		if (inAutoRotation) {
//			transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
//		}
		ChangeViewDistance();
	}

	public void ChangeViewDistance()
	{
		if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
		{
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			curDist = Vector2.Distance(touch1.position, touch2.position);
			if(curDist > lastDist)
			{
				distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/10;
			}
			else
			{
				distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/10;
			}
			lastDist = curDist;
			Camera.main.transform.position = Camera.main.transform.position + new Vector3 (0, 0, distance/700);
		}
		if(distance <= minimumDistance)
		{
			distance = minimumDistance;
		}
		if(distance >= maximumDistance)
		{
			distance = maximumDistance;
		}
	}

	void OnMouseDown()
	{
		inAutoRotation = false;
		mouseLastPosition = Input.mousePosition;
		StopCoroutine ("ChangeToAutoRotation");
	}

	void OnMouseDrag()
	{
		mouseDelta = mouseLastPosition - new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		GameManager.instance.car.transform.Rotate (Vector3.up * Time.deltaTime * mouseDelta.x * rotateSpeed);
		carRoot.transform.Rotate (Vector3.left * Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
		mouseLastPosition = Input.mousePosition;
	}

	void OnMouseUp()
	{
		StartCoroutine("ChangeToAutoRotation");
		mouseLastPosition = Input.mousePosition;
	}

	IEnumerator ChangeToAutoRotation()
	{
		
		yield return new WaitForSeconds (5.0f);
		inAutoRotation = true;
	}
}
