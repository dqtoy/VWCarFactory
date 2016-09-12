using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using com.ootii.Messages;

public class CarControl : MonoBehaviour {
	static public Camera mainCamera;
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
	public Vector2 cameraLimitBoundsX;
	public Vector2 cameraLimitBoundsY;
	public Vector2 cameraScaleBound;
	public Transform camTarget;
	public Vector2 camUpDownBound;
	public float camNowRotateY;
	public float camNowPosX;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		//MessageDispatcher.AddListener ("OnDragStart", OnDown,true);
		//MessageDispatcher.AddListener ("OnDraging", OnDrag,true);
		//MessageDispatcher.AddListener ("OnDragFinish", OnUp,true);
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
		//mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (!UIManager.instance.isBarDraging) {
			ChangeViewDistance (!GameManager.instance.inCameraPosition);
		}

		//if (!GameManager.instance.inGoto) {
		if (!GameManager.instance.inCameraPosition) {
			mainCamera.transform.LookAt (camTarget.position);
		} else {
			mainCamera.transform.LookAt (GameManager.instance.allCamPosition [GameManager.instance.nowCamPositionID].TransformDirection (Vector3.forward + new Vector3(0,0,1)));
				//new Vector3(GameManager.instance.allCamPosition[GameManager.instance.nowCamPositionID].localPosition.x,
												//	  GameManager.instance.allCamPosition[GameManager.instance.nowCamPositionID].localPosition.y,
												//	  GameManager.instance.allCamPosition[GameManager.instance.nowCamPositionID].localPosition.z + 1.0f));
			if (mainCamera.transform.localPosition.y >= cameraLimitBoundsY.y) {
				mainCamera.transform.Translate(Vector3.up * Time.deltaTime * (-0.1f));
			} else if(mainCamera.transform.localPosition.y <= cameraLimitBoundsY.x) {
				mainCamera.transform.Translate(Vector3.up * Time.deltaTime * 0.1f);
			}
//			Debug.Log (camNowPosX);
//			camNowPosX = camNowPosX + (Vector3.right * Time.deltaTime * mouseDelta.x * 0.1f).x;
//			if (camNowPosX >= cameraLimitBoundsX.y) {
//				camNowPosX = camNowPosX + (Vector3.right * Time.deltaTime * (-0.1f)).x;
//				mainCamera.transform.Translate(Vector3.right * Time.deltaTime * (-0.1f));
//			} else if(camNowPosX <= cameraLimitBoundsX.x) {
//				camNowPosX = camNowPosX + (Vector3.right * Time.deltaTime * 0.1f).x;
//				mainCamera.transform.Translate(Vector3.right * Time.deltaTime * 0.1f);
//			}
//			if (camNowRotateY > (cameraLimitBoundsX.y + GameManager.instance.cameraRotationY)) {
//				mainCamera.transform.Rotate(Vector3.up * Time.deltaTime * (-10.0f),Space.World);
//			}
//			else if(camNowRotateY < (cameraLimitBoundsX.x + GameManager.instance.cameraRotationY)) {
//				mainCamera.transform.Rotate(Vector3.up * Time.deltaTime * 10.0f,Space.World);
//			}
		}
		//mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.LookRotation(camTarget.position - mainCamera.transform.position),Time.deltaTime * 10.0f);
		//}
	}

	public void ChangeViewDistance(bool isChangeView)
	{
		
		float dis = Vector3.Distance (carRoot.transform.position, mainCamera.transform.position);
        
        //Debug.Log ("distance " + dis);
        if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
		{
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			curDist = Vector2.Distance(touch1.position, touch2.position);
            if (curDist > lastDist)
            {
                distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
            }
            else
            {
                distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
            }
            lastDist = curDist;

            if (dis > cameraScaleBound.x && dis < cameraScaleBound.y)
            {
                //mainCamera.transform.localPosition = mainCamera.transform.localPosition + new Vector3 (0, 0, distance/700);
                mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * distance / 10);
            }

            if (distance <= minimumDistance)
            {
                distance = minimumDistance;
            }
            if (distance >= maximumDistance)
            {
                distance = maximumDistance;
            }
            if (isChangeView) {
				if (dis >= cameraScaleBound.y) {
					mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * 0.1f);
				} else if(dis < cameraScaleBound.x) {
					mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * (-0.1f));
				}
			} else {
                float disInPos = Vector3.Distance(camTarget.position, mainCamera.transform.position);
                if (disInPos >= (cameraScaleBound.y - 0.8f))
                {
                    mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * 0.1f);
                }
                else if (disInPos < (cameraScaleBound.x + 0.8f))
                {
                    mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * (-0.1f));
                }
                //Debug.Log ((curDist - lastDist));
                //				if ((curDist - lastDist) <= -40) {
                //					GameManager.instance.CameraGoBack ();
                //				}
                //				lastDist = curDist;
            }

		}
	}

	public void OnDown()//IMessage rMessage)
	{
		inAutoRotation = false;
		mouseLastPosition = Input.mousePosition;
		//UIManager.instance.ChangeScrollBar (false);
		if (UIManager.instance.isPaintBarOut) {
			UIManager.instance.PaintBarAnimation (false);
		}

		//StopCoroutine ("ChangeToAutoRotation");
	}

	public void OnDrag()//IMessage rMessage)
	{
		if (!UIManager.instance.isBarDraging) {
			if (GameManager.instance.inCameraPosition) {
				LimitedCameraControl ();
			} else {
				NormalCameraControl ();
			}
		}
	}

	public void OnUp()//IMessage rMessage)
	{
		//StartCoroutine("ChangeToAutoRotation");
		mouseLastPosition = Input.mousePosition;
		mouseDelta = Vector2.zero;
	}

	IEnumerator ChangeToAutoRotation()
	{
		yield return new WaitForSeconds (5.0f);
		inAutoRotation = true;
	}

	void NormalCameraControl()
	{
		mouseDelta = mouseLastPosition - new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		//GameManager.instance.car.transform.Rotate (Vector3.up * Time.deltaTime * mouseDelta.x * rotateSpeed);
		//carRoot.transform.Rotate (Vector3.left * Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
		//mainCamera.transform.RotateAround(carRoot.transform.position,Vector3.left,Time.deltaTime * (-mouseDelta.y) * rotateSpeed);
		float tmpY = mainCamera.transform.localPosition.y;
		if ((tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) < camUpDownBound.y && (tmpY + (Vector3.up * Time.deltaTime * mouseDelta.y).y) > camUpDownBound.x) {
			mainCamera.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y * 0.2f,Space.World);
		}
		mainCamera.transform.RotateAround(carRoot.transform.position,Vector3.up,Time.deltaTime * (-mouseDelta.x) * rotateSpeed);
		mouseLastPosition = Input.mousePosition;
	}

	void LimitedCameraControl()
	{
		mouseDelta = mouseLastPosition - new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		if (mainCamera.transform.localPosition.y < cameraLimitBoundsY.y && mainCamera.transform.localPosition.y > cameraLimitBoundsY.x) {
			//mainCamera.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y * 0.2f,Space.World);
			mainCamera.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y * 0.1f);
		}

		if ((camNowPosX + (Vector3.right * Time.deltaTime * mouseDelta.x * 0.1f).x) <= cameraLimitBoundsX.y && (camNowPosX + (Vector3.right * Time.deltaTime * mouseDelta.x * 0.1f).x) >= cameraLimitBoundsX.x) {
			//mainCamera.transform.Translate(Vector3.up * Time.deltaTime * mouseDelta.y * 0.2f,Space.World);
			camNowPosX = camNowPosX + (Vector3.right * Time.deltaTime * mouseDelta.x * 0.1f).x;
			mainCamera.transform.Translate (Vector3.right * Time.deltaTime * mouseDelta.x * 0.1f);
		}
