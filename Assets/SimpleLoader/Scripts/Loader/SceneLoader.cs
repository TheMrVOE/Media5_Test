using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public int SceneID;
    public LoadingProgressCircle ProgressCircle;
    private AsyncOperation async;
    private float _progress;

	void Start () {
        StartCoroutine(_LoadScene());
	}

    private IEnumerator _LoadScene()
    {
        ProgressCircle.SetProgress(0);
        yield return new WaitForSeconds(3);

        async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneID);
        async.allowSceneActivation = false; 
        while (!async.isDone)
        {
            // print("asyn: "+async.progress+"     "+ "_progress: "+_progress);
            _progress = async.progress + ((async.progress / 10.0f) / async.progress);
          
            if (float.IsNaN(_progress)) _progress = 0;

            if (_progress > 0.95f) break;

            ProgressCircle.SetProgress(_progress);       
            yield return null;
        }
        
        //If loading is done
       ProgressCircle.SetProgress(1.0f);

        yield return async;
    }

    void Update()
    {
        if (_progress > 0.95f)
        {   
            if (Input.anyKeyDown)
                async.allowSceneActivation = true;

            foreach (var item in Input.touches)
            {
                if(item.phase == TouchPhase.Began)
                {
                    async.allowSceneActivation = true;
                    break;
                }
            }
        }
    }
}
