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
    private GameObject target; //el padre de los objetos de substance
    List<ProceduralMaterial> substance = new List<ProceduralMaterial>(); //todos los substance del objeto

    //Materiales para almacenar temporalmente el materlial de cada objeto
    List<Material> baseMat = new List<Material>();
    List<Material> albedoMat = new List<Material>();
    List<Material> normalMat = new List<Material>();
    List<Material> metallicSpecMat = new List<Material>(); //this works to save both metallic and specular materials, depending on workflow
    List<Material> roughnessGlossinessMat = new List<Material>(); //works for both workflows
    List<Material> heightMat = new List<Material>();
    List<Material> alphaMat = new List<Material>();
    List<Material> emissionMat = new List<Material>();

    [Header("Procedural")]
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
    public Toggle[] togglesToTurnOff;

    [Header("Normal Map")]
    [Range(10f, 20f)]
    public float distortion;


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

    //Esta funcion se llama cada que se crea un objeto nuevo
    public void LoadNewObject(GameObject parentOfSubstances)
    {
        //borrar lo que ya haya en la ui para las variables expuestas
        ClearUI();

        target = parentOfSubstances;
        //Sacar cada hijo con un substance
        substance.Clear();
        baseMat.Clear();
        albedoMat.Clear();
        normalMat.Clear();
        metallicSpecMat.Clear();
        roughnessGlossinessMat.Clear();
        heightMat.Clear();
        alphaMat.Clear();
        emissionMat.Clear();
        foreach (Transform child in parentOfSubstances.transform)
        {
           
            //Tomar el substance como material procedural y agregarlo a la lista
            ProceduralMaterial subTemp = child.GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
            ProceduralMaterial.substanceProcessorUsage = ProceduralProcessorUsage.All;
            if (subTemp != null)
            {
                subTemp.CacheProceduralProperty("_MainTex", true);

                //Solo se va a checar el workflow con el primer objeto que tenga material
                if (child.GetSiblingIndex() == 0)
                {
                    //Display Material Name
                    DisplayMaterialName(subTemp, target.name);
                }

                //Guardar el material 
                baseMat.Add(child.GetComponent<Renderer>().material);

                //Creaete albedo Mat
                Material tempMat;
                CreateMaterialFrom("_MainTex", out tempMat, child, subTemp);
                albedoMat.Add(tempMat);

                //Create Normal Mat
                tempMat = null;
                CreateMaterialFrom("_BumpMap", out tempMat, child, subTemp, true); //_BumpMap
                normalMat.Add(tempMat);


                if (isMetallicWorkflow)
                {
                    //Create Metallic Mat
                    tempMat = null;
                    CreateMaterialFrom("_MetallicGlossMap", out tempMat, child, subTemp);
                    metallicSpecMat.Add(tempMat);
                    //Create Roughness Mat
                    tempMat = null;
                    CreateMaterialFrom("_RoughnessMap", out tempMat, child, subTemp); //CHECAR QUE ASI SE LLAME EN SHADER
                    roughnessGlossinessMat.Add(tempMat);
                }
                else
                {
                    tempMat = null;
                    CreateMaterialFrom("_SpecGlossMap", out tempMat, child, subTemp);
                    metallicSpecMat.Add(tempMat);
                    //Create Roughness Mat
                    tempMat = null;
                    CreateMaterialFrom("_GloosMap", out tempMat, child, subTemp);
                    roughnessGlossinessMat.Add(tempMat);
                }

                //Create Height Map
                tempMat = null;
                CreateMaterialFrom("_ParallaxMap", out tempMat, child, subTemp);
                heightMat.Add(tempMat);
                //Create Alpha Map
                tempMat = null;
                CreateMaterialFrom("_AlphaMap", out tempMat, child, subTemp); //CONFIRMAR QU ESTE SEA EL NOMBRE EN EL SHADER
                alphaMat.Add(tempMat);
                tempMat = null;                           //Create S
                CreateMaterialFrom("_EmissionMap", out tempMat, child, subTemp);
                emissionMat.Add(tempMat);

                //Agregar a la UI
                if (subTemp != null)
                    DisplaySubstanceMaterialProperties(subTemp);

                //Agregar el substance a la lista
                substance.Add(subTemp);

            }

           
        }
    }

    private void ClearUI()
    {
        for(int i  = propertyParent.transform.childCount - 1; i >= 0; i--)
        {
            if (propertyParent.transform.GetChild(i).name != "txtTitulo")
                Destroy(propertyParent.transform.GetChild(i).gameObject);
        }
    }

    private void CreateMaterialFrom(string property, out Material _toAssing, Transform _child, ProceduralMaterial mySubstance, bool _isNormal = false)
    {
        _toAssing = new Material(Shader.Find("Standard"));
        try
        {
            //if (_child.GetComponent<Renderer>().material.GetTexture(Shader.PropertyToID(property)) != null)
                _toAssing.mainTexture = mySubstance.GetTexture(Shader.PropertyToID(property));
        }
        catch
        {
            Debug.Log("Objeto no tiene propiedad.");
        }

        if (_isNormal)
        {
            _toAssing.SetTexture("_BumpMap", mySubstance.GetTexture(Shader.PropertyToID("_BumpMap")));

            _toAssing.mainTexture = ToNormalMap(mySubstance.GetTexture(Shader.PropertyToID("_BumpMap")));
        }
    }

    public static float sCurve(float x, float distortion)
    {
        return 1f / (1f + Mathf.Exp(-Mathf.Lerp(5f, 15f, distortion) * (x - 0.5f)));
    }

    Texture2D ToNormalMap(Texture tex)
    {
        Texture2D t = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(tex.width, tex.height, 32);
        Graphics.Blit(tex, renderTexture);

        RenderTexture.active = renderTexture;
        t.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        t.Apply();

        RenderTexture.active = currentRT;

        Texture2D n = new Texture2D(t.width, t.height, TextureFormat.ARGB32, true);

        for (int y = 1; y < t.width - 1; y++)
        {
            for (int x = 1; x < t.height * 2 - 1; x++)
            {
                float xLeft = t.GetPixel(x - 1, y).grayscale;
                float xRight = t.GetPixel(x + 1, y).grayscale;
                float yUp = t.GetPixel(x, y - 1).grayscale;
                float yDown = t.GetPixel(x, y + 1).grayscale;
                float xDelta = ((xLeft - xRight) + 1) * .5f;
                float yDelta = ((yUp - yDown) + 1) * .5f;
                //n.SetPixel(x,y,new Color(xDelta,yDelta,1f,1f));
                //if((new Color( Mathf.Clamp01(sCurve(xDelta, distortion)) , Mathf.Clamp01(sCurve(yDelta, distortion)),1f,1.0f)) == Color.white) print(x+ " " + y  );
                n.SetPixel(x, y, new Color(Mathf.Clamp01(sCurve(xDelta, distortion)), Mathf.Clamp01(sCurve(yDelta, distortion)), 1f, 1.0f));
            }
        }
        n.Apply();

        return n;
    }

    public void DisplayMap(int option)
    {
        scr_ShaderWF.DesactiveWF();

        MapOptions map = (MapOptions)option;
       
        if(displayAspect)
        {
            switch (map)
            {
                case MapOptions._1_ALBEDO:
                    ChangeMaterialTo(ref albedoMat);
                    break;
                case MapOptions._2_NORMAL:
                    ChangeMaterialTo(ref normalMat, true); 
                    break;
                case MapOptions._3_METALLIC_SPECULAR:
                    ChangeMaterialTo(ref metallicSpecMat);
                    break;
                case MapOptions._4_ROUGH_GLOSS:
                    ChangeMaterialTo(ref roughnessGlossinessMat);
                    break;
                case MapOptions._5_HEIGHT_MAP:
                    if(heightMat != null)
                        ChangeMaterialTo(ref heightMat);
                    break;
                case MapOptions._6_EMISSION_MAP:
                    if (emissionMat != null)
                        ChangeMaterialTo(ref emissionMat);
                    break;
                case MapOptions._7_ALPHA_MAP:
                    if (alphaMat != null)
                        ChangeMaterialTo(ref alphaMat);
                    break;
            }
        }
        else
        {
            foreach(Toggle toggle in togglesToTurnOff)
            {
                toggle.isOn = false;
            }
        }
        
    }

    private void ChangeMaterialTo(ref List<Material> _matToChange, bool isNormal = false)
    {
        //cambiar todos sus materiales hijos
        for(int i = 0; i < target.transform.childCount; i++)
        {
            if(!isNormal)
             target.transform.GetChild(i).GetComponent<Renderer>().material = _matToChange[i];
            else
            {                 
                _matToChange[i].mainTexture = ToNormalMap(substance[i].GetTexture(Shader.PropertyToID("_BumpMap")));
                target.transform.GetChild(i).GetComponent<Renderer>().material = _matToChange[i];
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
            foreach (Toggle toggle in togglesToTurnOff)
            {
                toggle.isOn = false;
            }

            for (int i = 0; i < target.transform.childCount; i++)
            {
                target.transform.GetChild(i).GetComponent<Renderer>().material = baseMat[i];
            }            

            displayAspect = false;
        }
    }

    public void ResizeTextures(Dropdown dropdownResize )
    {
        ////Cambiar la textura principal y el bumpmap
        //string strSize = dropdownResize.options[dropdownResize.value].text;
        //ResizeTextureOfMaterial(baseMat, int.Parse(strSize.ToString()));
        //ResizeTextureOfMaterial(normalMat, int.Parse(strSize.ToString()));

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

    public void ToggleSubtanceProperty(string inputName, ProceduralMaterial mySubstance)
    {
        bool inputBool = mySubstance.GetProceduralBoolean(inputName);
        bool oldInputBool = inputBool;
        inputBool = GUILayout.Toggle(inputBool, inputName);
        if (inputBool != oldInputBool)
        {
            mySubstance.SetProceduralBoolean(inputName, inputBool);
            mySubstance.RebuildTextures();
        }
    }

    public void SlideSubstanceProperty(ProceduralPropertyDescription input, Slider slider, ProceduralMaterial mySubstance)
    {
        float inputFloat = mySubstance.GetProceduralFloat(input.name);
        float oldInputFloat = inputFloat;

        //print("VALUE CHANGED!" + inputFloat + " SLIDER VAL " + slider.value);
        inputFloat = slider.value;//GUILayout.HorizontalSlider(inputFloat, input.minimum, input.maximum);
        if (inputFloat != oldInputFloat)
        {
            mySubstance.SetProceduralFloat(input.name, inputFloat);
            mySubstance.RebuildTextures();
        }
    }

    public void DisplaySubstanceMaterialProperties(ProceduralMaterial mySubstance)
    {
        ProceduralPropertyDescription[] inputs = mySubstance.GetProceduralPropertyDescriptions();
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
                holder.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { ToggleSubtanceProperty(input.name, mySubstance); });

            }
            //Para variables expuestas flotantes
            else if (type == ProceduralPropertyType.Float)
                if (input.hasRange)
                {
                    GameObject holder = GameObject.Instantiate(propertyHoldeSliderPrefab, propertyParent.transform);
                    holder.transform.Find("txt").GetComponent<Text>().text = input.name;
                    holder.GetComponentInChildren<Slider>().onValueChanged.AddListener(delegate { SlideSubstanceProperty(input, holder.GetComponentInChildren<Slider>(), mySubstance); });
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

                        Vector4 inputVector = mySubstance.GetProceduralVector(input.name);
                        Vector4 oldInputVector = inputVector;
                        int c = 0;
                        while (c < vectorComponentAmount)
                        {
                            inputVector[c] = GUILayout.HorizontalSlider(inputVector[c], input.minimum, input.maximum);
                            c++;
                        }
                        if (inputVector != oldInputVector)
                            mySubstance.SetProceduralVector(input.name, inputVector);
                    }
                    else if (type == ProceduralPropertyType.Color3 || type == ProceduralPropertyType.Color4)
                    {
                        GUILayout.Label(input.name);
                        int colorComponentAmount = ((type == ProceduralPropertyType.Color3) ? 3 : 4);
                        Color colorInput = mySubstance.GetProceduralColor(input.name);
                        Color oldColorInput = colorInput;
                        int d = 0;
                        while (d < colorComponentAmount)
                        {
                            colorInput[d] = GUILayout.HorizontalSlider(colorInput[d], 0, 1);
                            d++;
                        }
                        if (colorInput != oldColorInput)
                            mySubstance.SetProceduralColor(input.name, colorInput);
                    }
                    else if (type == ProceduralPropertyType.Enum)
                    {
                        GUILayout.Label(input.name);
                        int enumInput = mySubstance.GetProceduralEnum(input.name);
                        int oldEnumInput = enumInput;
                        string[] enumOptions = input.enumOptions;
                        enumInput = GUILayout.SelectionGrid(enumInput, enumOptions, 1);
                        if (enumInput != oldEnumInput)
                            mySubstance.SetProceduralEnum(input.name, enumInput);
                    }
            i++;
        }
        mySubstance.RebuildTextures();

    }

    void DisplayMaterialName(Material mySubstance, string objectName)
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
            if (mySubstance.shader.name =="Standard")
            {
                workflow = "Metallic Roughness";
                isMetallicWorkflow = true;

                specularTggl.SetActive(false);
                glossinessTggl.SetActive(false);
                metallicTggl.SetActive(true);
                roughnessTggl.SetActive(true);
            }

            propertyParent.transform.Find("txtTitulo").GetComponent<Text>().text = workflow + ": " + objectName;
        }
        
    }
}
