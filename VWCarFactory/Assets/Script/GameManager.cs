using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	static public GameManager instance;
	public Transform camOutside;
	public Transform camInside;
	public Transform car;
	bool cameraIsInside;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CameraChange(bool inside)
	{
		if (cameraIsInside != inside) {
			if (inside) {
				CameraGoto (camInside);
				cameraIsInside = true;
			} else {
				CameraGoto (camOutside);
				cameraIsInside = false;
			}
		}
	}

	public void CameraGoto(Transform pos)
	{
		transform.DOMove (pos.position, 1.0f).SetEase (Ease.OutExpo);
		transform.DORotate (pos.rotation.eulerAngles, 1.0f).SetEase (Ease.OutExpo);
		if (cameraIsInside) {
			car.transform.DORotate (new Vector3 (0, -90, 0), 1.0f).SetEase (Ease.OutExpo);
		} else {
			car.transform.DORotate (new Vector3 (0, 0, 0), 1.0f).SetEase (Ease.OutExpo);
		}

	}
}
