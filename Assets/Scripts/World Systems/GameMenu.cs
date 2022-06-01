using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using SaveIsEasy;

public class GameMenu : MonoBehaviour
{
    public static bool StartFirstConversation = false;

    private void Awake()
    {
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);

        Regex re = new Regex(@".game$");

        if (fileNames.All<string>(fileName => !re.IsMatch(fileName)))
        {
            GameObject.Find("Load Game").SetActive(false);
        }

        if (FindObjectOfType<Player>() == null)
        {
            GameObject.Find("Save Game").SetActive(false);
        }
    }

    public void NewGame()
    {
        LockCursor();
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);

        Regex re = new Regex(@".game$");

        foreach(string fileName in fileNames)
        {
            if (re.IsMatch(fileName))
            {
                File.Delete(fileName);
            }
        }

        PauseController.gameIsPaused = false;

        Time.timeScale = 1f;

        StartFirstConversation = true;

        SceneManager.LoadScene("MAIN_SCENE", LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        LockCursor();
        Time.timeScale = 1f;
        PauseController.gameIsPaused = false;

        if (!SceneManager.GetSceneByName("MAIN_SCENE").isLoaded)
        {
            SceneSwapper.DataLoadRequest = true;

            SceneManager.LoadScene("MAIN_SCENE", LoadSceneMode.Single);
        }
        else
        {
            AsyncOperation sceneUnloaded = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            StartCoroutine(SceneUnloadedForLoading(sceneUnloaded));
        }

    }

    public void SaveGame()
    {
        LockCursor();
        SaveIsEasyAPI.SaveAll(SceneManager.GetActiveScene().name + ".game", SceneManager.GetActiveScene());
        PauseController.gameIsPaused = false;
        FindObjectOfType<PauseController>().PauseGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator SceneUnloadedForLoading(AsyncOperation sceneUnloaded)
    {
        while (!sceneUnloaded.isDone)
        {
            yield return null;
        }
        SceneSwapper.DataLoadRequest = true;

        SceneManager.LoadScene("MAIN_SCENE", LoadSceneMode.Single);
    }
}
