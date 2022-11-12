using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using EZCameraShake;

public class DonovanController : Human {
    //Poder del salto
    public float JumpPower;
    //Variable si esta tocando el suelo o no
    public bool Grounded;
    //Si esta tocando un Cover o no
    public bool OnCover;

    //Layer para la deteccion de los enemigos
    public LayerMask Layer;

    //Velocidad del salto del impulso horizontal caminando
    public float WalkingJumpSpeed;
    //Velocidad del salto del impulso horizontal corriendo
    public float RunJumpSpeed;

    //Si esta saltando(trigger)
    bool jump;
    //Variables auxiliares para mantener la misma direccion y velocidad en el aire
    Vector2 tempdirection;
    float tempspeed;
    //Si esta en Acccion o no
    public bool action;

    [HideInInspector]
    public bool Canjump = true;

    public Vector3 DistanceToCenter { get; private set; }

    public Transform target { get; private set; }

    public float Magnitude;

    [System.Serializable]
    public class GetDamageClass {
        public float ShakeDuration;
        public float ShakeMagnitude;
        public float ForceDuration;
        public float ForceMagnitude;
    }

    public GetDamageClass NormalPunch;
    public GetDamageClass LeftUppercut;

    public override void LoadData()
    {
        base.LoadData();
        Canjump = true;
        GameObject T = new GameObject("Target");
        target = T.transform;
    }

