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

        PlayerPrefs.SetInt("Last scene", -1);

        SceneManager.LoadScene("Woods");
    }

    public void LoadGame()
    {
        LockCursor();
        Time.timeScale = 1f;
        PauseController.gameIsPaused = false;

        AsyncOperation sceneUnloaded = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(SceneUnloadedForLoading(sceneUnloaded));
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
        SaveIsEasyAPI.SaveAll(SceneManager.GetActiveScene().name + ".game", SceneManager.GetActiveScene());
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
        SceneManager.LoadScene(PlayerPrefs.GetInt("Last scene"), LoadSceneMode.Single);
    }
}
