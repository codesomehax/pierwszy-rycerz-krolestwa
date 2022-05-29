using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartTransforms : List<(Scene from, Scene to, Vector3 position)>
{
    public SceneStartTransforms()
    {
        Add((SceneManager.GetSceneByName("Woods"), SceneManager.GetSceneByName("GoodCastle2.0"), new Vector3(4.29166365f, 0.0603375435f, 45.0141487f)));
        Add((SceneManager.GetSceneByName("GoodCastle2.0"), SceneManager.GetSceneByName("Woods"), new Vector3(20f, 301f, 20f)));
        Add((SceneManager.GetSceneByName("Woods"), SceneManager.GetSceneByName("Evil_Camp"), new Vector3(106.5f, 0.0500000007f, 153.869995f)));
        Add((SceneManager.GetSceneByName("Evil_Camp"), SceneManager.GetSceneByName("Woods"), new Vector3(176.169113f, 300f, 178.277679f)));
    }

    public Vector3 FindByScenes(Scene from, Scene to)
    {
        return Find(tuple => tuple.Item1 == from && tuple.Item2 == to).Item3;
    }
}
