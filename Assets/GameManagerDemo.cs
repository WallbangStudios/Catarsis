using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class GameManagerDemo : MonoBehaviour
{

    public static bool HasPistol;
    public static bool HasCard;
    public static bool HasKeys;
    [SerializeField]
    bool hascard, haskeys, haspistol;

    public MeshRenderer Lintern;
    public GameObject HUDShoot;
    // Start is called before the first frame update
    void Start()
    {
        HasCard = HasPistol = HasKeys = false;
    }

    // Update is called once per frame
    void Update()
    {
        hascard = HasCard;
        haskeys = HasKeys;
        haspistol = HasPistol;
        HUDShoot.SetActive(haspistol);



        //Lintern.enabled = haslintern;
    }
}
