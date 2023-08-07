using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    
    public void LoadNextScene()
    {
       StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }

   public void DoExitGame()
    {
        Application.Quit();
    }
   

   IEnumerator LoadAsynchronously(int sceneIndex)
   {
       AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
       while (!operation.isDone)
       {
           yield return null;
       }
   }
}
