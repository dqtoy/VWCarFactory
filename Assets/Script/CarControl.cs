﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using com.ootii.Messages;

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
	public Transform camTarget;
	public Vector2 camUpDownBound;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		MessageDispatcher.AddListener ("OnDragStart", OnDown,true);
		MessageDispatcher.AddListener ("OnDraging", OnDrag,true);
		MessageDispatcher.AddListener ("OnDragFinish", OnUp,true);
		inAutoRotation = true;
		camTarget = carRoot;
		mouseLastPosition = Input.mousePosition;

		//ChangeColor (0);
	}
	
	// Update is called once per frame
	void Update () {
//		if (inAutoRotation) {
//			transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
//		}
		if (!UIManager.instance.isBarDraging) {
			if (!GameManager.instance.inCameraPosition) {
				ChangeViewDistance();
			}
		}

		//if (!GameManager.instance.inGoto) {
		if (!GameManager.instance.inCameraPosition) {
			Camera.main.transform.LookAt (camTarget.position);
		}
		//Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation(camTarget.position - Camera.main.transform.position),Time.deltaTime * 10.0f);
		//}
	}

	public void ChangeViewDistance()
	{
		
		float dis = Vector3.Distance (carRoot.transform.position, Camera.main.transform.position);
		//Debug.Log ("distance " + dis);
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

			if (dis > 1.4f && dis < 2.5f) {
				//Camera.main.transform.localPosition = Camera.main.transform.localPosition + new Vector3 (0, 0, distance/700);
				Camera.main.transform.Translate(Vector3.forward * Time.deltaTime * distance/10);
			}
		}
		if(distance <= minimumDistance)
		{
			distance = minimumDistance;
		}
		if(distance >= maximumDistance)
		{
			distance = maximumDistance;
		}
		if (dis >= 2.5f) {
			Camera.main.transform.Translate(Vector3.forward * Time.deltaTime * 0.1f);
		} else if(dis < 1.4f) {
			Camera.main.transform.Translate(Vector3.forward * Time.deltaTime * (-0.1f));
		}
	}

	public void OnDown(IMessage rMessage)
	{
		inAutoRotation = false;
		mouseLastPosition = Input.mousePosition;
		UIManager.instance.ChangeScrollBar (false);
		if (UIManager.instance.isPaintBarOut) {
			UIManager.instance.PaintBarAnimation (false);
		}
		//StopCoroutine ("ChangeToAutoRotation");
	}

	public void OnDrag(IMessage rMessage)
	{
		if (!UIManager.instance.isBarDraging) {
			mouseDelta = mouseLastPosition - new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			//GameManager.instance.car.transform.Rotate (Vector3.up * Time.deltaTime * mouseDelta.x * rotateSpeed);
			//carRoot.transform.Rotate (Vector3.left * Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
			//Camera.main.transform.RotateAround(carRoot.transform.position,Vector3.left,Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
			float tmpY = Camera.main.transform.localPosition.y;
			if ((tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) < camUpDownBound.y && (tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) > camUpDownBound.x) {
				Camera.main.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y * 0.2f,Space.World);
			}
			Camera.main.transform.RotateAround(carRoot.transform.position,Vector3.up,Time.deltaTime * (-mouseDelta.x) * rotateSpeed);
			mouseLastPosition = Input.mousePosition;
		}
	}

	public void OnUp(IMessage rMessage)
	{
		//StartCoroutine("ChangeToAutoRotation");
		mouseLastPosition = Input.mousePosition;
	}

	IEnumerator ChangeToAutoRotation()
	{
		yield return new WaitForSeconds (5.0f);
		inAutoRotation = true;
	}
}
