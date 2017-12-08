using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class scr_GM : MonoBehaviour {

    public GameObject ParentModel;

    public List<GameObject> Models = new List<GameObject>();
    List<Shader> StandartSH = new List<Shader>();

    public Shader Wireframe;
    

    int IndexModel = 0;

    public Text TitleModel;

    public Toggle Tg_Wireframe;

	// Use this for initialization
	void Start () {
        GameObject[] models = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
        for(int i=0; i<models.Length; i++)
        {
            GameObject model = Instantiate(models[i], ParentModel.transform);
            Models.Add(model);
            StandartSH.Add(Models[i].GetComponent<MeshRenderer>().material.shader);
            if (i > 0)
                Models[i].SetActive(false);
        }
        UpdateInfoModel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetWireFrame()
    {
        if (Tg_Wireframe.isOn)
        {
            Models[IndexModel].GetComponent<MeshRenderer>().material.shader = Wireframe;
        } else
        {
            Models[IndexModel].GetComponent<MeshRenderer>().material.shader = StandartSH[IndexModel];
        }

    }

    public void NextModel()
    {
        Models[IndexModel].SetActive(false);
        IndexModel++;
        if (IndexModel >= Models.Count)
            IndexModel = 0;
        UpdateInfoModel();
    }

    public void PrevModel()
    {
        Models[IndexModel].SetActive(false);
        IndexModel--;
        if (IndexModel < 0)
            IndexModel = Models.Count - 1;
        UpdateInfoModel();
    }

    void UpdateInfoModel()
    {
        ParentModel.transform.rotation = Quaternion.identity;
        Models[IndexModel].SetActive(true);
        TitleModel.text = Models[IndexModel].name;
    }
}
