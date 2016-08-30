using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform);
	}
}
