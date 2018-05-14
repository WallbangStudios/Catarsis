using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Playercontroller : MonoBehaviour {

    /********** Sistema de Disparo **********/
        [HideInInspector]
            public bool CanShoot =true;
        public bool aiming=true;
        public bool Withgun = true;
        public float ViewRadius;
        public GameObject Aim;
        public GameObject Arms;
        float TempScale;
        [Range(0, 360)]
            public float ViewAngle;
        /*GESTION DE SISTEMA DE DISPARO:
            Aqui se controlan todo el sistema de disparo, no el movimiento*/
        void WithgunMove(bool Dodge)
        {
                   
                    if (Input.GetKeyDown(KeyCode.LeftControl) && !Dodge && CanDodge)
                    {

                        lastpress = Time.timeSinceLevelLoad;
                        CanDodge = false;
                        anim.SetTrigger("Dodge");
                        if (Input.GetAxisRaw("Horizontal") != 0) direction = Input.GetAxisRaw("Horizontal");
                        else direction = transform.localScale.x;

                    }

                    if (timer - lastpress > 1) CanDodge = true;

                    if (!Dodge) {
                        mov = new Vector2(Input.GetAxisRaw("Horizontal"), 0);                            

                        if (mov.x != 0)
                        {
                            if (!(Input.GetMouseButton(1) || Input.GetMouseButton(0)))
                                transform.localScale = new Vector3(mov.x, 1, 1);
                        }
                        if (Input.GetMouseButton(1))
                        {
                            speed = 2f;
                            mult = 1;
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            speed = 4f;
                            mult = 1;
                        }
                        else if (Input.GetKey(KeyCode.LeftShift))
                        {
                            mult = 2;
                            speed = 12f;
                        }
                        else
                        {
                            mult = 1;
                            speed = 4f;
                        } 
                    }
                    else if (Dodge)
                    {

                        speed = 25f;
                        transform.localScale = new Vector3(-direction, 1, 1);

                    }
                    colliderb.enabled = !Dodge;
                    anim.SetFloat("Movx", Math.Abs(mov.x/mult));
        
            }
        /*GESTION DEL BRAZO:
            Aqui se gestiona el cambio de estado de si tener o no un arma*/
            void ArmsManager(bool StAttacking0, float PlaybackTime, bool Dodge)
            {
                if (!Withgun)
                {

                    AnimationManager(StAttacking0, PlaybackTime);
                    Attacks(StAttacking0, PlaybackTime);
                    Move(StAttacking0, Dodge);

                }
                else
                {
                    WithgunMove(Dodge);
                }
                if (Input.GetKeyDown(KeyCode.Q)) Withgun = !Withgun;
                Aim.SetActive(Withgun);
            }

    /********** END Sistema de disparo **********/


    /**********  Sistema de Movimiento  **********/
        public float speed = 4f;
        public float direction = -1;
                int  mult = 1;
        bool CanDodge = true;
        [HideInInspector]
            public static Vector2 mov;
        /*GESTION DE MOVIMIENTO:
            Aqui se gestiona todo el movimiento en general, incluso los movimientos de los otros sistemas de combate*/
            void Move(bool attacking, bool Dodge) {

                if (Input.GetKeyDown(KeyCode.LeftControl) && !Dodge && CanDodge){

                    lastpress = Time.timeSinceLevelLoad;
                    CanDodge = false;
                    anim.SetTrigger("Dodge");
                    if (Input.GetAxisRaw("Horizontal") != 0) direction = Input.GetAxisRaw("Horizontal");
                    else direction = transform.localScale.x;
                    if (attacking) mov = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

                }

                if (timer - lastpress > 1) CanDodge = true;

                if (!Dodge && !attacking){

                    mov = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

                    if (mov.x != 0) {
                        transform.localScale = new Vector3(mov.x, 1, 1);
                    }
                    

                    if (Input.GetKey(KeyCode.LeftShift)) mult = 2;
                    else mult = 1;

                    speed = 8 * mult - 4;
                    

                }
                else if (Dodge){

                    speed = 25f;
                    transform.localScale = new Vector3(-direction, 1, 1);

                }
                else if (attacking && !Input.GetKeyDown(KeyCode.LeftControl)){

                    mov = Vector2.zero;

                }

                colliderb.enabled = !Dodge;
                anim.SetFloat("Movx", Math.Abs(mov.x / mult));

            }

    /********** END Sistema de Movimiento **********/


    /**********  Sistema de combate  **********/
        bool IsComboAnimationPlaying = false;
        public collscript Collscript;
        float timer;
        float lastpress;
        float tab = 1;
        //Declaracion inicial de las barras de energia y vida
            float Energy = 100f;
            float Health = 100f;
        bool attacking = false;
        bool Damaging = false;
        /*GESTOR DE LAS ANIMACIONES DEL SISTEMA DE COMBATE:
            Aqui se gestionan los combos para el sistema de combate*/
            void AnimationManager(bool attacking, float PlaybackTimer){

                if (attacking){

                    if (PlaybackTimer > 0.37)
                        IsComboAnimationPlaying = false;
                    else
                        IsComboAnimationPlaying = true;


                    if (PlaybackTimer > 0.3 && PlaybackTimer < 0.75) AttackCollider.enabled = true;
                    else AttackCollider.enabled = false;


                }
            }
        /*ACTIVACION DEL ATAQUE:
                Se activa el ataque y Se hace Set del trigger y la animacion del combo es true*/
            void Attacks(bool attacking, float PlaybackTimer) {
                if (IsComboAnimationPlaying == false){
                    if (Input.GetMouseButtonDown(0)){
                        anim.SetTrigger("Punch");
                        IsComboAnimationPlaying = true;
                    }
                }
            }

    /********** END Sistema de combate **********/



    //animation
            Animator anim;

    //Variables para las fisicas
        Rigidbody2D rb2d;
        BoxCollider2D colliderb;
        public BoxCollider2D AttackCollider;
        void FixedUpdate(){
            //Movimiento fisico del jugador
            rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
        }

    // Use this for initialization
    void Start(){
        //obtener los componentes del objeto
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        colliderb = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update(){
        if (PauseMenu.GameIsPaused)
            return;
        //VARIABLE IMPORTANTE: Se usa para el sistema de CoolDown
        timer = Time.timeSinceLevelLoad;

        //recopilar la informacion de los estados de los personajes
        //IMPORTANTE para la gestion de animacion y son bastante precisos para las transiciones y demas
            AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
            float PlaybackTime = stateinfo.normalizedTime;
            bool Dodge = stateinfo.IsName("Dodge");
            bool StAttacking0 = stateinfo.IsName("Punch1") || stateinfo.IsName("Punch2");
            bool StAttacking1 = stateinfo.IsName("Punch1");
            bool StAttacking2 = stateinfo.IsName("Punch2");
            if (stateinfo.IsName("Battlemove") ) IsComboAnimationPlaying = false;
                //&& IsComboAnimationPlaying

        //SALIDA DEL JUEGO
            //if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

            if (StAttacking1)
                Collscript.indexAttack = 1;
            else if (StAttacking2)
                Collscript.indexAttack = 0;

        ArmsManager(StAttacking0, PlaybackTime, Dodge);
        CanShoot = !Dodge;
        if (!AimScript.CanMove && !Dodge && mult != 2)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = (pos - Arms.transform.position).normalized;

            if (((Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1)) ||
                (Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0))))
            {
                if (dir.x != 0)
                    TempScale = Math.Sign(dir.x);
            }

            transform.localScale = new Vector3(Math.Sign(dir.x), 1, 1);

            Arms.transform.localRotation = Quaternion.Euler(0, 0, -transform.localScale.x * AimScript.angle + 90);
        }
        else
        {
            Arms.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Damaged(MeeleAttack MA){
        Health -= MA.Damage;
        if (Health <= 0) Dead();
    }
    public void Damaged(DistanceAttack DA) {
        Health -= DA.Damage;
        if (Health <= 0) Dead();
    }
    public void Dead()
    {
        PauseMenu.Died = true;
    }

}