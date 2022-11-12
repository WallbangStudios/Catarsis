using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Light2D;

[System.Serializable]
public class Impact {
    public float ImpulseMagnitude;
    public float accelerationGrounded, accelerationAirbone;

    public Impact() {
        ImpulseMagnitude = 0;
        accelerationAirbone = 0;
        accelerationGrounded = 0;
    }

    public Impact(float impulsemagnitude, float accGrounded = 0, float accAirbone = 0) {
        ImpulseMagnitude = impulsemagnitude;
        accelerationGrounded = accGrounded;
        accelerationAirbone = accAirbone;
    }
}

/// <summary>
/// Character in Any Combatable object in Gameplay
/// </summary>
[RequireComponent(typeof(Controller2D))]
public class Character : MonoBehaviour {

    /// <summary>
    /// Animator of the character(Read Only)e
    /// </summary>
    //Animador del Personaje
    public Animator anim { get; private set; }

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float DesacelerationTimeAirbone = .2f;
    public float DesacelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    float gravity;
    float jumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    [HideInInspector]
    public Controller2D controller;

    public bool SmoothMovement;

    /// <summary>
    /// BoxCollider2D of the Character
    /// </summary>
    //BoxCollider del personaje
    public BoxCollider2D AttackReciever;
    public BoxCollider2D ShootReciever;

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
    public int CurrentHealth;
    public int MaxHealth;

    //Variable auxiliar para la Funcion Trigger
    /// <summary>
    /// This is a trigger Function, that returns True only in One Frame
    /// </summary>
    /// <param name="Boolean"></param>
    /// <returns></returns>
    // Esto es un trigger, devuelve True solo en un frame
    public static bool Trigger(bool Boolean, ref bool Canchange)
    {
        
        //Si es true y es el primer frame
        if (Boolean && !Canchange) {
            //El Auxiliar sera true para que esta condicional no se cumpla
            Canchange = Boolean;
            return true;
        }
        //Si se deja de ser true y pasa a false, entonces se reinicia todo
        if (!Boolean && Canchange)
            Canchange = Boolean;
        //Si la condicional principal no se cumple, Siempre arrojara falso
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

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
            velocity.y = jumpVelocity;
    }
    
    //metodo de impulso para el empuje
    public void Impulse(Impact Parameter)
    {
        //activar el suavizado del empuje
        SmoothMovement = true;
        //hacer set de las aceleraciones si se quiere hacer
        if (Parameter.accelerationGrounded != 0)
            DesacelerationTimeGrounded = Parameter.accelerationGrounded;
        if (Parameter.accelerationAirbone != 0)
            DesacelerationTimeAirbone = Parameter.accelerationAirbone;
        //impulsar
        velocity.x = Parameter.ImpulseMagnitude;
        //desactivar cuando llegue a 0 o cerca
        StartCoroutine(DeactivateSmooth());
    }

    IEnumerator DeactivateSmooth()
    {
        bool analize = true;
        float time = 0;
        while (analize)
        {
            if (time > ((controller.collisions.below) ? DesacelerationTimeGrounded : DesacelerationTimeAirbone))
            {
                analize = false;
            }
            yield return null;
            time += Time.deltaTime;
        }
        SmoothMovement = false;
    }

