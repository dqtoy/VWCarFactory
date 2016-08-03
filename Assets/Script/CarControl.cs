using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
			ChangeViewDistance();
		}

		//if (!GameManager.instance.inGoto) {
		Camera.main.transform.LookAt (camTarget.position );
		//Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation(camTarget.position - Camera.main.transform.position),Time.deltaTime * 10.0f);
		//}
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
		UIManager.instance.ChangeScrollBar (false);
		//StopCoroutine ("ChangeToAutoRotation");
	}

	void OnMouseDrag()
	{
		if (!UIManager.instance.isBarDraging) {
			mouseDelta = mouseLastPosition - new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			//GameManager.instance.car.transform.Rotate (Vector3.up * Time.deltaTime * mouseDelta.x * rotateSpeed);
			//carRoot.transform.Rotate (Vector3.left * Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
			//Camera.main.transform.RotateAround(carRoot.transform.position,Vector3.left,Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
			float tmpY = Camera.main.transform.localPosition.y;
			if ((tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) < camUpDownBound.y && (tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) > camUpDownBound.x) {
				Camera.main.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y,Space.World);
			}
			Camera.main.transform.RotateAround(carRoot.transform.position,Vector3.up,Time.deltaTime * (-mouseDelta.x) * rotateSpeed);
			mouseLastPosition = Input.mousePosition;
		}
			
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
