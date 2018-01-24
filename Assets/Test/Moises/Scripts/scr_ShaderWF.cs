using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ShaderWF : MonoBehaviour {

    public static Shader WF;
    public static bool IsWf;
    public static bool wfswitch;

    public Shader[] normal;
    MeshRenderer ms;

    public static void InitShSy()
    {
        IsWf = false;
        wfswitch = false;
        WF = Resources.Load("Shaders/UCLAGameLabWireframe.shader") as Shader;
        if (WF==null)
        {
            Debug.LogWarning("No se encontro el Shader WF");
        }
    }

    public static void SwitchWF()
    {
        wfswitch = true;
    }

	// Use this for initialization
	void Start () {
        ms = GetComponent<MeshRenderer>();

        if (ms == null)
            return;

        normal = new Shader[ms.materials.Length];
        for (int i=0; i<ms.materials.Length; i++)
        {
            normal[i] = ms.materials[i].shader;
        }

    }
	
	// Update is called once per frame
	void Update () {
		if (wfswitch)
        {
            wfswitch = false;
            IsWf = !IsWf;
            if (IsWf)
            {
                for (int i = 0; i < ms.materials.Length; i++)
                {
                    ms.materials[i].shader = WF;
                }
            } else
            {
                for (int i = 0; i < ms.materials.Length; i++)
                {
                    ms.materials[i].shader = normal[i];
                }
            }
        }
	}
}
