using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemycontroller : MonoBehaviour {

    //La velocidad y el poder de salto
    public float speed = 10f;
    public float JumpPower = 10f;

    Animator anim;
    Rigidbody2D Rb2D;
    
    //inicializando la variable de vida
    float Health = 100f;

    //Donovan es el objetivo a seguir
    //Aqui se obtienen los datos de donovan como su posicion
    public GameObject Donovan;

    //Script del Collider para transmitir daño
    public collscript Collscript;

    //La posicion de donovan en vector2D
    [HideInInspector]
    public Vector2 PosDonovan;
    //La direccion de hacia donde esta donovan
    float direction = 1;

    //Distancia que hay entre donovan y Ailurofobia
    public float Distance;
    //Variables del salto
    private bool Jump;
    private bool CanJump;
    private bool Canchange;
    private bool NoMoreDamage=false;
    private int CountDamage=0;
    
    //Se activan con un AnimationEvent
    public void change() {
        //En cierto momento, el mutante podra cambiar su LocalScale
        Canchange = true;
    }
    public void jump() {
        //Sera un pequeño impulso hacia arriba para darle credibilidad a la animacion
        Rb2D.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    }

    //Aqui se Mueve y cambia su Scale Ailurofobia
    void Move() {
        //movimiento
        transform.position = Vector2.MoveTowards(
                transform.position, PosDonovan,
                speed * Time.deltaTime);

        //giro
        transform.localScale = new Vector3(direction, 1, 1);
        //Animacion de correr
        anim.SetFloat("Direction", Mathf.Abs(direction));
    }

    //Aqui se gestionan los movimiento dependiendo del estado
    void MoveManager() {
        //Se obtienen la informacion de los estados
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool prejump = stateinfo.IsName("Prejump");
        bool jumping = stateinfo.IsName("Jump");
        bool postjump = stateinfo.IsName("Postjump");
        float jumptime = stateinfo.normalizedTime;

        //Se calcula la distancia entre Ailurofobia y Donovan
        Distance = Vector2.Distance(transform.position, Donovan.transform.position);

        //Acumular los datos antes de saltar
        if (Jump)
        {
            //Acumular la direccion para que no se cambie mientras esta saltando
            direction = Mathf.Sign((transform.position.x - Donovan.transform.position.x));
            //Acumular la posicion de donovan mas lejos para que no lo persiga en el aire
            PosDonovan = new Vector2((transform.position.x + 50) * -direction, transform.position.y);
            //En el aire no podra saltar otra vez
            Jump = false;
            CanJump = false;
            if (prejump || jumping || postjump)
                Collscript.indexAttack = 3;
        }

        if (jumping)
        {
            //Saltando tendra una velocidad horizontal de:
            speed = 30f;
        }
            //Antes de saltar y despues de saltar
        else if (postjump || prejump)
        {
            //Dejara de moverse
            speed = 0;
            if (postjump && Canchange)
            {
                //En cierto punto de la Animacion de PostJump podra girar ailurofobia
                direction = Mathf.Sign((transform.position.x - Donovan.transform.position.x));
                Canchange = false;
                NoMoreDamage = false;
                CountDamage = 0;
            }
        }
            //Si no esta ni saltando, ni recibiendo un golpe, entonces esta Corriendo hacia Donovan
        else if (!jumping && !postjump && !prejump && !stateinfo.IsName("Damaged"))
        {
            //Ahora si puede volver a saltar
            CanJump = true;
            //correra con una velocidad de:
            speed = 18f;
            //Se obtiene la posicion de donovan en X
            PosDonovan = new Vector2(Donovan.transform.position.x, transform.position.y);
            //Se obtiene la direccion hacia donde esta donovan
            direction = Mathf.Sign((transform.position.x - Donovan.transform.position.x));
            //si la distancia es menor a 20, entonces Ailurofobia Saltara
            if (Distance < 20 && CanJump)
            {
                Jump = true;
                anim.SetTrigger("jump");
            }
        }
            //Siendo lastimado
        else if (stateinfo.IsName("Damaged") && !NoMoreDamage)
        {
            //no se movera, y tampoco podra saltar
            speed = 0;
            Jump = false;
            CanJump = false;
        }
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveManager();
        Move();
	}
    
    public void Damaged(MeeleAttack MA){
        if (!NoMoreDamage) {
            Health -= MA.Damage;
            CountDamage++;
            if (CountDamage > 1 && !NoMoreDamage) {
                NoMoreDamage = true;
                Jump = true;
                CanJump = true;
                anim.SetTrigger("jump");
            }
            else if(MA.Damage>5 && !NoMoreDamage)
                anim.SetTrigger("Damaged");
            
            print(MA.Damage);
        }
        if (Health <= 0) Dead();
    }
    public void Damaged(DistanceAttack DA) {
        Health -= DA.Damage;
        if(DA.Damage>5)
                anim.SetTrigger("Damaged");
            
            print(DA.Damage);
        if (Health <= 0) Dead();
    }
    public void Dead() {
        PauseMenu.Won = true;
    }
}
