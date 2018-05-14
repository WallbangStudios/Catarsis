using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Character in Any Combatable object in Gameplay
/// </summary>
public class Character : MonoBehaviour {

    /// <summary>
    /// Animator of the character(Read Only)
    /// </summary>
    //Animador del Personaje
    public Animator anim { get; private set; }

    /// <summary>
    /// Rigidbody2D of the Character(Read Only)
    /// </summary>
    //RigidBody del personaje
    public Rigidbody2D Rb2D { get; private set; }

    /// <summary>
    /// BoxCollider2D of the Character
    /// </summary>
    //BoxCollider del personaje
    public BoxCollider2D Coll { get; private set; }

    /// <summary>
    /// This is the Time Since the level Load(Read Only)
    /// </summary>
    // Es el Temporizador desde que el nivel Comenzo
    // se actualiza each frame en el update
    public static float timer { get; private set; }

    /// <summary>
    /// This is The Current Health of the Character(Read Only)
    /// </summary>
    // Cantidad de vida que tiene el personaje actualmente
    public float CurrentHealth { get; private set; }
    [SerializeField] float MaxHealth;

    bool Canchange;
    /// <summary>
    /// This is a trigger Function, that returns True only in One Frame
    /// </summary>
    /// <param name="Boolean"></param>
    /// <returns></returns>
    // Esto es un trigger, devuelve True solo en un frame
    public bool Trigger(bool Boolean) {

        if (Boolean && !Canchange) {
            Canchange = Boolean;
            return true;
        }
        if (!Boolean)
            Canchange = Boolean;
        return false;
    }

    /// <summary>
    /// In wich Direction will be Moving
    /// </summary>
    // Es la direccion hacia donde se movera el personaje 
    public Vector2 Direction { get; set; }
    /// <summary>
    /// Speed of Movement
    /// </summary>
    // La velocidad con la que se movera el personaje
    public float Speed { get; set; }
    /// <summary>
    /// (Virtual Void) The movement of the Character
    /// </summary>
    // Movimiento del personaje
    public virtual void Movement() {
        Rb2D.velocity = new Vector2(Direction.normalized.x * Speed, Rb2D.velocity.y);
        //Rb2D.MovePosition(Rb2D.position + Direction.normalized * Speed * Time.deltaTime);
    }
    /// <summary>
    /// (Virtual Void) The load method, it'll be in the Awake Event
    /// </summary>
    // Es donde se cargan todos los datos, esta el en Void Awake()
    // Es virtual para que los hijos tambien puedan agregar Datos a cargar
    public virtual void LoadData() {
        anim = GetComponent<Animator>();
        Rb2D = GetComponent<Rigidbody2D>();
        Coll = GetComponent<BoxCollider2D>();
        CurrentHealth = MaxHealth;
    }
    /// <summary>
    /// (Virtual Void) The Update method, it'll be in the Update event
    /// </summary>
    // Es donde estara el gameplay, se llamara en cada frame en el Update()
    // Es Virtual para que los hijos puedan agregar Comandos
    public virtual void UpdateThis() {
        if (PauseMenu.GameIsPaused)
            return;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        timer = Time.timeSinceLevelLoad;
    }
    /// <summary>
    /// (Virtual Void) The FixedUpdate method, it'll be in the FixedUpdate event
    /// </summary>
    // Es donde estara el gameplay, se llamara en cada FixedFrame del FixedUpdate()
    // Es Virtual para que los hijos puedan agregar Comandos
    public virtual void FixedUpdateThis() {
        if (PauseMenu.GameIsPaused)
            return;
        Movement();
    }
    /// <summary>
    /// (Virtual Void) Character gets Damaged By a Meele Attack
    /// </summary>
    /// <param name="MA"></param>
    // Es el metodo donde recibira daño Donovan con un MeeleAttack
    public virtual void Damaged(MeeleAttack MA) {
        CurrentHealth -= MA.Damage;
        if (CurrentHealth <= 0) Dead();
    }
    /// <summary>
    /// (Virtual Void) Character gets Damaged By a Firegun
    /// </summary>
    /// <param name="DA"></param>
    // Es el metodo donde recibira daño Donovan con un Disparo
    public virtual void Damaged(DistanceAttack DA) {
        CurrentHealth -= DA.Damage;
        if (CurrentHealth <= 0) Dead();
    }
    /// <summary>
    /// (Virtual Void) With this Method Donovan Will be Death
    /// </summary>
    // Es el metodo donde morirá donovan
    public virtual void Dead(){
        Destroy(transform.gameObject);
    }
    // En el Awake Solo estara El LoadData, 
    //ahi se pondra todo lo que habria en el Awake
    void Awake() {
        LoadData();
    }
    // En el Update Solo estara El UpdateThis, 
    // ahi se pondra todo lo que habria en el Update
    void Update() {
        UpdateThis();
    }
    // En el FixedUpdate Solo estara El FixedUpdateThis, 
    // ahi se pondra todo lo que habria en el FixedUpdate
    void FixedUpdate() {
        FixedUpdateThis();
    }

}
/// <summary>
/// The Ammo Of the FireGun
/// </summary>
[System.Serializable]
public class Ammo
{
    public int AmmoInMag;
    public int TotalAmmo;
}

