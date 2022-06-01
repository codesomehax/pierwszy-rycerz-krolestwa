using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material GoodCastleSkybox;
    public Material EvilCampSkybox;
    public Material WoodsSkybox;

    private Dictionary<string, Material> _materials;

    private void Awake()
    {
        _materials = new Dictionary<string, Material>();
        _materials.Add("GoodCastle2.0", GoodCastleSkybox);
        _materials.Add("Evil_Camp", EvilCampSkybox);
        _materials.Add("Woods", WoodsSkybox);
    }

    public void ChangeSkybox(string sceneName)
    {
        RenderSettings.skybox = _materials[sceneName];
    }
}
