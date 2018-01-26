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
    Material albedoMat;
    Material normalMat;
    Material metallicSpecMat; //this works to save both metallic and specular materials, depending on workflow
    Material roughnessGlossinessMat; //works for both workflows
    Material heightMat;
    Material alphaMat;
    Material emissionMat;

    [Tooltip("This is a panel with a text and a slider to display a substance property")]
    public GameObject propertyHolderTogglePrefab;
    public GameObject propertyHoldeSliderPrefab;
    [Tooltip("Child of canvas where the material properties will be displayed")]
    public GameObject propertyParent;

    private bool isMetallicWorkflow = false;

    [Header("UI")]
    public GameObject specularTggl;
    public GameObject metallicTggl;
    public GameObject roughnessTggl;
    public GameObject glossinessTggl;

    private bool displayAspect = false;

    public enum MapOptions
    {
        _0_NONE,
        _1_ALBEDO,
        _2_NORMAL,
        _3_METALLIC_SPECULAR,
        _4_ROUGH_GLOSS,
        _5_HEIGHT_MAP,
        _6_EMISSION_MAP,
        _7_ALPHA_MAP
    }

    private void Start()
    {
        substance = target.GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
        ProceduralMaterial.substanceProcessorUsage = ProceduralProcessorUsage.All;
        substance.CacheProceduralProperty("_MainTex", true);

        //Display Material Name
        DisplayMaterialName();

        //Guardar el material 
        baseMat = target.GetComponent<Renderer>().material;

        //Creaete albedo Mat
        CreateMaterialFrom("_MainTex", out albedoMat);

        //Create Normal Mat
        CreateMaterialFrom("_BumpMap", out normalMat); //_BumpMap

        if (isMetallicWorkflow)
        {
            //Create Metallic Mat
            CreateMaterialFrom("_MetallicGlossMap", out metallicSpecMat);
            //Create Roughness Mat
            CreateMaterialFrom("_RoughnessMap", out roughnessGlossinessMat); //CHECAR QUE ASI SE LLAME EN SHADER
        }
        else
        {
            CreateMaterialFrom("_SpecGlossMap", out metallicSpecMat);
            //Create Roughness Mat
            CreateMaterialFrom("_GloosMap", out roughnessGlossinessMat);
        }

        //Create Height Map
        CreateMaterialFrom("_ParallaxMap", out heightMat);
        //Create Alpha Map
        CreateMaterialFrom("_AlphaMap", out heightMat); //CONFIRMAR QU ESTE SEA EL NOMBRE EN EL SHADER
        //Create S
        CreateMaterialFrom("_EmissionMap", out heightMat);

        DisplaySubstanceMaterialProperties();
    }

    void CreateMaterialFrom(string property, out Material _toAssing)
    {
        _toAssing = new Material(Shader.Find("Standard"));
        if (target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID(property)) != null)
         _toAssing.mainTexture = target.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID(property));
    }

    public void DisplayMap(int option)
    {
        MapOptions map = (MapOptions)option;
       
        if(displayAspect)
        {
            switch (map)
            {
                case MapOptions._1_ALBEDO:
                    target.GetComponent<Renderer>().material = albedoMat;
                    break;
                case MapOptions._2_NORMAL:
                    target.GetComponent<Renderer>().material = normalMat;
                    break;
                case MapOptions._3_METALLIC_SPECULAR:
                    target.GetComponent<Renderer>().material = metallicSpecMat;
                    break;
                case MapOptions._4_ROUGH_GLOSS:
                    target.GetComponent<Renderer>().material = roughnessGlossinessMat;
                    break;
                case MapOptions._5_HEIGHT_MAP:
                    if(heightMat != null)
                        target.GetComponent<Renderer>().material = heightMat;
                    break;
                case MapOptions._6_EMISSION_MAP:
                    if (emissionMat != null)
                        target.GetComponent<Renderer>().material = emissionMat;
                    break;
                case MapOptions._7_ALPHA_MAP:
                    if (alphaMat != null)
                        target.GetComponent<Renderer>().material = alphaMat;
                    break;
            }
        }
        
    }
    
    public void DisplaySusbtanceMaterial()
    {
        if (!displayAspect)
        {
            displayAspect = true;
        }
        else
        {
            target.GetComponent<Renderer>().material = baseMat;
            displayAspect = false;
        }
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

        //print("VALUE CHANGED!" + inputFloat + " SLIDER VAL " + slider.value);
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
            //Para variables booleanas
            if (type == ProceduralPropertyType.Boolean)
            {
                GameObject holder = GameObject.Instantiate(propertyHolderTogglePrefab, propertyParent.transform);
                holder.GetComponentInChildren<Toggle>().GetComponentInChildren<Text>().text = input.name;
                holder.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { ToggleSubtanceProperty(input.name); });

            }
            //Para variables expuestas flotantes
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

    void DisplayMaterialName()
    {
        if(propertyParent != null)
        {
            string workflow = "Specular Glossiness";
            specularTggl.SetActive(true);
            glossinessTggl.SetActive(true);
            metallicTggl.SetActive(false);
            roughnessTggl.SetActive(false);
            //Check the workflow from the shader name
            //For Specular its Standard Specular
            //For metallic its Standard
            if (substance.shader.name =="Standard")
            {
                workflow = "Metallic Roughness";
                isMetallicWorkflow = true;

                specularTggl.SetActive(false);
                glossinessTggl.SetActive(false);
                metallicTggl.SetActive(true);
                roughnessTggl.SetActive(true);
            }

            propertyParent.transform.Find("txtTitulo").GetComponent<Text>().text = workflow + ": " + substance.name;
        }
        
    }
}