public class Human : Character {

    //Esta Clase tiene todo lo del Sistema de Disparos
    [System.Serializable]
    public class ShootingSystem  {

        public StatesInfo.ShootingtStateInfo Shoot;
        public void LoadData() {
            JsonLoader.LoadData();
            AmmoDonovan = new Ammo();
            ShootAttack = JsonLoader.attacks.DistanceAttacks[indexGun];
            instance = Arms.transform.parent.gameObject;
            MeInThis = instance.GetComponent<Human>();
            TempField = FieldOfFire;
            Shootangle = angle;
            viewShootAngle = DirFromAngle(Shootangle, false);
            MaxIntense = ShootLight.intensity;
            ShootLight.intensity = 0;
            //PC = Donovan.GetComponent<Playercontroller>();
            AmmoDonovan.TotalAmmo = TotalAmmo;
            AmmoDonovan.AmmoInMag = ShootAttack.Magazine;
            ArmsAnim = Arms.GetComponent<Animator>();
            Shoot = MeInThis.StateInfo.Shooting;
        }

        #region Movement
        float lastpress;
        bool CanDodge;
        public float direction;
        public void WithgunMove(){

            Actions GameplayActions = MeInThis.GameplayActions;

            if (GameplayActions.DodgeAction && !Shoot.Dodge && CanDodge)
            {

                lastpress = Time.timeSinceLevelLoad;
                CanDodge = false;
                MeInThis.anim.SetTrigger("Dodge");
                if (MeInThis.Direction.x != 0) direction = MeInThis.Direction.x;
                else direction = instance.transform.localScale.x;

            }

            if (timer - lastpress > 1) CanDodge = true;

            if (Shoot.ShootingMovement)
            {

                if (MeInThis.Direction.x != 0)
                {
                    if (CanTurn() || GameplayActions.RunAction)
                        instance.transform.localScale = new Vector3(MeInThis.Direction.x, 1, 1);
                }
                if (GameplayActions.SecondaryAction)
                {
                    MeInThis.Speed = MeInThis.GameplaySpeeds.AimWalkSpeed;
                        mult = 0.5f;
                }
                else if (GameplayActions.PrimaryAction)
                {
                    MeInThis.Speed = MeInThis.GameplaySpeeds.ShootWalkSpeed;
                        mult = 0.5f;
                }
                else if (GameplayActions.RunAction && CanTurn())
                {
                    mult = 1;
                    MeInThis.Speed = MeInThis.GameplaySpeeds.RunSpeed;
                }
                else
                {
                    if (CanTurn())
                        mult = 0;
                    MeInThis.Speed = MeInThis.GameplaySpeeds.WalkSpeed;
                }
            }
            else if (Shoot.Dodge)
            {
                MeInThis.Direction = new Vector2(direction, 0);
                MeInThis.Speed = MeInThis.GameplaySpeeds.DodgeSpeed;
                instance.transform.localScale = new Vector3(-direction, 1, 1);

            }
            else if (Shoot.InCover)
            {
                
            }
            float dire;
            if (!CanTurn() && mult != 1 && MeInThis.Direction.x != 0)
                dire = Mathf.Sign(AimDirection.x) * MeInThis.Direction.x;
            else
                dire = 1;

            MeInThis.Coll.enabled = !Shoot.Dodge;
            MeInThis.anim.SetFloat("Movx", (Mathf.Abs(MeInThis.Direction.x)+ mult) * dire);
        }
        #endregion

