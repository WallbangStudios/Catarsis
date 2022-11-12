using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiluroPhobia : Character {

    public GameObject Donovan;
    private float Distance;
    float direction = 1;
    //esta es la direccion cuando hace el dash
    private bool PositionJump;
    private bool Dashing;
    Vector2 PosDonovan;
    //variable para la funcion trigger
    bool Auxiliar;

    [SerializeField]
    GameplaySpeeds Speeds;

    public float DistanceToJump;

    public bool CanDash = false;

    [System.Serializable]
    public struct GameplaySpeeds
    {
        public float Walk;
        public float Dash;
    }


    //Variables del salto
    private bool Jump;
    private bool CanJump;
    private bool Canchange;
    private bool NoMoreDamage = false;
    private int CountDamage = 0;

    public bool ControlDirection;
    public trigger triger;


    //Se activan con un AnimationEvent
    public void change()
    {
        //En cierto momento, el mutante podra cambiar su LocalScale
        Canchange = true;
    }

    void Move()
    {


        //movimiento
        //transform.position = Vector3.MoveTowards(new Vector3 (transform.position.x, transform.position.y, 0), Direction , Speed*Time.deltaTime);



        //Animacion de correr
        //anim.SetFloat("Direction", Mathf.Abs(direction));
    }


    public void jump()
    {
        //Sera un pequeño impulso hacia arriba para darle credibilidad a la animacion
        //Rb2D.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    }

    public override void UpdateThis()
    {

        base.UpdateThis();

        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool prejump = stateinfo.IsName("Prejump");
        bool jumping = stateinfo.IsName("Jump");
        bool postjump = stateinfo.IsName("Postjump");
        bool AttackBite = stateinfo.IsName("AttackBite");
        bool Movement = stateinfo.IsName("Movement");
        bool Damaged = stateinfo.IsName("Damaged");
        float jumptime = stateinfo.normalizedTime;



        Distance = Mathf.Abs(Donovan.transform.position.x - transform.position.x);

        if (PositionJump)
        {
            //Acumular la direccion
            direction = Mathf.Sign(Donovan.transform.position.x - transform.position.x);

            //Acumular la posicion de donovan
            Direction = new Vector2(Mathf.Sign(Donovan.transform.position.x - transform.position.x), 0);

            PositionJump = false;
            CanDash = false;

        }


        if (prejump || postjump)
        {

            //Dejara de moverse
            Speed = 0;
            if (postjump && Canchange)
            {
                //En cierto punto de la Animacion de PostJump podra girar ailurofobia
                direction = Mathf.Sign((transform.position.x - Donovan.transform.position.x));
                Canchange = false;
                NoMoreDamage = false;
                CountDamage = 0;
            }

        }
        if (jumping)
        {

            Speed = Speeds.Dash;

        }

        if (!triger.entrar_ascensor) {
            Speed = 0;
            Direction = new Vector2(0, 0);
            //
            direction = 0;
            //
            anim.SetFloat("Direction", Mathf.Abs(direction));

            transform.localScale = new Vector3(1, 1, 1);
        }

        //esto es para que el trigger se ejecute frame por frame
        Dashing = Trigger(Distance < DistanceToJump && Movement, ref Auxiliar);
        if (Distance < DistanceToJump && Movement && !Dashing) {
        }
        if (Movement && triger.entrar_ascensor)
        {

            CanDash = true;
            Speed = Speeds.Walk;
            Direction = new Vector2(Mathf.Sign(Donovan.transform.position.x - transform.position.x), 0);
            //
            direction = Mathf.Sign(transform.position.x - Donovan.transform.position.x);
            //
            print(Mathf.Sign(transform.position.x - Donovan.transform.position.x));
            anim.SetFloat("Direction", Mathf.Abs(direction));

            transform.localScale = new Vector3(direction, 1, 1);

            if (Dashing)
            {

                PositionJump = true;
                anim.SetTrigger("jump");

            }
        }
        if (Damaged && !NoMoreDamage)
        {
            //no se movera, y tampoco podra saltar
            Speed = 0;
            Jump = false;
            CanJump = false;
        }

    }

    public override void LoadData(){
        base.LoadData();

        //NewAttacks NA = new NewAttacks();
        //List<NewDistanceAttack> NDA = new List<NewDistanceAttack>();
        //NDA.Add(new NewDistanceAttack { Damage = 0, FieldOfFire = 10, FireRate = 2, Magazine = 30, Recoil = 10, Name = "UZI", Impulse = new Impact(10, 2, 3) });
        //NDA.Add(new NewDistanceAttack { Damage = 2, FieldOfFire = 25, FireRate = 5, Magazine = 20, Recoil = 5, Name = "M4", Impulse = new Impact(20, 30, 5) });
        //NDA.Add(new NewDistanceAttack { Damage = 1, FieldOfFire = 60, FireRate = 10, Magazine = 5, Recoil = 2, Name = "M16", Impulse = new Impact(15, 2, 9) });
        //List<NewMeeleAttack> NMA = new List<NewMeeleAttack>();
        //NMA.Add(new NewMeeleAttack { Name = "Punch", Damage = 10, Impulse = new Impact(9, 3, 4) });
        //NMA.Add(new NewMeeleAttack { Name = "Left", Damage = 20, Impulse = new Impact(10, 4, 9) });
        //NA.MeeleAttacks = NMA;
        //NA.DistanceAttacks = NDA;

        //JsonLoader<NewAttacks>.UpdateData(NA, "Attackdemo");

    }

    public override void Damaged(MeeleAttack MA)
    {

        if (!Dead)
        {
            if (!NoMoreDamage)
            {
                StartCoroutine(Damaged());
                CurrentHealth -= MA.Damage;
                if (CurrentHealth <= 0)
                {
                    Death();
                    return;
                }
                CountDamage++;
                if (CountDamage > 2 && !NoMoreDamage)
                {
                    NoMoreDamage = true;
                    Jump = true;
                    CanJump = true;
                    anim.SetTrigger("jump");
                }
                else if (MA.Damage > 0 && !NoMoreDamage)
                    anim.SetTrigger("Damaged");
                if (MA.Name == "Uppercut")
                    Impulse(new Impact(MagnitudeImpulse * -Mathf.Sign(Donovan.transform.position.x - transform.position.x), AccGrounded, AccAirbone));
            }
        }
    }

    public float MagnitudeImpulse;
    public float AccGrounded;
    public float AccAirbone;

    public override void Damaged(DistanceAttack DA)
    {

        if (!Dead)
        {
            StartCoroutine(Damaged());
            CurrentHealth -= DA.Damage;
            if (CurrentHealth <= 0)
            {
                Death();
                return;
            }
        }

    }

    public bool Dead { get; private set; }

    public void Death()
    {
        anim.SetTrigger("Dead");
        Dead = true;
        //Destroy(gameObject, 3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceToJump);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 40);
    }
}
