using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatClass : MonoBehaviour {

    public List<ProceduralMaterial> substance;
    public List<Material> baseMaterial;
    public List<Material> albedoMaterial;
    public List<Material> normalMaterial;
    public List<Material> metallicSpecMaterial;
    public List<Material> roughnessGlossinesMaterial;
    public List<Material> heightMaterial;
    public List<Material> alphaMaterial;
    public List<Material> emissionMaterial;

    public void Init()
    {
        substance = new List<ProceduralMaterial>();
        baseMaterial = new List<Material>();
        albedoMaterial = new List<Material>();
        normalMaterial = new List<Material>();
        metallicSpecMaterial = new List<Material>();
        roughnessGlossinesMaterial = new List<Material>();
        heightMaterial = new List<Material>();
        alphaMaterial = new List<Material>();
        emissionMaterial = new List<Material>();
    }

    public void Clear()
    {
        substance.Clear();
        baseMaterial.Clear();
        albedoMaterial.Clear();
        normalMaterial.Clear();
        metallicSpecMaterial.Clear();
        roughnessGlossinesMaterial.Clear();
        heightMaterial.Clear();
        alphaMaterial.Clear();
        emissionMaterial.Clear();
    }

}
