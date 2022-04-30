using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{

    public Slider progressBar;
    public static string nextScene;
    public Text loadtext;
    public static void LoadScene(string nxScene)
    {
        nextScene = nxScene;
       // GameManager.instance.SavePlayerDataJason();
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        yield return null;
         
        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;
            if(progressBar.value < 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f,Time.deltaTime);
            }
            else if (progressBar.value >= 0.9f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }
            if (progressBar.value >= 1f)
            {
                loadtext.text = "pressSpaceBar";
            }

            if(Input.GetKeyDown(KeyCode.Space) && progressBar.value >=1.0f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

        }
    }

}
