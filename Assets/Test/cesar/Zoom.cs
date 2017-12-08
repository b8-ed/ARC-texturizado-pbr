using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

    public GameObject obj;
    Camera cam;
    public int radio=10;
    //inicial 60 depth
    int sentido = 0;

    float velocidad = 0.2f;

	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        cam.transform.LookAt(obj.transform);
        //
        if(Input.GetKey(KeyCode.LeftArrow))
        {

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

        }
    }

    /// <summary>
    /// rotar cuando le presionas -1 izquierda   +1 derecha  0 nada
    /// </summary>
    /// <param name="sentido"></param>
    void FnOrbitar(int sentido)
    {
        if(sentido ==-1)
        {
           //rotar izquierda
        }
        else if(sentido==1)
        {
            //rotar derecha
        }
    }
    void FnOrbitar()
    {

    }
}
