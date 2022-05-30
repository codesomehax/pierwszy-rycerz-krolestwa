using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using SaveIsEasy;

public class SceneSwapper : MonoBehaviour
{
    public SceneAsset GotoScene;

    private Player _player;
    private QuestManager _questManager;
    private bool _canEnter;

    

    private void Awake()
    {
        _canEnter = false;
        _player = FindObjectOfType<Player>();
        _questManager = FindObjectOfType<QuestManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && _canEnter)
        {
            SaveIsEasyAPI.SaveAll(SceneManager.GetActiveScene().name + ".game");
            SceneManager.LoadScene(GotoScene.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
            _canEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
            _canEnter = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SaveIsEasyAPI.FileExists(scene.name + ".game"))
        {
            Player playertemp = null;
            QuestManager questManagerTemp = null;

            if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("Last scene"))
            {
                playertemp = ObjectCopier.DeepCopy<Player>(_player);
                questManagerTemp = ObjectCopier.DeepCopy<QuestManager>(_questManager);
            }

            SaveIsEasyAPI.LoadAll(scene.name + ".game");

            if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("Last scene"))
            {
                _player = playertemp;
                _questManager = questManagerTemp;
            }

            SaveIsEasyAPI.SaveAll(SceneManager.GetActiveScene().name + ".game", SceneManager.GetActiveScene());
        }
    }
}
