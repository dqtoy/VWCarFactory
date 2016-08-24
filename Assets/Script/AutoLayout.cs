using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoLayout : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        RectTransform rect = GetComponent<RectTransform>();
        grid.cellSize = new Vector2(150, rect.rect.height / 6);

    }
	
	// Update is called once per frame
	void Update () {

    }


}
