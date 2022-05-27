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
    private bool _canEnter;

    

    private void Awake()
    {
        _canEnter = false;
        _player = FindObjectOfType<Player>();
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
            SaveIsEasyAPI.LoadAll(scene.name + ".game");
        }

        PlayerPrefs.SetInt("Last scene", SceneManager.GetActiveScene().buildIndex);
    }
}
