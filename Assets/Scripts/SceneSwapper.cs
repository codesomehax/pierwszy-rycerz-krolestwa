using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public SceneAsset GotoScene;

    private bool _canEnter;

    private void Awake()
    {
        _canEnter = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && _canEnter)
        {
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
}
