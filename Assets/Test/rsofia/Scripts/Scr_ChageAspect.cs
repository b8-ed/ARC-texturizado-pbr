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

    [Tooltip("This is a panel with a text and a slider to display a substance property")]
    public GameObject propertyHolderTogglePrefab;
    public GameObject propertyHoldeSliderPrefab;
    [Tooltip("Child of canvas where the material properties will be displayed")]
    public GameObject propertyParent;

    private void Start()
    {
        substance = target.GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
        ProceduralMaterial.substanceProcessorUsage = ProceduralProcessorUsage.All;
        substance.CacheProceduralProperty("_MainTex", true);

        //Guardar el material normal
        baseMat = target.GetComponent<Renderer>().material;

        //Create Normal Mat
        CreateMaterialFrom("_BumpMap", out normalMat);

        //Create Metallic Mat
        //CreateMaterialFrom("_Shininess", normalMat);


        DisplaySubstanceMaterialProperties();
    }

    void CreateMaterialFrom(string property, out Material _toAssing)
    {
        _toAssing = new Material(Shader.Find("Standard"));
        _toAssing.mainTexture = target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID(property));
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
        string strSize = dropdownResize.options[dropdownResize.value].text;
        ResizeTextureOfMaterial(baseMat, int.Parse(strSize.ToString()));
        ResizeTextureOfMaterial(normalMat, int.Parse(strSize.ToString()));

    }

    private void ResizeTextureOfMaterial(Material mat, int size)
    {
        //ProceduralPropertyDescription[] inputs = substance.GetProceduralPropertyDescriptions();
        //Texture2D tempText =  mat.GetTexture(Shader.PropertyToID("_MainTex")) as Texture2D;
        
        //print("Main Texture: " + tempText + " main texture " + mat.mainTexture);
        //if (tempText != null)
        //{
        //    tempText.Resize(size, size);
        //    tempText.Apply();
        //    mat.mainTexture = tempText;
        //}
        //else
        //    print("Temp text is null");

    }

    public void ToggleSubtanceProperty(string inputName)
    {
        bool inputBool = substance.GetProceduralBoolean(inputName);
        bool oldInputBool = inputBool;
        inputBool = GUILayout.Toggle(inputBool, inputName);
        if (inputBool != oldInputBool)
        {
            substance.SetProceduralBoolean(inputName, inputBool);
            substance.RebuildTextures();
        }
    }

    public void SlideSubstanceProperty(ProceduralPropertyDescription input, Slider slider)
    {
        float inputFloat = substance.GetProceduralFloat(input.name);
        float oldInputFloat = inputFloat;

        print("VALUE CHANGED!" + inputFloat + " SLIDER VAL " + slider.value);
        inputFloat = slider.value;//GUILayout.HorizontalSlider(inputFloat, input.minimum, input.maximum);
        if (inputFloat != oldInputFloat)
        {
            substance.SetProceduralFloat(input.name, inputFloat);
            substance.RebuildTextures();
        }
    }

    public void DisplaySubstanceMaterialProperties()
    {
        ProceduralPropertyDescription[] inputs = substance.GetProceduralPropertyDescriptions();
        int i = 0;
        while (i < inputs.Length)
        {
            ProceduralPropertyDescription input = inputs[i];
            ProceduralPropertyType type = input.type;
            if (type == ProceduralPropertyType.Boolean)
            {
                GameObject holder = GameObject.Instantiate(propertyHolderTogglePrefab, propertyParent.transform);
                holder.GetComponentInChildren<Toggle>().GetComponentInChildren<Text>().text = input.name;
                holder.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { ToggleSubtanceProperty(input.name); });

            }
            else if (type == ProceduralPropertyType.Float)
                if (input.hasRange)
                {
                    GameObject holder = GameObject.Instantiate(propertyHoldeSliderPrefab, propertyParent.transform);
                    holder.transform.Find("txt").GetComponent<Text>().text = input.name;
                    holder.GetComponentInChildren<Slider>().onValueChanged.AddListener(delegate { SlideSubstanceProperty(input, holder.GetComponentInChildren<Slider>()); });
                }
                else if (type == ProceduralPropertyType.Vector2 || type == ProceduralPropertyType.Vector3 || type == ProceduralPropertyType.Vector4)
                    if (input.hasRange)
                    {
                        GUILayout.Label(input.name);
                        int vectorComponentAmount = 4;
                        if (type == ProceduralPropertyType.Vector2)
                            vectorComponentAmount = 2;

                        if (type == ProceduralPropertyType.Vector3)
                            vectorComponentAmount = 3;

                        Vector4 inputVector = substance.GetProceduralVector(input.name);
                        Vector4 oldInputVector = inputVector;
                        int c = 0;
                        while (c < vectorComponentAmount)
                        {
                            inputVector[c] = GUILayout.HorizontalSlider(inputVector[c], input.minimum, input.maximum);
                            c++;
                        }
                        if (inputVector != oldInputVector)
                            substance.SetProceduralVector(input.name, inputVector);
                    }
                    else if (type == ProceduralPropertyType.Color3 || type == ProceduralPropertyType.Color4)
                    {
                        GUILayout.Label(input.name);
                        int colorComponentAmount = ((type == ProceduralPropertyType.Color3) ? 3 : 4);
                        Color colorInput = substance.GetProceduralColor(input.name);
                        Color oldColorInput = colorInput;
                        int d = 0;
                        while (d < colorComponentAmount)
                        {
                            colorInput[d] = GUILayout.HorizontalSlider(colorInput[d], 0, 1);
                            d++;
                        }
                        if (colorInput != oldColorInput)
                            substance.SetProceduralColor(input.name, colorInput);
                    }
                    else if (type == ProceduralPropertyType.Enum)
                    {
                        GUILayout.Label(input.name);
                        int enumInput = substance.GetProceduralEnum(input.name);
                        int oldEnumInput = enumInput;
                        string[] enumOptions = input.enumOptions;
                        enumInput = GUILayout.SelectionGrid(enumInput, enumOptions, 1);
                        if (enumInput != oldEnumInput)
                            substance.SetProceduralEnum(input.name, enumInput);
                    }
            i++;
        }
        substance.RebuildTextures();

    }
}
