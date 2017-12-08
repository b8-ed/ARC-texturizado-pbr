using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_MVC : MonoBehaviour {

    float speed = 50f; //how fast the object should rotate

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.z<30) // forward
        {
            transform.Translate(new Vector3(0f, 0f, 0.5f),Space.World);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.z > -3) // back
        {
            transform.Translate(new Vector3(0f, 0f, -0.5f), Space.World);
        }
    }
}