    /// <summary>
    /// (Virtual Void) The movement of the Character
    /// </summary>
    // Movimiento del personaje
    public virtual void Movement() {
        //Movimiento rectilineo uniforme horizontal
        float targetvelocityX = Direction.x * Speed;
        if (SmoothMovement)
            velocity.x = Mathf.SmoothDamp(velocity.x, targetvelocityX, ref velocityXSmoothing, (controller.collisions.below) ? DesacelerationTimeGrounded : DesacelerationTimeAirbone);
        else
            velocity.x = targetvelocityX;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        //Rb2D.velocity = new Vector2(Direction.normalized.x * Speed, Rb2D.velocity.y);
        //Rb2D.MovePosition(Rb2D.position + Direction.normalized * Speed * Time.deltaTime);
    }
    /// <summary>
    /// (Virtual Void) The load method, it'll be in the Awake Event
    /// </summary>
    // Es donde se cargan todos los datos, esta el en Void Awake()
    // Es virtual para que los hijos tambien puedan agregar Datos a cargar
    public virtual void LoadData() {
        anim = GetComponent<Animator>();

        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        if (jumpHeight == 0 && timeToJumpApex == 0) {
            gravity = 0;
            jumpVelocity = 0;
        }

        //Rb2D = GetComponent<Rigidbody2D>();
        //Al principio del juego, la vida estara al 100%
        CurrentHealth = MaxHealth;
        //Cargando los Shader para el efecto de ponerse de color blanco
        textShader = Shader.Find("GUI/Text Shader");
        Default = Shader.Find("Sprites/Default");
    }
    /// <summary>
    /// (Virtual Void) The Update method, it'll be in the Update event
    /// </summary>
    // Es donde estara el gameplay, se llamara en cada frame en el Update()
    // Es Virtual para que los hijos puedan agregar Comandos
    public virtual void UpdateThis() {
        //Si se pausa, no habra ningun Gameplay
        if (PauseMenu.GameIsPaused)
            return;
        //Ajustar la vida para que no sobrepase los limites del 0% a 100%
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        //Variable auxiliar que se utiliza en los Timers de Cooldown
        timer = Time.timeSinceLevelLoad;
    }
    /// <summary>
    /// (Virtual Void) The FixedUpdate method, it'll be in the FixedUpdate event
    /// </summary>
    // Es donde estara el gameplay, se llamara en cada FixedFrame del FixedUpdate()
    // Es Virtual para que los hijos puedan agregar Comandos
    public virtual void FixedUpdateThis() {
        //Si se pausa, no habra ningun gameplay
        if (PauseMenu.GameIsPaused)
            return;

        
        //Movimiento basico
        Movement();
    }
    /// <summary>
    /// (Virtual Void) Character gets Damaged By a Meele Attack
    /// </summary>
    /// <param name="MA"></param>
    // Es el metodo donde recibira daño Donovan de un MeeleAttack(Ataque Cuerpo a Cuerpo)
    public virtual void Damaged(MeeleAttack MA) {
        //se resta la vida
        CurrentHealth -= MA.Damage;
        //Si llega a 0, morira
        if (CurrentHealth <= 0) {
            Dead();
            return;
        }
    }
    /// <summary>
    /// (Virtual Void) Character gets Damaged By a Firegun
    /// </summary>
    /// <param name="DA"></param>
    // Es el metodo donde recibira daño Donovan de un Disparo
    public virtual void Damaged(DistanceAttack DA) {
        //se resta la vida
        CurrentHealth -= DA.Damage;
        //Si llega a 0, morira
        if (CurrentHealth <= 0){
            Dead();
            return;
        }
    }

    public List<SpriteRenderer> Sprites;

    protected Shader textShader;
    protected Shader Default;

    public float Duration;

    public IEnumerator Damaged()
    {
        for (int i = 0; i < Sprites.Count; i++) {
            SpriteRenderer SP = Sprites[i];
            SP.material.shader = textShader;
            SP.color = Color.white;

        }
        yield return new WaitForSeconds(Duration);
        for (int i = 0; i < Sprites.Count; i++)
        {
            SpriteRenderer SP = Sprites[i];
            SP.material.shader = Default;
            SP.color = Color.white;

        }
    }
    /// <summary>
    /// (Virtual Void) With this Method Donovan Will be Death
    /// </summary>
    // Es el metodo donde morirá donovan
    public virtual void Dead(){
        //De forma predeterminada, si el objeto muere, se destruye
        Destroy(transform.gameObject);
        //Se puede sobreescribir con un overide
    }
    // En el Awake Solo estara El LoadData, 
    //ahi se pondra todo lo que habria en el Awake
    void Awake() {
        //se cargan los datos
        LoadData();
    }
    // En el Update Solo estara El UpdateThis, 
    // ahi se pondra todo lo que habria en el Update
    void Update() {
        // si esta pausado, entonces no habra ningun gameplay
        if (PauseMenu.GameIsPaused)
            return;
        UpdateThis();
    }
    // En el FixedUpdate Solo estara El FixedUpdateThis, 
    // ahi se pondra todo lo que habria en el FixedUpdate
    void FixedUpdate() {
        FixedUpdateThis();
    }

