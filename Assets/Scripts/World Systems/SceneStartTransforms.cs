using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartTransforms : List<(int from, int to, Vector3 position)>
{
    public SceneStartTransforms()
    {
        Add((SceneManager.GetSceneByName("Woods").buildIndex, SceneManager.GetSceneByName("GoodCastle2.0").buildIndex, new Vector3(4.29166365f, 0.0603375435f, 45.0141487f)));
        Add((SceneManager.GetSceneByName("GoodCastle2.0").buildIndex, SceneManager.GetSceneByName("Woods").buildIndex, new Vector3(20f, 301f, 20f)));
        Add((SceneManager.GetSceneByName("Woods").buildIndex, SceneManager.GetSceneByName("Evil_Camp").buildIndex, new Vector3(106.5f, 0.0500000007f, 153.869995f)));
        Add((SceneManager.GetSceneByName("Evil_Camp").buildIndex, SceneManager.GetSceneByName("Woods").buildIndex, new Vector3(176.169113f, 300f, 178.277679f)));
    }

    public Vector3 FindByScenes(int from, int to)
    {
        (int, int, Vector3) a = this.Find(tuple => tuple.Item1 == from && tuple.Item2 == to);


        return a.Item3;
    }
}
