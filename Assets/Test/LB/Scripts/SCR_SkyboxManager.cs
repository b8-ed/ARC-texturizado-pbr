//Code by Luis Bazan
//Github user: luisquid11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SkyboxManager : MonoBehaviour {

    public Material MAT_Skybox;
    public Material MAT_Gray;

    private bool isSkyboxOn = true;

	void Start () 
	{
        MAT_Skybox = RenderSettings.skybox;
	}
	
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSkybox();
        }
	}

    public void ChangeSkybox()
    {
        isSkyboxOn = !isSkyboxOn;

        if (isSkyboxOn)
            RenderSettings.skybox = MAT_Skybox;
        else
            RenderSettings.skybox = MAT_Gray;
        DynamicGI.UpdateEnvironment();
    }
}
