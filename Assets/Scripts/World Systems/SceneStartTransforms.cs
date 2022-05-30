using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartTransforms : List<(string from, string to, Vector3 position)>
{
    public static string[] SceneNames = {"Main Menu", "Woods", "GoodCastle2.0", "Evil_Camp"};

    public SceneStartTransforms()
    {
        Add(("Woods", "GoodCastle2.0", new Vector3(4.29166365f, 0.0603375435f, 45.0141487f)));
        Add(("GoodCastle2.0", "Woods", new Vector3(20f, 301f, 20f)));
        Add(("Woods", "Evil_Camp", new Vector3(106.5f, 0.0500000007f, 153.869995f)));
        Add(("Evil_Camp", "Woods", new Vector3(176.169113f, 300f, 178.277679f)));
    }

    public Vector3 FindByScenes(string from, string to)
    {
        (string, string, Vector3) a = this.Find(tuple => tuple.Item1 == from && tuple.Item2 == to);


        return a.Item3;
    }
}