    public override void UpdateThis(){
        //BASE
        base.UpdateThis();
        //Analizando todas las animaciones de donovan
        #region Todas las acciones de Donovan
            GameplayActions.PrimaryAction = Input.GetButton("Fire1");
            GameplayActions.PrimaryActionDown = Input.GetButtonDown("Fire1");
            GameplayActions.SecondaryAction = Input.GetButton("Fire2");
            GameplayActions.SecondaryActionDown = Input.GetButtonDown("Fire2");
            GameplayActions.RunAction = Input.GetButton("Run");
            GameplayActions.DodgeAction = Input.GetButtonDown("Dodge");
            GameplayActions.ReloadAction = Input.GetButtonDown("Reload");
            GameplayActions.QuitShootAction = Input.GetKeyDown(KeyCode.Q);
        #endregion

        bool Dying = StateInfo.StateInfo.IsName("Death") || StateInfo.StateInfo.IsName("Died");
        if (Dying) {
            ShootingObject.ArmsAnim.SetBool("CanShowup", false);
            return;
        }


        if (GameplayActions.PrimaryActionDown) {
            //Camera.main.GetComponent<FollowTarget>().GetDamage(GetDamage.ShakeDuration, GetDamage.ShakeMagnitude,
              //                                          GetDamage.ForceDuration, GetDamage.ForceMagnitude);
        }
        //Direccion que se movera Donovan
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        Vector3 Center = new Vector2(Screen.width / 2, Screen.height / 2);
        float InputRadius = Center.x * 0.75f;
        float clampy = (Center.y * 0.75f) / InputRadius;

        float ScreenJX = Mathf.Clamp((Input.mousePosition.x - Center.x) / InputRadius, -1, 1);
        float ScreenJY = Mathf.Clamp((Input.mousePosition.y - Center.y) / (Center.y / 2), -clampy, clampy);
        //Posicion del mouse en la pantalla
        DistanceToCenter = new Vector3(ScreenJX, ScreenJY);
        target.position = transform.position + new Vector3(0, 2.49f) + DistanceToCenter * Magnitude;
        FollowTarget FT = Camera.main.GetComponent<FollowTarget>();
        if (GameplayActions.SecondaryAction && (Mathf.Abs(ScreenJX) == 1 || Mathf.Abs(ScreenJY) == clampy))
        {
            FT.target = target;
        }
        else {
            FT.target = transform;
        }

        //lo del GameManagerDemo ES SOLO PARA LA DEMO, DESPUES LO REGRESAMOS A COMO ESTABA ANTES
        if (GameplayActions.QuitShootAction && GameManagerDemo.HasPistol) WithGun = !WithGun;

        //Metodo donde se Gestiona el sistema
        RaycastHit2D Hit = Physics2D.CircleCast(transform.position, 27, Vector2.zero, 0, Layer);
        Direction = direction;
        //action = Hit.collider != null;
        //Si esta en accion, estara o combatiendo o disparando
        if (action){
            ArmsManager(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            ShootingObject.ArmsAnim.SetBool("Action", true);
        }//Si no, entonces esta en modo de reposo
        else{
            //Multiplicador para saber si esta corriendo o caminando
            float mult;
            
            //Si no esta parado, puede girarse
            if (direction.x != 0)
            {
                transform.localScale = new Vector3(direction.x, 1, 1);
            }

            //Si esta corriendo o caminando, tendra diferentes velocidades
            if (GameplayActions.RunAction)
            {
                Speed = GameplaySpeeds.RunSpeed;
                mult = 2;
            }
            else
            {
                Speed = GameplaySpeeds.WalkSpeed;
                mult = 1;
            }
            //Setting the variables on the Animator
            anim.SetFloat("Movx", Mathf.Abs(direction.x / mult));
            ShootingObject.ArmsAnim.SetBool("Action", false);
            ShootingObject.ArmsAnim.SetBool("CanShowup", false);
            //Iluminacion de la pistola
            ShootingObject.IlluminateShoot();
        }
       
        //Salto
        if (Input.GetButtonDown("Up") && Grounded && Canjump && !GameplayActions.DodgeAction) {
            //S
            if (!CanJump())
            {
                jump = true;
                
            }
            anim.SetTrigger("Jump");
            tempdirection = direction;
            if (direction.x != 0 && !GameplayActions.RunAction) {
                tempspeed = WalkingJumpSpeed;
            }
            else if (direction.x != 0 && GameplayActions.RunAction) {
                tempspeed = RunJumpSpeed;
            }
            else if (direction.x == 0) {
                tempspeed = 0;
            }
        }
        if (Grounded)
        {
            
            if (!jump){
                tempspeed = Speed;
                tempdirection = Direction;
            }
        }
        else {
            Direction = tempdirection;
            Speed = tempspeed;
        }

        if (Input.GetKeyDown(KeyCode.K)){
            Impulse(new Impact(magnitude));
        }

        anim.SetBool("Action", action);
        anim.SetBool("Withgun", WithGun);
        anim.SetBool("OnCover", OnCover);
        anim.SetBool("Aiming", GameplayActions.SecondaryAction);
        anim.SetBool("Grounded", Grounded);
    }

    public float magnitude;
    public bool CanJump() {
        bool CanJump;
        if (WithGun)
            CanJump = StateInfo.Shooting.InCover || StateInfo.Shooting.Dodge;
        else
            CanJump = StateInfo.Fight.InCover || StateInfo.Fight.Dodge;

        return CanJump;
    }

    public override void Damaged(DistanceAttack DA)
    {
        base.Damaged(DA);
        StartCoroutine(Damaged());
        if (WithGun)
            anim.SetTrigger("DamagedS");
        else
            anim.SetTrigger("DamagedF");
    }

    public override void Damaged(MeeleAttack MA)
    {
        base.Damaged(MA);
        StartCoroutine(Damaged());
        ShakeCameraLeftUppercut();
        if (WithGun)
            anim.SetTrigger("DamagedS");
        else
            anim.SetTrigger("DamagedF");
    }

    //Metodo de muerte
    public override void Dead()
    {
        PauseMenu.Dead = true;
        AttackReciever.gameObject.SetActive(false);
        ShootReciever.enabled = false;
        anim.SetTrigger("Death");
    }

    public void Finale() {
        PauseMenu.Dead = true;
        AttackReciever.gameObject.SetActive(false);
        ShootReciever.enabled = false;
        anim.SetTrigger("Final");
        this.enabled = false;
    }

    public GameObject PanelFinal;

    public void Final() {
        PanelFinal.SetActive(true);
        gameObject.SetActive(false);
    }

    public void YouDied() {
        //Se pausara y aparecera el mensaje de la muerte
        PauseMenu.Died = true;
    }

    public bool OnEdge;

    public override void FixedUpdateThis()
    {
        base.FixedUpdateThis();
        Grounded = controller.collisions.below || OnEdge;
        if (jump)
        {
            //Rb2D.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            OnJumpInputDown();
            jump = false;
            
        }
    }

    public void ShakeCameraLeftUppercut()
    {
        Camera.main.GetComponent<FollowTarget>().GetDamage(LeftUppercut.ShakeDuration, LeftUppercut.ShakeMagnitude, 0, 0);
    }

    public void ShakeCameraNormalPunch()
    {
        Camera.main.GetComponent<FollowTarget>().GetDamage(NormalPunch.ShakeDuration, NormalPunch.ShakeMagnitude, 0, 0);
    }

    public void Reload() {
        ShootingObject.ReloadAmmo();
    }

}