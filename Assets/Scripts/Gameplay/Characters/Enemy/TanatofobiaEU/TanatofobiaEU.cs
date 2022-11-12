using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanatofobiaEU : Character {

    public GameObject Objective;
    [SerializeField]
    float Distance;

    public float DistoAtteck;

    float DirectionAttck;
    Vector2 PosObjective;

    bool CanAttack;

    //Variable que se usa para el metodo Trigger
    bool Auxiliar;

    public float WalkSpeed;

    public DarkRoomManager Manager;

    public override void LoadData()
    {
        base.LoadData();
        Deaded = false;

    }
    public bool Deaded { get; private set; }

    public override void UpdateThis()
    {
        if (Deaded || PauseMenu.Dead)
            return;
        Speed = WalkSpeed;
        base.UpdateThis();

        AnimatorStateInfo Stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool Movement = Stateinfo.IsName("Movement");
        bool Attack = Stateinfo.IsName("Attack");
        bool Dead = Stateinfo.IsName("Dead");
        bool Death = Stateinfo.IsName("Death");

        Distance = Mathf.Abs(Objective.transform.position.x - transform.position.x);
        if (Distance > 40)
        {
            Speed = 0;
            Direction = new Vector2(0, 0);
            //
            //
            anim.SetFloat("Direction", Mathf.Abs(0));

            transform.localScale = new Vector3(1, 1, 1);
        }

        Attack = Trigger(Distance < DistoAtteck && Movement, ref Auxiliar);
        if (Movement && !(Distance > 40)) {
            Direction = new Vector2(Mathf.Sign(Objective.transform.position.x - transform.position.x), 0);

            anim.SetFloat("Direction",Mathf.Abs(Direction.x));
            transform.localScale = new Vector3(-Mathf.Sign(Direction.x), 1, 1);

            if (Attack) {
                anim.SetTrigger("Attack");
            }
        }
        if (Attack) {
            Direction = Vector2.zero;
        }
        if (Dead && Death) {
            Direction = Vector2.zero;
        }
    }

    public override void Damaged(DistanceAttack DA)
    {
        if (Deaded)
            return;
        StartCoroutine(Damaged());
        base.Damaged(DA);
    }

    public override void Damaged(MeeleAttack MA)
    {
        if (Deaded)
            return;
        StartCoroutine(Damaged());
        base.Damaged(MA);
    }

    public override void Dead()
    {
        anim.SetTrigger("Dead");
        Deaded = true;
        Direction = Vector2.zero;
    }

    public void Death() {
        Manager.numbersdeath++;
        Destroy(transform.gameObject);
    }

}
