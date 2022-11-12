using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atomosphobia : Character {
    public float Distance { get; private set; }
    public float Dir { get; private set; }

    public float DistoAtteck;

    public GameObject Donovan;

    public bool Deaded { get; private set; }

    public float WalkSpeed;

    bool Auxiliar;

    public bool Hitted;


    public override void LoadData()
    {
        base.LoadData();
    }

    public LayerMask layer;

    public override void UpdateThis()
    {
        if (Deaded)
            return;
        Speed = WalkSpeed;
        base.UpdateThis();



        AnimatorStateInfo Stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool Movement = Stateinfo.IsName("Idle");
        bool Attack = Stateinfo.IsName("Attack");
        bool Dead = Stateinfo.IsName("Dead");
        bool Death = Stateinfo.IsName("Death");

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - DistoAtteck, transform.position.y + 4.5f), Vector2.right, DistoAtteck * 2, layer);

        Hitted = hit;

        Distance = Mathf.Abs(Donovan.transform.position.x - transform.position.x);

        Attack = Trigger(hit && Movement, ref Auxiliar);
        if (Movement)
        {
            Direction = new Vector2(Donovan.transform.position.x - transform.position.x, 0);

            //anim.SetFloat("Direction", Mathf.Abs(Direction.x));
            transform.localScale = new Vector3(Mathf.Sign(Direction.x), 1, 1);

            if (Attack)
            {
                anim.SetTrigger("Atacar");
            }
        }
        if (Attack)
        {
            Direction = Vector2.zero;
        }
        if (Dead && Death)
        {
            Direction = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - DistoAtteck, transform.position.y + 4.5f), new Vector2(transform.position.x + DistoAtteck, transform.position.y + 4.5f));
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
        //if (MA.Name == "Uppercut")
        //{
        //    anim.SetTrigger("Damaged");
        //    Impulse(new Impact(30 * -Mathf.Sign(Donovan.transform.position.x - transform.position.x), 0.2f));
        //}
    }

    public override void Dead()
    {
        anim.SetTrigger("Dead");
        Deaded = true;
    }

    public void Death() {
        Destroy(transform.gameObject);
    }
}
