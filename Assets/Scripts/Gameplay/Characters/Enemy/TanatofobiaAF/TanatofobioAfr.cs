using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanatofobioAfr : Character {

	public float Distance {get; private set;}
	public float Dir{get; private set;}

    public float DistoAtteck;

	public GameObject Donovan;

    public GameObject Spear;

    public bool Deaded { get; private set; }

    public float WalkSpeed;

    bool Auxiliar;

    private Vector2 DirectionEnemy;

    public Transform Spawner;

	public override void LoadData(){
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
        bool Movement = Stateinfo.IsName("Movement");
        bool Attack = Stateinfo.IsName("Attack");
        bool Dead = Stateinfo.IsName("Dead");
        bool Death = Stateinfo.IsName("Death");

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - DistoAtteck, transform.position.y + 4.5f), Vector2.right, DistoAtteck * 2, layer);
        

        Distance = Mathf.Abs(Donovan.transform.position.x - transform.position.x);

        Attack = Trigger(hit && Movement, ref Auxiliar);
        if (Movement) {
            Direction = new Vector2(Donovan.transform.position.x - transform.position.x, 0);

            //anim.SetFloat("Direction",Mathf.Abs(Direction.x));
            transform.localScale = new Vector3(-Mathf.Sign(Direction.x), 1, 1);

            if (Attack) {
                anim.SetTrigger("Eyacular"); 
            }
        }
        if (Attack) {
            Direction = Vector2.zero;
        }
        if (Dead && Death) {
            Direction = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - DistoAtteck, transform.position.y + 4.5f), new Vector2(transform.position.x + DistoAtteck, transform.position.y + 4.5f));
    }

    public void Throw(){
        GameObject A = Instantiate(Spear, Spawner.position, Quaternion.Euler(0, 0, 0));
        A.transform.localScale = transform.localScale;
        A.SetActive(true);
        LanceMovement Control= A.GetComponent<LanceMovement>();

        float DirectionThrow = transform.localScale.x;

        if(DirectionThrow==1)
        {
            Control.Direction= Vector2.left;
        }
        else
        {
            Control.Direction= Vector2.right;
        }

        Destroy(A, 5f);
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
        if (MA.Name == "Uppercut") {
            anim.SetTrigger("Damaged");
            Impulse(new Impact(30 * -Mathf.Sign(Donovan.transform.position.x - transform.position.x), 0.2f));
        }
    }

    public override void Dead() {
        anim.SetTrigger("Dead");
        Deaded = true;
    }
}
