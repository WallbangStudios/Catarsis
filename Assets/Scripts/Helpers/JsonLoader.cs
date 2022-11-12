using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Cargar los datos de los ataques
/// <summary>
/// Its The load of a data from Json
/// </summary>
public class JsonLoader <T>{
    /// <summary>
    /// Load all the Data from the Json 
    /// (Use this Only one time in any Awake Method of the Scene)
    /// </summary>
    public static T LoadData(string FileName) {
        //Se obtiene el path de donde esta el archivo
        string FilePath = Application.dataPath + "/JSONS/" + FileName + ".json";
        //Se lee el texto de forma plana
        string JsonString = File.ReadAllText(FilePath);
        //Se carga los datos en el objeto
        T Item = JsonUtility.FromJson<T>(JsonString);

        return Item;
    }

    public static void UpdateData(T Data, string FileName) {
        try
        {
            string FilePath = Application.dataPath + "/JSONS/" + FileName + ".json";
            string JsonString = JsonUtility.ToJson(Data);
            File.WriteAllText(FilePath, JsonString); 
        }
        catch (System.Exception)
        {
            Debug.Log("Error!");
        }
        
    }
    //Con esto ya puedes utilizar los datos del json
}


/// <summary>
/// its the meele attack class
/// </summary>
[System.Serializable]
public class MeeleAttack
{
    public string Name;
    public int Damage;
}

[System.Serializable]
public class NewMeeleAttack {
    public string Name;
    public int Damage;
    public Impact Impulse;
}


/// <summary>
/// The fire weapons class
/// </summary>
[System.Serializable]
public class DistanceAttack
{
    public string Name;
    public float FireRate;
    public float FieldOfFire;
    public float Recoil;
    public int Magazine;
    public int Damage;
}
[System.Serializable]
public class NewDistanceAttack
{
    public string Name;
    public float FireRate;
    public float FieldOfFire;
    public float Recoil;
    public int Magazine;
    public int Damage;
    public Impact Impulse;
}


/// <summary>
/// A list of Attacks
/// </summary>
[System.Serializable]
public class Attacks
{
    public List<MeeleAttack> MeeleAttacks;
    public List<DistanceAttack> DistanceAttacks;

}

[System.Serializable]
public class NewAttacks
{
    public List<NewMeeleAttack> MeeleAttacks;
    public List<NewDistanceAttack> DistanceAttacks;

}
