using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDmanager : MonoBehaviour {

    public Human HUMAN;

    Animator InvAnimator;
    bool IsInvOpen = false;

    public GameObject HUDShoot;
    public Text Magazine;
    public Text TotalAmmo;
    public Text TotalAmmoShadow;
    public List<RawImage> Mags;

    public ContextScript Context;
    public Text ContextText;

    public List<Animator> Lifes;

    public AiluroPhobia Boss;
    public RectTransform Bar;

    float MaxBar;
	// Use this for initialization
	void Start () {
        InvAnimator = gameObject.GetComponentInChildren<Animator>();
        MaxBar = Bar.rect.width;
	}

    public float smoothbar;
    float refbar;
	
	// Update is called once per frame
	void Update () {

        Bar.transform.parent.gameObject.SetActive(Boss.triger.entrar_ascensor);

        float bar = MaxBar * Boss.CurrentHealth / Boss.MaxHealth;

        Bar.sizeDelta = new Vector2(Mathf.SmoothDamp(Bar.sizeDelta.x, bar, ref refbar, smoothbar), Bar.sizeDelta.y);


        for (int i = 0; i < Lifes.Count; i++)
        {
            if (HUMAN.CurrentHealth >= i + 1)
            {
                Lifes[i].SetBool("HasLife", true);
            }
            else
            {
                Lifes[i].SetBool("HasLife", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.I)){
            IsInvOpen = !IsInvOpen;
            InvAnimator.SetBool("IsOpen", IsInvOpen);
        }
        if (GameManagerDemo.HasPistol)
        {
            //HUDShoot.SetActive(true);
            Magazine.text = HUMAN.ShootingObject.AmmoDonovan.AmmoInMag.ToString();
            float TotalMagazines = HUMAN.ShootingObject.AmmoDonovan.TotalAmmo / (float)HUMAN.ShootingObject.ShootAttack.Magazine;
            int TotalMags = Mathf.CeilToInt(TotalMagazines);
            int MagsForUI = Mathf.Clamp(TotalMags, 0, 3);
            for (int i = 0; i < Mags.Count; i++) {
                if (MagsForUI >= i + 1)
                {
                    Mags[i].enabled = true;
                }
                else {
                    Mags[i].enabled = false;
                }
            }
            TotalAmmo.text = HUMAN.ShootingObject.AmmoDonovan.TotalAmmo.ToString();
            TotalAmmoShadow.text = HUMAN.ShootingObject.AmmoDonovan.TotalAmmo.ToString();

        }
        //TXT.text = HUMAN.ShootingObject.ShootAttack.Name.ToUpperInvariant() + " / " 
        //  + HUMAN.ShootingObject.AmmoDonovan.AmmoInMag + " - " + HUMAN.ShootingObject.AmmoDonovan.TotalAmmo;
        //else
        //    HUDShoot.SetActive(false);

	}

    public void SetContext(string Sentence)
    {
        if (Context.InSentence)
            return;
        ContextText.text = Sentence;
        Context.InSentence = true;
        Context.gameObject.SetActive(true);
    }
}
