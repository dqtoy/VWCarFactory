using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform);
		transform.localRotation = Quaternion.Euler (0, 0, 45.0f);
	}
}
