using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider loadingPercent;
    [SerializeField] string sceneToLoad;

    private AsyncOperation loadSceneOp;

    private void Start()
    {
        loadingPercent.value = 0f;
        loadSceneOp = SceneManager.LoadSceneAsync(sceneToLoad);       
    }

    private void Update()
    {
        if(loadSceneOp.progress < 0.9f)
        {
            loadingPercent.value = loadSceneOp.progress;
        }
        else
        {
            loadingPercent.value = 1f;
        }
    }
}
