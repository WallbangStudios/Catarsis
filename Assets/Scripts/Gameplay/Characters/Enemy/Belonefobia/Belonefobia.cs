using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belonefobia : Character
{

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

    private bool Canchange;

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

    public void Reposition(float plus) {
        transform.position = new Vector3(transform.position.x + plus * Mathf.Sign(Direction.x), transform.position.y, transform.position.z);
    }


    public void Dash(float Magnitude) {
        Speed = Magnitude;
    }

    public LayerMask Layer;

    public override void UpdateThis()
    {

        if (Death)
        {
            Speed = 0;
            Direction = new Vector2(0, 0);
            //
            direction = 0;
            //
            anim.SetFloat("Direction", Mathf.Abs(direction));

            transform.localScale = new Vector3(1, 1, 1);
            return;
        }

        base.UpdateThis();

        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool prejump = stateinfo.IsName("Prejump");
        bool jump = stateinfo.IsName("Jump");
        bool postjump = stateinfo.IsName("Postjump");
        bool movement = stateinfo.IsName("Movement");
        bool damaged = stateinfo.IsName("Damaged");



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
                transform.localScale = new Vector3(direction, 1, 1);
                Canchange = false;
            }

        }
        if (jump)
        {

            Speed = Speeds.Dash;

        }
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 40, transform.position.y + 4.5f), Vector2.right, 80, Layer);
        Debug.DrawLine(new Vector2(transform.position.x - 40, transform.position.y + 4.5f), new Vector2(transform.position.x + 40, transform.position.y + 4.5f));

        if (!hit)
        {
            Speed = 0;
            Direction = new Vector2(0, 0);
            //
            direction = 0;
            //
            anim.SetFloat("Direction", Mathf.Abs(direction));

            transform.localScale = new Vector3(1, 1, 1);
        }

        //esto es para que el trigger se ejecute frame por frame
        Dashing = Trigger(Distance < DistanceToJump && movement, ref Auxiliar);
        if (movement && hit)
        {

            CanDash = true;
            Speed = Speeds.Walk;
            Direction = new Vector2(Mathf.Sign(Donovan.transform.position.x - transform.position.x), 0);
            //
            direction = Mathf.Sign(transform.position.x - Donovan.transform.position.x);
            //
            //print(Mathf.Sign(transform.position.x - Donovan.transform.position.x));
            anim.SetFloat("Direction", Mathf.Abs(direction));

            transform.localScale = new Vector3(direction, 1, 1);

            if (Dashing)
            {

                PositionJump = true;
                anim.SetTrigger("jump");

            }
        }
        if (damaged)
        {

            Speed = 0;
            PositionJump = false;
            CanDash = false;

        }

    }

    public override void LoadData()
    {

        base.LoadData();

    }

    public override void Damaged(MeeleAttack MA)
    {
        if (Death)
            return;

        base.Damaged(MA);

        if (MA.Damage > 0)
        {
            StartCoroutine(Damaged());
        }
    }



    public override void Damaged(DistanceAttack DA)
    {
        if (Death)
            return;
        base.Damaged(DA);

        StartCoroutine(Damaged());

    }

    public bool Death { get; private set; }

    public override void Dead() {
        Death = true;
        anim.SetTrigger("Dead");

    }


}
