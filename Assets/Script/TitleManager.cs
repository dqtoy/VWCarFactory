using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	//public MovieTexture mTexture;
	public GameObject page1;
	public GameObject page3;
	public GameObject pageMain;
	public GameObject page1_1;
	public GameObject page1_2;
	public GameObject page3_1;
	public GameObject page3_2;
	public GameObject page3_3;
	//public MovieTexture movie1;
	//public MovieTexture movie2;
	public AudioSource movieSound1;
	public AudioSource movieSound2;
	public AudioSource backgroundMusic;

	// Use this for initialization
	void Start () {
		//mTexture.loop = true;
		//mTexture.Play ();
	}

	public void ButtonDown1()
	{
		page1.SetActive (true);
		pageMain.SetActive (false);
		ButtonDown1_1 ();
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown1_1()
	{
		page1_1.SetActive (true);
		page1_2.SetActive (false);
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown1_2()
	{
		page1_1.SetActive (false);
		page1_2.SetActive (true);
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown2()
	{
		StartCoroutine (LoadScene());
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown3()
	{
		page3.SetActive (true);
		pageMain.SetActive (false);
		ButtonDown3_2 ();
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown3_1()
	{
		page3_1.SetActive (false);
		page3_2.SetActive (true);
		page3_3.SetActive (false);
		//movie1.Play ();
		movieSound1.Play ();
		backgroundMusic.Stop ();
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown3_2()
	{
		page3_1.SetActive (true);
		page3_2.SetActive (false);
		page3_3.SetActive (false);
		//movie1.Stop ();
		movieSound1.Stop ();
		//movie2.Stop ();
		movieSound2.Stop ();
		if (backgroundMusic.isPlaying == false) {
			backgroundMusic.Play ();
		}
		GetComponent<AudioSource> ().Play();
	}

	public void ButtonDown3_3()
	{
		page3_1.SetActive (false);
		page3_2.SetActive (false);
		page3_3.SetActive (true);
		//movie2.Play ();
		movieSound2.Play ();
		backgroundMusic.Stop ();
		GetComponent<AudioSource> ().Play();
	}

	public void BackToMain()
	{
		page1.SetActive (false);
		page3.SetActive (false);
		pageMain.SetActive (true);
		GetComponent<AudioSource> ().Play();
	}

	IEnumerator LoadScene()
	{
		AsyncOperation async = Application.LoadLevelAsync(1);
		yield return async;
	}
}
