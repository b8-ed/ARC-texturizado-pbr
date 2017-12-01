//ruth sofia brown
//git rsofia
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scr_ChageAspect : MonoBehaviour
{
    public GameObject target;
    public Texture normal;


	public void DisplayWireframe()
    {
        
    }

    public void DisplayAlbedo()
    {

    }

    public void DisplayNormal()
    {
        target.GetComponent<Renderer>().material.SetTexture(Shader.PropertyToID("_MainTex"), normal);
    }

    public void DisplayMetallic()
    {

    }

    public void DisplayRoughness()
    {

    }
}
