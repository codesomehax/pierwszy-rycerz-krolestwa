using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }
    public void PauseGame ()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
            GameObject.Find("Player Camera").GetComponent<AudioListener>().enabled = false;
            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else 
        {
            AsyncOperation sceneUnloaded = SceneManager.UnloadSceneAsync("Main Menu");
            StartCoroutine(Unpause(sceneUnloaded));
        }
    }

    private IEnumerator Unpause(AsyncOperation sceneUnloaded)
    {
        while (!sceneUnloaded.isDone)
        {
            yield return null;
        }

        GameObject.Find("Player Camera").GetComponent<AudioListener>().enabled = true;
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}
