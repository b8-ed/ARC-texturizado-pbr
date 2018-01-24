//Code by Luis Bazan
//Github user: luisquid11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SkyboxManager : MonoBehaviour {

    public Material [] MAT_Skybox;
    public Material MAT_Gray;

    private int localIndex = 0;
    private bool isSkyboxOn = true;

	void Start () 
	{
        MAT_Skybox[0] = RenderSettings.skybox;
	}
	
    public void SetSkybox(int index)
    {
        localIndex = index;
        RenderSettings.skybox = MAT_Skybox[index];
    }

    public void RotateSkybox(float value)
    {
        RenderSettings.skybox.SetFloat("_Rotation", value * 360);
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
            RenderSettings.skybox = MAT_Skybox[localIndex];
        else
            RenderSettings.skybox = MAT_Gray;
        DynamicGI.UpdateEnvironment();
    }
}
