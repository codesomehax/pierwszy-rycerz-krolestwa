using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 10f*Time.deltaTime, Space.Self); //krecenie smiglem
        //transform.Rotate(0f,0f,10f )

    }
}
