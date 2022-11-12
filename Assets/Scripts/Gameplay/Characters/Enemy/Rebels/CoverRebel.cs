using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class CoverRebel : Character {

    public enum States {
        Cover,
        shooting
    }

    States State;

    //Distancia de reconocimiento
    public float Radius;

    //Para el Raycast
    public LayerMask Layer;

    //Define los estados
    public bool InAction { get; private set; }

    public bool AimAction { get; private set; }

    AnimatorStateInfo StInf;

    float LastEncountered;
    float LastAimed;
    float LastShootCant;

    bool CanShoot;

    Attacks attacks;

    DistanceAttack Shootattack;
    //Start
    public override void LoadData()
    {
        base.LoadData();

        State = States.Cover;

        ShootLight1.LoadData();
        ShootLight2.LoadData();
        
        //cargar el json de los ataques
        attacks = JsonLoader<Attacks>.LoadData("Atacks");

        Shootattack = attacks.DistanceAttacks[1];

    }
    #region VariablesAuxiliares
    //Estas variables se usasn para las funciones trigger
    bool AuxiliarA;
    bool AuxiliarB;
    #endregion
    //each frame
    public override void UpdateThis()
    {
        base.UpdateThis();
        #region StatesInfos
        StInf = anim.GetCurrentAnimatorStateInfo(0);
        bool Aim = StInf.IsName("Aim");
        bool OnCover = StInf.IsName("Cover");
        bool Shoting = StInf.IsName("Shooting");
        #endregion
        //Circulo que analiza si esta donovan en una distancia a la redonda
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, Radius, Vector2.zero, 0, Layer);
        //Si esta, entonces hay accion
        InAction = hit.collider != null;
        if (InAction){
            /*Se lanzara un raycast donde se analiza si esta o no agachado*/
            RaycastHit2D A = Physics2D.Raycast(transform.position + Vector3.up * 4.4f,
                    Vector2.right * transform.localScale.x, Radius, Layer);
            Debug.DrawLine(transform.position + Vector3.up * 4.4f,
                transform.position + Vector3.up * 4.4f + Vector3.right * transform.localScale.x * Radius, Color.blue);
            bool Triggercover = Trigger(OnCover, ref AuxiliarA);
            if (Triggercover) {
                LastEncountered = timer;
                print(LastEncountered);
            }
            if (State == States.Cover) {
                if (OnCover){
                    if (A != null){
                        if (timer - LastEncountered > 2f){

                            anim.SetBool("Aim", true);
                            State = States.shooting;
                            LastAimed = timer;
                            LastShootCant = timer;
                            CanShoot = false;
                            print("set true");
                        }
                    }
                    else {
                        if (timer - LastEncountered > 5f)
                        {

                            anim.SetBool("Aim", true);
                            State = States.shooting;
                            LastAimed = timer;
                            LastShootCant = timer;
                            CanShoot = false;
                            print("set true");
                        }
                    }
                }
            }
            if (State == States.shooting) {
                int ToShoot;
                if (A != null)
                    ToShoot = 2;
                else
                    ToShoot = 0;
                if (A != null){
                    if (CanShoot){
                        if (Aim){

                            Shooting();
                        }
                    }
                    if (damaged){
                        damaged = false;
                        anim.SetBool("Aim", false);
                        ShootsCant = 0;
                        State = States.Cover;
                    }
                    if (ShootsCant > ToShoot){
                        ShootsCant = 0;
                        anim.SetBool("Aim", false);
                        State = States.Cover;
                    }
                    if (timer - LastShootCant > 2f && !CanShoot){
                        CanShoot = true;
                    }
                }
                else {
                    
                }
            }
            //si se sabe que esta apuntando, para darle realismo hay que dar unos segundos de reaccion
        }
        else {
            
        }

        IlluminateShoot();
    }

    public string EnemyTag;

    public GameObject ShootBlood;

    public AudioSource AudSrc;

    public List<AudioClip> Audios;

    int ShootsCant;
    int ShootsDCant;

    void Shooting() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 4.4f,
                     Vector2.right * transform.localScale.x, Radius, Layer);
        if (hit.collider != null) {
            if (hit.transform.tag == EnemyTag){

                hit.transform.gameObject.SendMessage("Damaged", Shootattack);
                GameObject Blood = Instantiate(ShootBlood, hit.point,
                    Quaternion.Euler(0, 0, Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg));
                Destroy(Blood, 1f);
                ShootsDCant++;
                print("LE DIO AL PLAYER!");
            }
            else {
                Destroy(hit.transform.gameObject);
                print("le dio a algo");
            }
        }
        illuminate = true;
        PlayClip(AudSrc, Audios[1]);
        ShootsCant++;
        anim.SetTrigger("Shoot");
        print("Disparado");
    }

    [System.Serializable]
    public class ShootLight
    {
        public LightSprite Light;
        [HideInInspector]
        public Color LightColor;
        public float MaxIntense { get; set; }
        [Range(0, 0.5f)]
        public float DegTime;

        public float DegVelIntense { get; private set; }

        public void LoadData()
        {
            LightColor = Light.Color;
            MaxIntense = LightColor.a;
        }

        public void Degratation()
        {
            DegVelIntense = MaxIntense / DegTime;
            if (LightColor.a > 0 + DegVelIntense * Time.deltaTime)
                LightColor.a -= DegVelIntense * Time.deltaTime;
            else
                LightColor.a = 0;
        }

        public void Illuminate()
        {
            LightColor.a = MaxIntense;
        }

        public void RefreshColor()
        {
            Light.Color = LightColor;
        }
    }

    public ShootLight ShootLight1;
    public ShootLight ShootLight2;
    bool illuminate = false;


    void IlluminateShoot()
    {
        if (illuminate){
            ShootLight1.Illuminate();
            ShootLight2.Illuminate();
            illuminate = false;
        }
        else
        {
            ShootLight1.Degratation();
            ShootLight2.Degratation();
        }
        ShootLight1.RefreshColor();
        ShootLight2.RefreshColor();
    }
    
    public void PlayClip(AudioSource A, AudioClip Clip)
    {
        A.clip = Clip;
        A.Play();
    }

    bool damaged;
    public override void Damaged(DistanceAttack DA)
    {
        damaged = true;
        StartCoroutine(Damaged());
        base.Damaged(DA);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}