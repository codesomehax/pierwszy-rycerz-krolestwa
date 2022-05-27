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
            GameObject.Find("Continue").SetActive(false);
        }
    }

    public void NewGame()
    {
        string[] fileNames = Directory.GetFiles(Application.persistentDataPath);

        Regex re = new Regex(@".game$");

        foreach(string fileName in fileNames)
        {
            if (re.IsMatch(fileName))
            {
                File.Delete(fileName);
            }
        }

        SceneManager.LoadScene("Woods");
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Last scene", 0));
    }

    public void SaveGame()
    {
        SaveIsEasyAPI.SaveAll(SceneManager.GetActiveScene().name + ".game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