    public virtual void HasAttacked() {
    
    }
}
/// <summary>
/// The Ammo Of the FireGun
/// </summary>
//La municnion que hay en un arma
[System.Serializable]
public class Ammo{
    //Municion disponible que hay en una cacerina
    public int AmmoInMag;
    //Municion total disponible
    public int TotalAmmo;
}

public class Human : Character {

    //Esta Clase tiene todo lo del Sistema de Disparos
    [System.Serializable]
    public class ShootingSystem  {
        [HideInInspector]
        public StatesInfo.ShootingtStateInfo Shoot;

        Attacks attacks;
        public void LoadData() {
            //Cargar el Json de los datos
            attacks = JsonLoader<Attacks>.LoadData("Atacks");
            
            //Instanciar la municion en el Characer
            AmmoDonovan = new Ammo();
            AmmoDonovan.TotalAmmo = TotalAmmo;
            AmmoDonovan.AmmoInMag = ShootAttack.Magazine;

            ShootAttack = attacks.DistanceAttacks[indexGun];

            ArmsAnim = Arms.GetComponent<Animator>();
            instance = Arms.transform.parent.gameObject;

            MeInThis = instance.GetComponent<Human>();
            //Campo de tiro temporal
            TempField = FieldOfFire;
            //angulo de tiro temporal
            Shootangle = angle;
            //tangente de angulo de tiro temporal
            viewShootAngle = DirFromAngle(Shootangle, false);
            //PC = Donovan.GetComponent<Playercontroller>();
            
            Shoot = MeInThis.StateInfo.Shooting;

            ShootLight1.LoadData();
            ShootLight2.LoadData();

            ArmsSource = Arms.GetComponent<AudioSource>();
        }

        #region Movement
        //Cooldown
        float lastpress;
        //Cooldown del Dodge
        bool CanDodge;

