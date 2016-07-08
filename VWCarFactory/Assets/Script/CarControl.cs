using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class CarControl : MonoBehaviour {

	public float rotateSpeed;
	public Color[] colorBody; 
	public Color[] colorBox; 
	public Transform carRoot;
	public GameObject car;
	public GameObject[] bodyObj;
	public GameObject[] boxObj;
	public GameObject allPartObj;
	public MeshFilter[] AnimationEndPositionZ;
	public List<Vector3> AnimationInitialPositionZ;
	public GameObject allEndPosition;
	public MeshFilter[] allBodyParts;
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
		ChangeColor (0);
		allBodyParts = allPartObj.GetComponentsInChildren<MeshFilter> ();
		AnimationEndPositionZ = allEndPosition.GetComponentsInChildren<MeshFilter> ();
		for (int i = 0; i < allBodyParts.Length; i++) {
			AnimationInitialPositionZ.Add (allBodyParts [i].transform.localPosition);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (inAutoRotation) {
//			transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
//		}
//
//		if (Input.GetKeyDown(KeyCode.Space)) {
//			ChangeBodyPartsButton();
//		}
		ChangeViewDistance();
	}

	public void ChangeColor(int id)
	{
		foreach (GameObject item in bodyObj) {
			item.GetComponent<Renderer> ().material.color = colorBody [id];
		}

		foreach (GameObject item2 in boxObj) {
			item2.GetComponent<Renderer> ().material.color = colorBox [id];
		}
	}

	public void ChangeBodyParts(Vector3 offsetVal,int i)
	{
		allBodyParts [i].transform.DOLocalMove (offsetVal, animationTime).SetEase (Ease.InOutExpo);
	}

	public void ChangeBodyPartsButton()
	{
		for (int i = 0; i < allBodyParts.Length; i++) {
			if (isBreak) {
				ChangeBodyParts (AnimationInitialPositionZ[i],i);
			} else {
				ChangeBodyParts (AnimationEndPositionZ[i].transform.localPosition,i);
			}
		}
		isBreak = !isBreak;
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
		car.transform.Rotate (Vector3.up * Time.deltaTime * mouseDelta.x * rotateSpeed);
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