        #region AimFunctions


        [HideInInspector]
        public DistanceAttack ShootAttack;

        public GameObject Aim;
        public GameObject Arms;
        GameObject instance;
        public Human MeInThis { get; private set; }

        [HideInInspector]
        public Vector3 AimDirection;
        public float TimeRest;
        float LastPress;

        public Ammo AmmoDonovan { get; private set; }
        public int TotalAmmo;

        public int indexGun = 0;

        public Light ShootLight;
        bool illuminate = false; 
        float MaxIntense;
        public float DegVelIntense;


        public void IlluminateShoot()
        {
            if (illuminate){
                ShootLight.intensity = MaxIntense;
                illuminate = false;
            }
            else
                if (ShootLight.intensity > 0)
                    ShootLight.intensity = ShootLight.intensity - DegVelIntense;
        }

        public LayerMask LayerShoot;

        [Range(0, 360)]
        public float angle = 270;

        [Range(0, 180)]
        public float FieldOfFire = 25f;
        public float RadiusOfFire = 2;
        public float FireRate = 15f;

        public string EnemyTag;

        float TempField;

        [HideInInspector]
        public float Shootangle;
        Vector3 viewShootAngle;
        float NextTimeToFire = 0f;

        public Animator ArmsAnim { get; private set; }

        public float mult { get; private set; }