        public float direction;
        public void WithgunMove(){

            Actions GameplayActions = MeInThis.GameplayActions;
            bool CT = CanTurn();
            CanDodge = MeInThis.Candodge(GameplayActions.DodgeAction);

            if (GameplayActions.DodgeAction && !(Shoot.Dodge || Shoot.Damaged || Shoot.Reloading || !MeInThis.controller.collisions.below) && CanDodge
                && !ArmsAnim.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {

                lastpress = Time.timeSinceLevelLoad;
                MeInThis.anim.SetTrigger("Dodge");
                if (MeInThis.Direction.x != 0) direction = MeInThis.Direction.x;
                else direction = instance.transform.localScale.x;

            }

            if (GameplayActions.ReloadAction && MeInThis.ShootingObject.CanReload() && MeInThis.controller.collisions.below 
                && !ArmsAnim.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                MeInThis.anim.SetTrigger("Reload");
            }

            if (Shoot.ShootingMovement)
            {
                if (MeInThis.Direction.x != 0)
                {
                    if (CT || GameplayActions.RunAction)
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
                else if (GameplayActions.RunAction && CT && MeInThis.Direction.x != 0)
                {
                    mult = 1;
                    MeInThis.Speed = MeInThis.GameplaySpeeds.RunSpeed;
                }
                else
                {
                    if (CT)
                        mult = 0;
                    if (!GameplayActions.RunAction)
                        MeInThis.Speed = MeInThis.GameplaySpeeds.WalkSpeed;
                }
            }
            if (Shoot.Dodge)
            {
                MeInThis.Direction = new Vector2(direction, 0);
                MeInThis.Speed = MeInThis.GameplaySpeeds.DodgeSpeed;
                instance.transform.localScale = new Vector3(direction, 1, 1);

            }
            else if (Shoot.TransitionCover || Shoot.OnCover || Shoot.JumpingCover)
            {
                mult = 0;
                //print(MeInThis.Tempdirection);
                if (MeInThis.Tempdirection != 0)
                    MeInThis.transform.localScale = new Vector3(MeInThis.Tempdirection, 1, 1);
                if (Shoot.TransitionCover)
                    MeInThis.Direction = Vector2.zero;
            }
            if (Shoot.Damaged || Shoot.Reloading) {
                MeInThis.Speed = 0;
            }
            float dire;
            if (!CT && mult != 1 && MeInThis.Direction.x != 0)
                dire = Mathf.Sign(AimDirection.x) * MeInThis.Direction.x;
            else
                dire = 1;

            MeInThis.AttackReciever.enabled = !Shoot.Dodge;
            MeInThis.anim.SetFloat("Movx", (Mathf.Abs(MeInThis.Direction.x)+ mult) * dire);
            //print("(|" + MeInThis.Direction.x + "| + " + mult + ") * " + dire + " = " + 
              //  (Mathf.Abs(MeInThis.Direction.x) + mult) * dire);
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

        public List<AudioClip> ShootingClips;

        public GameObject ShootBlood;

        public AudioSource ArmsSource { get; private set;}

        [System.Serializable]
        public class ShootLight{
            public LightSprite Light;
            [HideInInspector]
            public Color LightColor;
            public float MaxIntense { get; set; }
            [Range(0,0.5f)]
            public float DegTime;

            public float DegVelIntense{ get; private set; }

            public void LoadData(){
                LightColor = Light.Color;
                MaxIntense = LightColor.a;
            }

            public void Degratation(){
                DegVelIntense = MaxIntense / DegTime;
                if(LightColor.a > 0 + DegVelIntense * Time.deltaTime)
                    LightColor.a -= DegVelIntense * Time.deltaTime;
                else
                    LightColor.a = 0;
            }

            public void Illuminate(){
                LightColor.a = MaxIntense;
            }

            public void RefreshColor(){
                Light.Color = LightColor;
            }
        }

        public ShootLight ShootLight1;
        public ShootLight ShootLight2;
        bool illuminate = false;


        public void IlluminateShoot()
        {
            if (illuminate){
                ShootLight1.Illuminate();
                ShootLight2.Illuminate();
                illuminate = false;
            }
            else{
                ShootLight1.Degratation();
                ShootLight2.Degratation();
            }
            ShootLight1.RefreshColor();
            ShootLight2.RefreshColor();
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

            if ((MeInThis.GameplayActions.PrimaryAction || MeInThis.GameplayActions.SecondaryAction) && !Shoot.Dodge && !Shoot.Reloading && MeInThis.controller.collisions.below)
                LastPress = Time.timeSinceLevelLoad;

            if (MeInThis.GameplayActions.DodgeAction && CanDodge) {
                LastPress = 0;
            }


            return timer - LastPress > TimeRest;
        }

        public void GetJsonData()
        {
            ShootAttack = attacks.DistanceAttacks[indexGun];
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
                    && !(Shoot.Dodge || Shoot.Reloading || Shoot.Damaged) && MeInThis.controller.collisions.below && AmmoDonovan.AmmoInMag > 0){

                NextTimeToFire = Time.time + 1 / FireRate;
                Shootangle = Random.Range(Aalpha, Balpha);
                viewShootAngle = DirFromAngle(Shootangle, false);
                //angletest+=ShootAttack.Recoil;
                RaycastHit2D hit = Physics2D.Raycast(Aim.transform.position, viewShootAngle, RadiusOfFire, LayerShoot.value);
                if (hit.collider != null)
                {
                    if (hit.transform.tag == EnemyTag)
                    {
                        hit.transform.gameObject.SendMessage("Damaged", ShootAttack);
                        Vector2 L = hit.transform.localPosition;
                        GameObject Blood = Instantiate(ShootBlood, hit.point, Quaternion.Euler(0, 0, Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg));
                        Destroy(Blood, 1f);
                    }
                    
                }
                illuminate = true;
                MeInThis.PlayClip(ArmsSource, ShootingClips[1]);
                AmmoDonovan.AmmoInMag--;
                ArmsAnim.SetTrigger("Shoot");
                Camera.main.GetComponent<FollowTarget>().GetDamage(0.2f, 0.3f, 0.01f, 35 * -Mathf.Sign(AimDirection.x));
            }

            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngle * RadiusOfFire, Color.blue);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngleA * RadiusOfFire, Color.white);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + ViewAngleB * RadiusOfFire, Color.white);
            Debug.DrawLine(Aim.transform.position, Aim.transform.position + viewShootAngle * RadiusOfFire, Color.red);
        }

