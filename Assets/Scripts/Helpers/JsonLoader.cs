using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Cargar los datos de los ataques
/// <summary>
/// Its The load of a data from Json
/// </summary>
public static class JsonLoader{

    //El path del archivo a cargar
    private static string FilePath;
    //El Json a cargar
    private static string JsonString;
    //El objeto que cargara todos los datos
    //Muy parecido a una base de datos Orientada a Objetos (OO)
    /// <summary>
    /// The atacks readed from Json DataBase
    /// </summary>
    public static Attacks attacks;
    
/*    void Awake() {
        FilePath = Application.dataPath + "/Atacks.json";
        JsonString = File.ReadAllText(FilePath);
        Attacks Attacks = JsonUtility.FromJson<Attacks>(JsonString);
        //JsonString = JsonUtility.ToJson(Attacks);
        //File.WriteAllText(FilePath, JsonString);
    }*/

    /// <summary>
    /// Load all the Data from the Json 
    /// (Use this Only one time in any Awake Method of the Scene)
    /// </summary>
    public static void LoadData() {
        //Se obtiene el path de donde esta el archivo
        FilePath = Application.dataPath + "/Scripts/Atacks.json";
        //Se lee el texto de forma plana
        JsonString = File.ReadAllText(FilePath);
        //Se carga los datos en el objeto
        attacks = JsonUtility.FromJson<Attacks>(JsonString);
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


/// <summary>
/// A list of Attacks
/// </summary>
[System.Serializable]
public class Attacks
{
    public List<MeeleAttack> MeeleAttacks;
    public List<DistanceAttack> DistanceAttacks;

}