        public Vector3 DirFromAngle(float AngleInDegrees, bool isGlobal)
        {
            if (!isGlobal)
            {
                AngleInDegrees += instance.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(AngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(AngleInDegrees * Mathf.Deg2Rad), 0);
        }

        public bool CanTurn()
        {

            if (Time.timeSinceLevelLoad < TimeRest)
                return true;

            if (MeInThis.GameplayActions.PrimaryAction || MeInThis.GameplayActions.SecondaryAction)
                LastPress = Time.timeSinceLevelLoad;

            return timer - LastPress > TimeRest;
        }

        public void GetJsonData()
        {
            ShootAttack = JsonLoader.attacks.DistanceAttacks[indexGun];
            TempField = ShootAttack.FieldOfFire;
            FireRate = ShootAttack.FireRate;
        }

        public void Aiming()
        {
            if (MeInThis.GameplayActions.SecondaryAction)
                FieldOfFire = 1;
            else
                FieldOfFire = TempField;
        }

        public void GetDirectionAndAngle(Vector3 Objective)
        {
            if (!CanTurn()){
                AimDirection = (Objective - Arms.transform.position).normalized;
                angle = Mathf.Atan2(AimDirection.x, AimDirection.y) * Mathf.Rad2Deg;
            }
        }

        public void Shooting()
        {
          
            float Aalpha = angle + FieldOfFire / 2;
            float Balpha = angle - FieldOfFire / 2;

            Vector3 ViewAngleA = DirFromAngle(Aalpha, false);
            Vector3 ViewAngleB = DirFromAngle(Balpha, false);
            Vector3 ViewAngle = DirFromAngle(angle, false);

            if (MeInThis.GameplayActions.PrimaryAction && Time.time >= NextTimeToFire 
                    && !Shoot.Dodge && AmmoDonovan.AmmoInMag > 0 && Shoot.ShootCover){

                NextTimeToFire = Time.time + 1 / FireRate;
                Shootangle = Random.Range(Aalpha, Balpha);
                viewShootAngle = DirFromAngle(Shootangle, false);
                //angletest+=ShootAttack.Recoil;
                RaycastHit2D hit = Physics2D.Raycast(Aim.transform.position, viewShootAngle, RadiusOfFire, LayerShoot.value);
                if (hit.collider != null)
                {
                    if (hit.transform.tag == EnemyTag)
                    {
                        hit.transform.gameObject.GetComponentInParent<BoxCollider2D>().SendMessage("Damaged", ShootAttack);
                    }
                    else
                    {
                        Destroy(hit.transform.gameObject);
                    }
                    
                }
                illuminate = true;
                AmmoDonovan.AmmoInMag--;
                ArmsAnim.SetTrigger("Shoot");
            }

            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngle * RadiusOfFire, Color.blue);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngleA * RadiusOfFire, Color.white);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngleB * RadiusOfFire, Color.white);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + viewShootAngle * RadiusOfFire, Color.red);
        }

        public void Reload()
        {
            if (MeInThis.GameplayActions.ReloadAction && AmmoDonovan.AmmoInMag < ShootAttack.Magazine)
            {
                int RestAmmo = Mathf.Clamp(AmmoDonovan.TotalAmmo, 0, ShootAttack.Magazine - AmmoDonovan.AmmoInMag);
                AmmoDonovan.AmmoInMag += RestAmmo;
                AmmoDonovan.TotalAmmo -= RestAmmo;
            }
        }

        public void RotateArm()
        {
            if (!CanTurn() && !Shoot.Dodge && !Shoot.TransitionCover)
            {

                instance.transform.localScale = new Vector3(Mathf.Sign(AimDirection.x), 1, 1);

                Arms.transform.localRotation = Quaternion.Euler(0, 0, -instance.transform.localScale.x * angle + 90);
            }
            else
            {
                Arms.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        #endregion




    }
    // Esta Clase tiene todo lo del Sistema de Peleas
    [System.Serializable]
    public class FightingSystem {

        public bool IsComboAnimationPlaying = false;
        collscript Collscript;
        float lastpress;
        float tab = 1;
        
        //Declaracion inicial de las barras de energia y vida
        float Energy = 100f;
        float Health = 100f;

        public BoxCollider2D AttackCollider;

        GameObject instance;
        public Human MeInThis { get; private set; }

        public StatesInfo.FightingStateInfo Fight;
        public void LoadData() {
            instance = AttackCollider.gameObject.transform.parent.transform.parent.gameObject;
            MeInThis = instance.GetComponent<Human>();
            Collscript = AttackCollider.gameObject.GetComponent<collscript>();
            Fight = MeInThis.StateInfo.Fight;
        }

        bool CanDodge;
        float direction;
        float mult;
        public void Move(){

            Actions GameplayActions = MeInThis.GameplayActions;

            if (GameplayActions.DodgeAction && !Fight.Dodge && CanDodge)
            {
                lastpress = Time.timeSinceLevelLoad;
                CanDodge = false;
                MeInThis.anim.SetTrigger("Dodge");
                if (MeInThis.Direction.x != 0) direction = MeInThis.Direction.x;
                else direction = instance.transform.localScale.x;

            }

            if (timer - lastpress > 1) CanDodge = true;

            if (!Fight.Dodge && !Fight.StAttacking0)
            {

                if (MeInThis.Direction.x != 0)
                {
                    instance.transform.localScale = new Vector3(MeInThis.Direction.x, 1, 1);
                }


                if (GameplayActions.RunAction)
                {
                    MeInThis.Speed = MeInThis.GameplaySpeeds.RunSpeed;
                    mult = 2;
                }
                else {
                    MeInThis.Speed = MeInThis.GameplaySpeeds.WalkSpeed;
                    mult = 1;
                } 


            }
            else if (Fight.Dodge)
            {
                MeInThis.Direction = new Vector2(direction, 0);
                MeInThis.Speed = MeInThis.GameplaySpeeds.DodgeSpeed;
                instance.transform.localScale = new Vector3(-direction, 1, 1);

            }
            else if (Fight.StAttacking0 && !GameplayActions.DodgeAction)
            {

                MeInThis.Direction = Vector2.zero;

            }

            MeInThis.Coll.enabled = !Fight.Dodge;
            MeInThis.anim.SetFloat("Movx", Mathf.Abs(MeInThis.Direction.x/mult));

        }

        /*GESTOR DE LAS ANIMACIONES DEL SISTEMA DE COMBATE:
            Aqui se gestionan los combos para el sistema de combate*/
        public void AnimationManager()
        {
            if (Fight.StAttacking0)
            {

                if (MeInThis.StateInfo.PlaybackTime > 0.37)
                    IsComboAnimationPlaying = false;
                else
                    IsComboAnimationPlaying = true;


                if (MeInThis.StateInfo.PlaybackTime > 0.3 && MeInThis.StateInfo.PlaybackTime < 0.75) 
                    AttackCollider.enabled = true;
                else 
                    AttackCollider.enabled = false;


            }
        }
        /*ACTIVACION DEL ATAQUE:
                Se activa el ataque y Se hace Set del trigger y la animacion del combo es true*/
        public void Attacks()
        {
            if (IsComboAnimationPlaying == false)
            {
                if (MeInThis.GameplayActions.PrimaryActionDown)
                {
                    MeInThis.anim.SetTrigger("Punch");
                    IsComboAnimationPlaying = true;
                }
            }
        }

    }
    // Todas las acciones que un humano puede tener
    [System.Serializable]
    public struct Actions {
        
        public bool PrimaryActionDown;
        
        public bool SecondaryActionDown;
        
        public bool PrimaryAction;
        
        public bool SecondaryAction;
        
        public bool RunAction;
        
        public bool DodgeAction;

        public bool ReloadAction;

        public bool QuitShootAction;

    }
    //Velocidades de los desplazamientos
    [System.Serializable]
    public class Speeds {

        public float WalkSpeed = 4f;
        
        public float RunSpeed = 12f;
        
        public float DodgeSpeed = 25f;
        
        public float AimWalkSpeed = 2f;
        
        public float ShootWalkSpeed = 4f;

    }
    //Toda la informacion de los estados de animación
    [System.Serializable]
    public struct StatesInfo
    {
        [System.Serializable]
        public class ShootingtStateInfo
        {
            #region Variables
            public bool ShootingMovement;
            public bool Dodge;
            public bool EnterCover;
            public bool OnCover;
            public bool AimCover;
            public bool ShootCover;
            public bool ExitCover;
            public bool TransitionCover;
            public bool InCover;
            public bool JumpingCover;
            #endregion

            public void GetStatesInfo(AnimatorStateInfo StateInfo)
            {
                ShootingMovement = StateInfo.IsName("ShootingMovement");

                Dodge = StateInfo.IsName("DodgeS");

                EnterCover = StateInfo.IsName("EnterCoverS");
                OnCover = StateInfo.IsName("OnCoverS");
                AimCover = StateInfo.IsName("AimCoverS");
                ShootCover = StateInfo.IsName("ShootCoverS");
                ExitCover = StateInfo.IsName("ExitCoverS");

                TransitionCover = AimCover || EnterCover;
                InCover = TransitionCover || ShootCover || OnCover;
            }
        }
        [System.Serializable]
        public class FightingStateInfo
        {
            /// <summary>
            /// If its Dodging, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es dodge
            public bool Dodge;
            /// <summary>
            /// If its Attacking, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es Attacking(Cualquiera de los dos brazos)
            public bool StAttacking0;
            /// <summary>
            /// If its attacking with de Right arm, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es Attacking con la derecha
            public bool StAttacking1;
            /// <summary>
            /// If its attacking with de Left arm, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es Attacking con la izquierda
            public bool StAttacking2;

            public void GetStatesInfo(AnimatorStateInfo StateInfo, Human H)
            {
                Dodge = StateInfo.IsName("Dodge");

                StAttacking0 = StateInfo.IsName("Punch1") || StateInfo.IsName("Punch2");
                StAttacking1 = StateInfo.IsName("Punch1");
                StAttacking2 = StateInfo.IsName("Punch2");
                if (StateInfo.IsName("Battlemove"))
                    H.FightingObject.IsComboAnimationPlaying = false;
            }
        }
        [System.Serializable]
        public class StandardStateInfo
        {
            public bool Movement;
            public bool Jump;
            public bool Fall;

            public void GetStatesInfo(AnimatorStateInfo StateInfo)
            {
                Movement = StateInfo.IsName("Movement");
                Jump = StateInfo.IsName("Jump");
                Fall = StateInfo.IsName("Fall");
            }

        }

        /// <summary>
        /// The StateInfo Of the Animator of The Character(Read Only)
        /// </summary>
        // Recopila Toda la informacion de los States del animator
        public AnimatorStateInfo StateInfo;

        public ShootingtStateInfo Shooting;
        public FightingStateInfo Fight;
        public StandardStateInfo Standard;

        /// <summary>
        /// The time Playing Of the State(Read Only)
        /// </summary>
        // Tiempo que se esta reproduciendo cada animacion
        public float PlaybackTime;
        /// <summary>
        /// The Recopilation of The States and Bool Method
        /// </summary>
        // En este metodo se recopilan todos los states Info 
        public void GetStatesInfo(Human H)
        {
            PlaybackTime = StateInfo.normalizedTime;

            Shooting.GetStatesInfo(StateInfo);
            Fight.GetStatesInfo(StateInfo, H);
            Standard.GetStatesInfo(StateInfo);

            H.ShootingObject.Shoot = Shooting;
            H.FightingObject.Fight = Fight;
        }

        public StatesInfo(AnimatorStateInfo Stateinf) {
            Shooting = new ShootingtStateInfo();
            Fight = new FightingStateInfo();
            Standard = new StandardStateInfo();

            PlaybackTime = 0;
            //recopilar la informacion de los estados de los personajes
            //IMPORTANTE para la gestion de animacion y son bastante precisos para las transiciones y demas
            StateInfo = Stateinf;
        }
    }

    public StatesInfo StateInfo { get; private set; }

    /// <summary>
    /// If its with Gun, return True
    /// </summary>
    // Variable Bool que sera verdadera si tiene un arma
    public bool WithGun;

    /// <summary>
    /// This Is the Actions a human can do
    /// </summary>
    // Instanciacion de la Clase Actions
    [HideInInspector]
    public Actions GameplayActions;

    /// <summary>
    /// All the Speeds of the Movement Gameplay
    /// </summary>
    [Tooltip("All the Speeds of the Movement Gameplay")]
    public Speeds GameplaySpeeds;

    bool CanShowup() {
        if (WithGun && ShootingObject.mult != 1){
            if (!ShootingObject.CanTurn()) {
                if (StateInfo.Shooting.ShootingMovement || StateInfo.Shooting.OnCover)
                    return true;
            }
        }
          return false;
    }

    /// <summary>
    /// The Manager Of The Arms if its WithGun Or not
    /// </summary>
    /// <param name="Objective">The Objective will be Pointing</param>
    public void ArmsManager(Vector3 Objective) {
        if (!WithGun)
        {
            FightingObject.Move();
            FightingObject.AnimationManager();
            FightingObject.Attacks();
        }
        else {
            ShootingObject.GetDirectionAndAngle(Objective);
            ShootingObject.WithgunMove();
            ShootingObject.RotateArm();
            ShootingObject.Aiming();
            ShootingObject.Shooting();
            ShootingObject.Reload();
        }

        ShootingObject.IlluminateShoot();
        if (GameplayActions.QuitShootAction) WithGun = !WithGun;
        ShootingObject.ArmsAnim.SetBool("CanShowup",CanShowup());
    }

    /// <summary>
    /// These are all the variables of Shooting System
    /// </summary>
    [Tooltip("These are all the variables of Shooting System")]
    public ShootingSystem ShootingObject;
    /// <summary>
    /// These are all the variables of Fighting System
    /// </summary>
    [Tooltip("These are all the variables of Fighting System")]
    public FightingSystem FightingObject;
    /* Los overrides funcionan como un agregado a los Metodos virtual y Abstract
       seria tener la base e ir sumando con el override en el hijo
       Seria: 
    Objeto Padre{ 
        metodo Virtual(){
        //BASE 
        }
    }
    Objeto Hijo:Padre{
        Override Metodo Virtual(){
            Base.Virtual();
            //Agregado
        }
    }
    
     * Si instanciamos la clase Hijo y llamamos Su metodo Virtual() 
     * Tendriamos una rutina de Base + Agregado
    */
    public override void LoadData() {
        base.LoadData();
        ShootingObject.LoadData();
        FightingObject.LoadData();
        
    }

    public override void UpdateThis(){
        base.UpdateThis();
        StateInfo = new StatesInfo(anim.GetCurrentAnimatorStateInfo(0));
        StateInfo.GetStatesInfo(this);
        ShootingObject.GetJsonData();
    }
}
