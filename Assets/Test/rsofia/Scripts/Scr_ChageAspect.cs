//ruth sofia brown
//git rsofia
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scr_ChageAspect : MonoBehaviour
{
    [Tooltip("El objeto 3d del cual va a tomar los materiales")]
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

        //Guardar el material normal
        baseMat = target.GetComponent<Renderer>().material;

        //Create Normal Mat
        CreateMaterialFrom("_BumpMap", out normalMat);

        //Create Metallic Mat
        //CreateMaterialFrom("_Shininess", normalMat);
    }

    void CreateMaterialFrom(string property, out Material _toAssing)
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
        //Asignar el material de normal
        target.GetComponent<Renderer>().material = normalMat;
    }

    public void DisplayMetallic()
    {

    }

    public void DisplayRoughness()
    {

    }

    public void ResizeTextures(Dropdown dropdownResize)
    {
        //Cambiar la textura principal y el bumpmap
        //print("Option: " + dropdownResize.options[dropdownResize.value].text);
        string strSize = dropdownResize.options[dropdownResize.value].text;
        ResizeTextureOfMaterial(baseMat, int.Parse(strSize.ToString()));
        ResizeTextureOfMaterial(normalMat, int.Parse(strSize.ToString()));

    }

    private void ResizeTextureOfMaterial(Material mat, int size)
    {
        Texture2D tempText = (mat.mainTexture as Texture2D);

        //RenderTexture renderTexture = RenderTexture.active;
        //RenderTexture.active = mat.mainTexture as RenderTexture;
        //// width and height, chosen to match the source r.t.
        //Texture2D tex = new Texture2D(mat.mainTexture.width, mat.mainTexture.height, TextureFormat.ARGB32, false);
        //// Grab everything
        //tex.ReadPixels(new Rect(0f, 0f, mat.mainTexture.width, mat.mainTexture.height), 0, 0);
        //tex.Apply();
        //RenderTexture.active = renderTexture;


        print("Main Texture: " + tempText);
        if (tempText != null)
        {
            tempText.Resize(size, size);
            mat.mainTexture = tempText;
        }

    }
}
