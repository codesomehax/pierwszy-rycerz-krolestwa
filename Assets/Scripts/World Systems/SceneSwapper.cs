using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneSwapper : MonoBehaviour
{
    public static bool DataLoadRequest = true;

    public string FromSceneName;
    public string GotoSceneName;

    
    private Player _player;
    private bool _canEnter;

    

    private void Awake()
    {
        _canEnter = false;
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canEnter)
        {
            SceneStartTransforms transforms = new SceneStartTransforms();

            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = transforms.FindByScenes(FromSceneName, GotoSceneName);
            _player.GetComponent<CharacterController>().enabled = true;

            _canEnter = false;

            FindObjectOfType<SkyboxChanger>().ChangeSkybox(GotoSceneName);

            IRespawnable[] respawnables = FindObjectsOfType<MonoBehaviour>().OfType<IRespawnable>().ToArray<IRespawnable>();

            foreach (IRespawnable respawnable in respawnables)
            {
                respawnable.Respawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
            _canEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _canEnter = false;
    }
}