        public void Reload()
        {
            if(MeInThis.GameplayActions.SecondaryActionDown && !(Shoot.Reloading || Shoot.Dodge || Shoot.Damaged || MeInThis.controller.collisions.below))
                MeInThis.PlayClip(ArmsSource, ShootingClips[Random.Range(2,4)]);
        }

        public bool CanReload() {
            return AmmoDonovan.AmmoInMag < ShootAttack.Magazine && !Shoot.Reloading && AmmoDonovan.TotalAmmo != 0;
        }

        public void ReloadAmmo() {
            int RestAmmo = Mathf.Clamp(AmmoDonovan.TotalAmmo, 0, ShootAttack.Magazine - AmmoDonovan.AmmoInMag);
            AmmoDonovan.AmmoInMag += RestAmmo;
            AmmoDonovan.TotalAmmo -= RestAmmo;
        }

        public void RotateArm()
        {
            if (!CanTurn() && !(Shoot.Dodge || Shoot.Reloading || Shoot.Damaged))
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

        [HideInInspector]
        public StatesInfo.FightingStateInfo Fight;
        public void LoadData() {
            instance = AttackCollider.transform.parent.gameObject;
            MeInThis = instance.GetComponent<Human>();
            Collscript = AttackCollider.gameObject.GetComponent<collscript>();
            Fight = MeInThis.StateInfo.Fight;
        }

        bool CanDodge;
        float direction;
        float mult;
        public void Move(){

            Actions GameplayActions = MeInThis.GameplayActions;

            bool CD = MeInThis.Candodge(GameplayActions.DodgeAction);
            if (GameplayActions.DodgeAction && !(Fight.Dodge || Fight.Damaged || !MeInThis.controller.collisions.below) && CD){

                lastpress = Time.timeSinceLevelLoad;
                CanDodge = false;
                MeInThis.anim.SetTrigger("Dodge");
                if (MeInThis.Direction.x != 0) direction = MeInThis.Direction.x;
                else direction = instance.transform.localScale.x;

            }

            if (Fight.BattleMove)
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
                instance.transform.localScale = new Vector3(direction, 1, 1);

            }
            else if (Fight.Attacking && !GameplayActions.DodgeAction)
            {

                MeInThis.Direction = Vector2.zero;

            }
            else if (Fight.ExitCover || Fight.OnCover || Fight.EnterCover)
            {
                mult = 0;
                print(MeInThis.Tempdirection);
                if (MeInThis.Tempdirection != 0)
                    MeInThis.transform.localScale = new Vector3(MeInThis.Tempdirection, 1, 1);
                if (Fight.EnterCover || Fight.ExitCover)
                    MeInThis.Direction = Vector2.zero;
            }
            else if (Fight.Damaged) {
                MeInThis.Speed = 0;
                print(true);
            }

            MeInThis.AttackReciever.enabled = !Fight.Dodge;
            MeInThis.anim.SetFloat("Movx", Mathf.Abs(MeInThis.Direction.x/mult));

        }

