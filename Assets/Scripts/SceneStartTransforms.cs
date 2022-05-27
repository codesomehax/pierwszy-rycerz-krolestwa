using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartTransforms : MonoBehaviour
{
    public static Dictionary<Scene, Dictionary<Scene, (Vector3, Quaternion)>> StartTransforms;

    public void Awake()
    {
        
    }
}
