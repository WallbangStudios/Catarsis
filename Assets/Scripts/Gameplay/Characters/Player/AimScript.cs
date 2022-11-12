using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AimScript : MonoBehaviour {

    [HideInInspector]
    public DistanceAttack ShootAttack;
    Playercontroller PC;
    
    [HideInInspector]
    public static Vector3 AimDirection;
    [HideInInspector]
    public static bool CanMove;
    public float TimeRest;
    float lastpress;

    Ammo AmmoDonovan = new Ammo();
    public int TotalAmmo;

    public int indexGun = 0;

    public GameObject Donovan;
    public Light ShootLight;
    bool illuminate=false;
    float MaxIntense;
    public float DegVelIntense;

    public void IlluminateShoot() {
        if (illuminate)
        {

            ShootLight.intensity = MaxIntense;

            illuminate = false;


        }
        else
        {
            if(ShootLight.intensity > 0)
                ShootLight.intensity = ShootLight.intensity - DegVelIntense;

        }
    }

    public LayerMask LayerShoot;

    [Range(0,360)]
    public static float angle=270;

    [Range(0,180)]
    public float FieldOfFire = 25f;
    public float radiusofFire = 2;
    public float FireRate = 15f;

    float TempField;

    [HideInInspector]
    public float Shootangle;
    Vector3 viewShootAngle;
    float NextTimeToFire=0f;

    float suma = 1;
    float mult = 1;

    Attacks attacks;

    public Vector3 DirFromAngle(float AngleInDegrees,bool isGlobal) {
        if (!isGlobal) {
            AngleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(AngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(AngleInDegrees * Mathf.Deg2Rad), 0);
    }

	// Use this for initialization
	void Start () {
        attacks = JsonLoader<Attacks>.LoadData("Attacks");
        ShootAttack = attacks.DistanceAttacks[indexGun];
        TempField = FieldOfFire;
        Shootangle = angle; 
        viewShootAngle = DirFromAngle(Shootangle, false);
        MaxIntense = ShootLight.intensity;
        ShootLight.intensity = 0;
        PC = Donovan.GetComponent<Playercontroller>();
        AmmoDonovan.TotalAmmo = TotalAmmo;
        AmmoDonovan.AmmoInMag = ShootAttack.Magazine;
	}
	
	// Update is called once per frame
	void Update () {
        if(PauseMenu.GameIsPaused)
            return;

        float timer=2;
        if (Time.timeSinceLevelLoad > 1)
            timer = Time.timeSinceLevelLoad;
        
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            lastpress = Time.timeSinceLevelLoad;

        CanMove = timer - lastpress > TimeRest;
        if (Input.GetKeyDown(KeyCode.E)) {
            indexGun++;
            if (indexGun >= attacks.DistanceAttacks.ToArray().Length) {
                indexGun = 0;
            }
        }

        ShootAttack = attacks.DistanceAttacks[indexGun];
        TempField = ShootAttack.FieldOfFire;
        FireRate = ShootAttack.FireRate;
        suma = 1;

        if (Input.GetMouseButton(1))
        {
            FieldOfFire = 1;
            mult = 1;
        }
        else {
            FieldOfFire = TempField;
            mult = 2;
        }
        /*
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            angletest += suma * Input.GetAxisRaw("Vertical")*mult;
        }*/
        if (!CanMove){
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AimDirection = (pos - transform.parent.transform.position).normalized;
            angle = Mathf.Atan2(AimDirection.x, AimDirection.y) * Mathf.Rad2Deg;
        }
        
        //angletest = Mathf.Clamp(angletest, 181, 359);
        //angletest = -Vector2.Angle(ArmsScript.position,Input.mousePosition);

        float Aalpha = angle + FieldOfFire / 2;
        float Balpha = angle - FieldOfFire / 2;

        Vector3 ViewAngleA = DirFromAngle(Aalpha,false);
        Vector3 ViewAngleB = DirFromAngle(Balpha, false);
        Vector3 ViewAngle = DirFromAngle(angle, false);

        if (Input.GetMouseButton(0) && Time.time>=NextTimeToFire && PC.CanShoot && AmmoDonovan.AmmoInMag>0) {
            NextTimeToFire = Time.time + 1 / FireRate;
            Shootangle = Random.Range(Aalpha,Balpha);
            viewShootAngle = DirFromAngle(Shootangle, false);
            //angletest+=ShootAttack.Recoil;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, viewShootAngle, radiusofFire,LayerShoot.value);
            if (hit.collider!=null) {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponentInParent<BoxCollider2D>().SendMessage("Damaged",ShootAttack);
                }
                else {
                    Destroy(hit.transform.gameObject);
                }
            }
            illuminate = true;
            AmmoDonovan.AmmoInMag--;
        }

        if (Input.GetKeyDown(KeyCode.R) && AmmoDonovan.AmmoInMag < ShootAttack.Magazine) {
            int RestAmmo = Mathf.Clamp(AmmoDonovan.TotalAmmo, 0, ShootAttack.Magazine - AmmoDonovan.AmmoInMag);
            AmmoDonovan.AmmoInMag += RestAmmo;
            AmmoDonovan.TotalAmmo -= RestAmmo;
        }

        //print(ShootAttack.Name + "/" + AmmoDonovan.AmmoInMag + "-" + AmmoDonovan.TotalAmmo);

        IlluminateShoot();
        Debug.DrawLine(transform.position, transform.position + ViewAngle * radiusofFire, Color.blue);
        Debug.DrawLine(transform.position, transform.position + ViewAngleA * radiusofFire, Color.white);
        Debug.DrawLine(transform.position, transform.position + ViewAngleB * radiusofFire, Color.white);
        Debug.DrawLine(transform.position, transform.position + viewShootAngle * radiusofFire, Color.red);
        
	}
}
