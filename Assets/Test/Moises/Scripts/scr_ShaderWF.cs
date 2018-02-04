using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ShaderWF : MonoBehaviour {

    public Shader WF;

    public static List<GameObject> Models;
    public static List<Shader> Baseshaders;

    void Start()
    {
        Models = new List<GameObject>();
        Baseshaders = new List<Shader>();
    }

    public static void InitShwf()
    {
        if (Models != null)
            Models.Clear();
        else
            Models = new List<GameObject>();
        if (Baseshaders != null)
            Baseshaders.Clear();
        else
            Baseshaders = new List<Shader>();
        foreach (MeshRenderer mr in FindObjectsOfType<MeshRenderer>())
        {
            Models.Add(mr.gameObject);
        }
        for (int i = 0; i < Models.Count; i++)
        {
            MeshRenderer ms = Models[i].GetComponent<MeshRenderer>();

            if (ms == null)
                return;
            for (int j = 0; j < ms.materials.Length; j++)
            {
                Baseshaders.Add(ms.materials[j].shader);
            }
        }
        
    }
	
    public void ActiveWF()
    {
        for (int i = 0; i < Models.Count; i++)
        {
            MeshRenderer ms = Models[i].GetComponent<MeshRenderer>();

            if (ms == null)
                return;
            for (int j = 0; j < ms.materials.Length; j++)
            {
                ms.materials[j].shader = WF;
            }
        }
    }

    public static void DesactiveWF()
    {
        int s = 0;
        for (int i = 0; i < Models.Count; i++)
        {
            MeshRenderer ms = Models[i].GetComponent<MeshRenderer>();

            if (ms == null)
                return;
            for (int j = 0; j < ms.materials.Length; j++)
            {
                ms.materials[j].shader = Baseshaders[s];
                s++;
            }
        }
    }
}