//		}else if (camNowPosX > cameraLimitBoundsX.y) {
//			Debug.Log (camNowPosX);
//			camNowPosX = cameraLimitBoundsX.y;
//		}else if (camNowPosX < cameraLimitBoundsX.x) {
//			camNowPosX = cameraLimitBoundsX.x;
//		}
		//Debug.Log (cameraLimitBoundsX.y + GameManager.instance.cameraRotationY);
		//Debug.Log (Time.deltaTime * rotateSpeed * mouseDelta.x);
//		if (mouseDelta.x > 0 && (mainCamera.transform.rotation.eulerAngles.y + Time.deltaTime * rotateSpeed * mouseDelta.x) > 360) {
//			camNowRotateY = mainCamera.transform.rotation.eulerAngles.y + 360;
//		} else if (mouseDelta.x < 0 && (mainCamera.transform.rotation.eulerAngles.y - Time.deltaTime * rotateSpeed * mouseDelta.x) < 0) {
//			camNowRotateY = mainCamera.transform.rotation.eulerAngles.y - 360;
//		} else {
//			camNowRotateY = mainCamera.transform.rotation.eulerAngles.y;
//		}
//		if (mainCamera.transform.rotation.eulerAngles.y < (cameraLimitBoundsX.y + GameManager.instance.cameraRotationY) && mainCamera.transform.rotation.eulerAngles.y > (cameraLimitBoundsX.x + GameManager.instance.cameraRotationY)) 
//		{
//			//mainCamera.transform.RotateAround(carRoot.transform.position,Vector3.up,Time.deltaTime * (-mouseDelta.x) * rotateSpeed);
//			mainCamera.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * mouseDelta.x,Space.World);
//
//		}

		mouseLastPosition = Input.mousePosition;
	}
}
