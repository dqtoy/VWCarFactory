using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{




	public Transform target;
	//ƒø±Í
	public bool canBeOcclusion ;
	//…„œÒª˙ «∑Òø…“‘±ª’⁄µ≤
	public float rotateSpeed = 180f;
	//◊™∂ØÀŸ∂»
	public float mouseScrollWheelSpeed = 300f;
	//◊™∂ØÀŸ∂»
	public float distance = 0f;
	//÷∏∂®æ‡¿Î
	public float _maxDis = 0f;
	public MoveType inputType = MoveType.Drag1R;
	//“∆∂Ø∑Ω Ω




	private float _distanceLerp = 1;
	private float _mixDis = 0f;


	public enum MoveType
	{
		Drag0L,
		Drag1R,
		Drag2Mid,
		MousePosition,
	}

	void Start ()
	{
		if (target.GetComponent<Collider> ()) {
			_mixDis = 0;// Vector3.Distance (target.GetComponent<Collider> ().bounds.center,
				//target.GetComponent<Collider> ().bounds.max);
			if (_maxDis == 0f)
				_maxDis = 20 * _mixDis;

		} else {
			Debug.LogError ("ƒø±Í√ª”–≈ˆ◊≤∆˜£¨Œﬁ∑®ºÏ≤‚◊Ó–°æ‡¿Î£°£°");
			Debug.Break ();
		}


		if (target) {
			if (distance != 0) {
				_maxDis = distance > _maxDis ? distance : _maxDis;
				Ray ray = new Ray (target.position, target.InverseTransformPoint (transform.position));
				Vector3 v = ray.GetPoint (distance);
				transform.position = v;
			} else
				distance = Vector3.Distance (transform.position, target.position);                           //…Ë÷√æ‡¿Î


			transform.LookAt (target.position);
		}
	}




	void Update ()
	{

		float h = GetXAxis ();                                                                                       //ªÒ»° ‰»Î                              
		float v = GetYAxis ();
		//ChangeDistance ();                                                                                            //∏ƒ±‰æ‡¿Î                              
		//œﬁ÷∆–˝◊™
		float xEulerAngles = transform.rotation.eulerAngles.x;
		if (xEulerAngles <= 90) {
			xEulerAngles += (v * Time.deltaTime * rotateSpeed);                                                      //œﬁ÷∆transform.rotation µƒx÷·
			xEulerAngles = xEulerAngles > 90f ? 90f : xEulerAngles;                                                  //Œ™90-0-360-270                                 
			xEulerAngles = xEulerAngles < 0 ? (360f + xEulerAngles) : xEulerAngles;
		} else {
			xEulerAngles += (v * Time.deltaTime * rotateSpeed);
			xEulerAngles = xEulerAngles < 270f ? 270 : xEulerAngles;
			xEulerAngles = xEulerAngles > 360f ? (xEulerAngles - 360f) : xEulerAngles;
		}



		transform.Rotate (0, h * Time.deltaTime * rotateSpeed, 0, Space.World);
		transform.rotation = Quaternion.Euler (new Vector3 (xEulerAngles, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));


		Ray ray = new Ray (target.position, transform.rotation * new Vector3 (0, 0, -distance));                            //º‡≤‚…„œÒÕ∑”Îƒø±Íº‰ «∑Ò”–’⁄µ≤
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, distance) && !canBeOcclusion)
			transform.position = hit.distance < _mixDis ? transform.position : hit.point;
		else
			transform.position = ray.GetPoint (Mathf.LerpAngle (Vector3.Distance (transform.position, target.position),
				distance, Time.deltaTime * _distanceLerp));
	}


	float GetXAxis ()
	{
		switch (inputType) {
		case MoveType.Drag0L:
			if (Input.GetMouseButton ((int)inputType))
				return Input.GetAxis ("Mouse X");
			break;
		case MoveType.Drag1R:
			if (Input.GetMouseButton ((int)inputType))
				return Input.GetAxis ("Mouse X");
			break;
		case MoveType.Drag2Mid:
			if (Input.GetMouseButton ((int)inputType))
				return Input.GetAxis ("Mouse X");
			break;
		case MoveType.MousePosition:
			return Input.GetAxis ("Mouse X");
			break;
		default:
			break;
		}
		return 0f;
	}


	float GetYAxis ()
	{
		switch (inputType) {
		case MoveType.Drag0L:
			if (Input.GetMouseButton ((int)inputType))
				return -Input.GetAxis ("Mouse Y");
			break;
		case MoveType.Drag1R:
			if (Input.GetMouseButton ((int)inputType))
				return -Input.GetAxis ("Mouse Y");
			break;
		case MoveType.Drag2Mid:
			if (Input.GetMouseButton ((int)inputType))
				return -Input.GetAxis ("Mouse Y");
			break;
		case MoveType.MousePosition:
			return -Input.GetAxis ("Mouse Y");
			break;
		default:
			break;
		}
		return 0f;
	}

	/// <summary>
	/// ª¨¬÷∏ƒ±‰æ‡¿Î
	/// </summary>
//	void ChangeDistance ()
//	{
//		float f = Input.GetAxis ("Mouse ScrollWheel");
//		distance += -f * mouseScrollWheelSpeed * Time.deltaTime;
//		distance = Mathf.Clamp (distance, _mixDis, _maxDis);
//	

}
