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
    ProceduralMaterial substance;
    Material baseMat;
    Material normalMat;
    Material metallicMat;

    private void Start()
    {
        //substance = target.GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
        //ProceduralMaterial.substanceProcessorUsage = ProceduralProcessorUsage.All;
        //substance.CacheProceduralProperty("_BumpMap", true);
        baseMat = target.GetComponent<Renderer>().material;

        //Create Normal Mat
        //Texture normalMap = target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID("_BumpMap"));
        //normalMat = new Material(Shader.Find("Standard"));
        //normalMat.mainTexture = normalMap;
        CreateMaterialFrom("_BumpMap", normalMat);

        //Create Metallic Mat
        //CreateMaterialFrom("_Shininess", normalMat);
    }

    void CreateMaterialFrom(string property, Material _toAssing)
    {
        _toAssing = new Material(Shader.Find("Standard"));
        _toAssing.mainTexture = target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID(property));
    }

    public void DisplayWireframe()
    {
        
    }

    public void DisplayAlbedo()
    {
        target.GetComponent<Renderer>().material = baseMat;
    }

    public void DisplayNormal()
    {
        //Texture normalTex = substance.GetTexture("_BumpMap") as Texture;
        //substance.SetProceduralTexture("baseColor", (Texture2D)normalTex);
        //substance.RebuildTextures();

        Texture normalMap = target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID("_BumpMap"));
        ////target.GetComponent<Renderer>().material.SetTexture(Shader.PropertyToID("_MainTex"), normalMap);

        ////Create new material to display Normal
        Material normalMat = new Material(Shader.Find("Standard"));
        ////normalMat.SetTextureScale("Tiling", new Vector2(100, 0));
        normalMat.mainTexture = normalMap;
        target.GetComponent<Renderer>().material = normalMat;
    }

    public void DisplayMetallic()
    {

    }

    public void DisplayRoughness()
    {

    }
}
