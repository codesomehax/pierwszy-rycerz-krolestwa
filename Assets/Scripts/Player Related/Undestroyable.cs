using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Undestroyable : MonoBehaviour
{
    private string _objectID;

    private void Awake()
    {
        _objectID = name + transform.position.ToString() + transform.rotation.ToString();
    }

    private void Start()
    {
        foreach (Undestroyable obj in FindObjectsOfType<Undestroyable>())
        {
            if (obj != this)
            {
                if (obj._objectID == _objectID)
                {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