        /*GESTOR DE LAS ANIMACIONES DEL SISTEMA DE COMBATE:
            Aqui se gestionan los combos para el sistema de combate*/
        [System.Serializable]
        public struct AttackCombo{
            [Range(0,1)]
            public float FirstAttack;
            [Range(0,1)]
            public float SecondAttack;
        }
        public AttackCombo AttackComboActiveTime;
        public void AnimationManager()
        {
            if(Fight.BattleMove){
                if (MeInThis.GameplayActions.PrimaryActionDown){
                    Collscript.indexAttack = 0;
                    MeInThis.anim.SetTrigger("Punch");
                }
            }
            if (Fight.Attacking)
            {
                if (Fight.LeftPunch){

                    if (MeInThis.StateInfo.PlaybackTime > AttackComboActiveTime.FirstAttack && MeInThis.StateInfo.PlaybackTime < 0.89)
                        if (MeInThis.GameplayActions.PrimaryActionDown){
                            Collscript.indexAttack = 1;
                            MeInThis.anim.SetTrigger("Punch2");
                        }
                        //IsComboAnimationPlaying = false;
                    //else
                        //IsComboAnimationPlaying = true;
                }
                else if (Fight.RightPunch){
                    if (MeInThis.StateInfo.PlaybackTime > AttackComboActiveTime.SecondAttack && MeInThis.StateInfo.PlaybackTime < 0.89)
                        if (MeInThis.GameplayActions.PrimaryActionDown){
                            Collscript.indexAttack = 2;
                            MeInThis.anim.SetTrigger("Punch3");
                        }
                        //IsComboAnimationPlaying = false;

                    //else
                        //IsComboAnimationPlaying = true;
                }
                else if (Fight.LeftUppercut)
                {

                }
                else {
                    //IsComboAnimationPlaying = true;
                }
            }
        }
        /*ACTIVACION DEL ATAQUE:
                Se activa el ataque y Se hace Set del trigger y la animacion del combo es true*/
        public void Attacks()
        {
            if(false) //(IsComboAnimationPlaying == false)
            {
                if (MeInThis.GameplayActions.PrimaryActionDown)
                {
                    MeInThis.anim.SetTrigger("Punch");
                    print("Punch");
                    IsComboAnimationPlaying = true;
                }
            }
        }

    }
    [SerializeField]
    float DodgeCooldown;

    float LastPressDodge; 

    public bool Candodge(bool TriggerDodge){


        if(timer - LastPressDodge < DodgeCooldown)
            return false;

        if (TriggerDodge)
            LastPressDodge = timer;
        return true;
        /*
        if(WithGun){

            if(StateInfo.Shooting.ShootingMovement)
                return true;
            else
                return false;

        }
        else{

            if(StateInfo.Fight.BattleMove || StateInfo.Fight.Attacking)
                return true;
            else
                return false;

        }*/

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
    public class StatesInfo
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
            public bool Jump;
            public bool Fall;
            public bool InTheAir;
            public bool Damaged;
            public bool Reloading;
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
                JumpingCover = StateInfo.IsName("ExitCoverF");

                Jump = StateInfo.IsName("JumpS");
                Fall = StateInfo.IsName("FallS");

                Damaged = StateInfo.IsName("DamagedS");
                Reloading = StateInfo.IsName("Recharge");

                InTheAir = Jump || Fall;

                TransitionCover = AimCover || EnterCover || JumpingCover;
                InCover = TransitionCover || ShootCover || OnCover;
            }
        }
        [System.Serializable]
        public class FightingStateInfo
        {
            #region Variables
            /// <summary>
            /// If its Dodging, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es dodge
            public bool Dodge;
            /// <summary>
            /// If its Attacking, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es Attacking(Cualquiera de los dos brazos)
            public bool Attacking;
            /// <summary>
            /// If its Punching with de Right arm, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es un golpe con la derecha
            public bool RightPunch;
            /// <summary>
            /// If its Punching with de Right arm, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es un golpe con la Izquierda
            public bool LeftPunch;
            /// <summary>
            /// If its an Uppercut with de Left arm, return True(Read Only)
            /// </summary>
            // Variable Bool que sera verdadera si es un Uppercut con la izquieda
            public bool LeftUppercut;

            public bool BattleMove;

            public bool EnterCover;
            public bool OnCover;
            public bool ExitCover;

            public bool InCover;
            
            public bool Jump;
            public bool Fall;
            public bool InTheAir;

            public bool Damaged;
            #endregion

            public void GetStatesInfo(AnimatorStateInfo StateInfo, Human H)
            {
                Dodge = StateInfo.IsName("DodgeF");

                EnterCover = StateInfo.IsName("EnterCoverF");
                OnCover = StateInfo.IsName("OnCoverF");
                ExitCover = StateInfo.IsName("ExitCoverF");

                InCover = EnterCover || OnCover || ExitCover;

                Jump = StateInfo.IsName("Jump");
                Fall = StateInfo.IsName("Fall");

                InTheAir = Jump || Fall;

                Damaged = StateInfo.IsName("DamagedF");

                RightPunch = StateInfo.IsName("RightPunchF");
                LeftPunch = StateInfo.IsName("LeftPunchF");
                LeftUppercut = StateInfo.IsName("LeftUppercutF");
                Attacking = RightPunch || LeftPunch || LeftUppercut;
                BattleMove = StateInfo.IsName("Battlemove");
                if (BattleMove)
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
        public void GetStatesInfo(Human H, AnimatorStateInfo Stateinf)
        {
            //recopilar la informacion de los estados de los personajes
            //IMPORTANTE para la gestion de animacion y son bastante precisos para las transiciones y demas
            StateInfo = Stateinf;
            PlaybackTime = StateInfo.normalizedTime;

            Shooting.GetStatesInfo(StateInfo);
            Fight.GetStatesInfo(StateInfo, H);
            Standard.GetStatesInfo(StateInfo);

            H.ShootingObject.Shoot = Shooting;
            H.FightingObject.Fight = Fight;
        }

        public StatesInfo() {
            Shooting = new ShootingtStateInfo();
            Fight = new FightingStateInfo();
            Standard = new StandardStateInfo();

            PlaybackTime = 0;
            
        }
    }

    public StatesInfo StateInfo { get; private set; }

    [HideInInspector]
    public float Tempdirection;

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
                if (StateInfo.Shooting.ShootingMovement || StateInfo.Shooting.ShootCover)
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

        ShootingObject.ArmsAnim.SetBool("CanShowup",CanShowup());
        //print(CanShowup());
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
        StateInfo = new StatesInfo();
        base.LoadData();
        ShootingObject.LoadData();
        FightingObject.LoadData();
        LastPressDodge = -DodgeCooldown;        
    }

    public override void UpdateThis(){
        base.UpdateThis();

        StateInfo.GetStatesInfo(this, anim.GetCurrentAnimatorStateInfo(0));
        ShootingObject.GetJsonData();
    }

    public override void HasAttacked(){
        FightingObject.IsComboAnimationPlaying = false;
    }

    public Vector2 RepositionVar = new Vector2(2, 3.8f);
    public void Reposition() {
        transform.position = transform.position + new Vector3(RepositionVar.x * transform.localScale.x, RepositionVar.y);
    }

    public void PlayClip(AudioSource A, AudioClip Clip){
        A.clip = Clip;
        A.Play();
    }

    public List<AudioClip> Punchclips;

    public void PlayPunchClip(int index){
        AudioSource A = GetComponent<AudioSource>();
        A.clip = Punchclips[index];
        A.Play();
    }

}
