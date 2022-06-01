using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartTransforms : List<(string from, string to, Vector3 position)>
{
    public static string[] SceneNames = {"Main Menu", "Woods", "GoodCastle2.0", "Evil_Camp"};

    public SceneStartTransforms()
    {
        Add(("Woods", "GoodCastle2.0", new Vector3(-219.6521f, 308f, 311.82666f)));
        Add(("GoodCastle2.0", "Woods", new Vector3(-231.54129f ,300f ,515.950806f)));
        Add(("Woods", "Evil_Camp", new Vector3(104.738838f, 310f, 495.661804f)));
        Add(("Evil_Camp", "Woods", new Vector3(-85.7630005f, 300f, 658.416504f)));
    }

    public Vector3 FindByScenes(string from, string to)
    {
        (string, string, Vector3) a = this.Find(tuple => tuple.Item1 == from && tuple.Item2 == to);


        return a.Item3;
    }
}
