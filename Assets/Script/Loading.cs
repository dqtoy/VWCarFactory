using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
    public UnityEngine.UI.Image loadingImage;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        StartCoroutine(LoadingScene("Main"));

    }

    IEnumerator LoadingScene(string __name)
    {
        yield return new WaitUntil(() =>
        {
            loadingImage.fillAmount += Time.deltaTime;
            return loadingImage.fillAmount >= 0.5f;
        });
        var _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(__name);
        yield return new WaitUntil(() =>
        {
            loadingImage.fillAmount = _async.progress * 0.5f + .5f;

            return _async.isDone;
        });
    }
}
