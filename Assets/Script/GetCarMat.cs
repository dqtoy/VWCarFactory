using UnityEngine;
using System.Collections;

public class GetCarMat : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Material body = GameObject.FindGameObjectWithTag ("CarBody").GetComponent<Renderer> ().sharedMaterial;
//		Debug.Log ("bodymat " + body);
//		if (body != null) {
//			GetComponent<Renderer> ().sharedMaterial = body;
//		}
	}

	void OnEnable()
	{
		StartCoroutine (RegetMat ());
		Material mat=CarStudio.GetCurrentBodyMat();
		if (mat != null)
			GetComponent<Renderer> ().material = mat;
	}

	IEnumerator RegetMat()
	{
		yield return new WaitForSeconds (0.00001f);
		GetComponent<Renderer>().material = GameObject.FindGameObjectWithTag("CarBody").GetComponent<Renderer>().sharedMaterial;
	}
}
