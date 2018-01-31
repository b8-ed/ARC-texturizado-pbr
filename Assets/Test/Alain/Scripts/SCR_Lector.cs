using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SCR_Lector : MonoBehaviour
{
    StreamReader reader;
    DirectoryInfo dir;
    string nombre;
    string objeto;

    void Start ()
    {
        dir = new DirectoryInfo("Assets/Test/Alain/Objetos");
        FileInfo [] info =  dir.GetFiles();

        foreach (FileInfo file in info)
        {
            nombre = file.Name;
            string [] size = nombre.Split('.', '_');

            nombre = size[0] + " " + size[1];
            Debug.Log(nombre);

            if (file.Extension == ".meta")
            {
                reader = new StreamReader(file.ToString()); 
                for (int i = 0; i < 9; i++)
                    objeto = reader.ReadLine();

                string[] ssize = objeto.Split(null);
                   objeto = ssize[5];

                Debug.Log(objeto);
            }
        }
	}
}