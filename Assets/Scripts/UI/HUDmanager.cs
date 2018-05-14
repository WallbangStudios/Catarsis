using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDmanager : MonoBehaviour {

    TextMeshProUGUI TXT;
    public Human HUMAN;

	// Use this for initialization
	void Start () {
        TXT = gameObject.GetComponentInChildren<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        if (HUMAN.WithGun)
            TXT.text = HUMAN.ShootingObject.ShootAttack.Name.ToUpperInvariant() + " / " 
                + HUMAN.ShootingObject.AmmoDonovan.AmmoInMag + " - " + HUMAN.ShootingObject.AmmoDonovan.TotalAmmo;
        else
            TXT.text = "";
	}
}
