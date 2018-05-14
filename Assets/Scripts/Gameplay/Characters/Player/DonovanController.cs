using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonovanController : Human {

    public float JumpPower;
    public bool Grounded;
    public bool OnCover;

    public LayerMask Layer;

    public float WalkingJumpSpeed;
    public float RunJumpSpeed;

    bool jump;
    Vector2 tempdirection;
    float tempspeed;

    //Agregandole Todas las Acciones del Update
    public override void UpdateThis()
    {
        //BASE
        base.UpdateThis();
        //Todas las acciones de Donovan
            GameplayActions.PrimaryAction = Input.GetButton("Fire1");
            GameplayActions.PrimaryActionDown = Input.GetButtonDown("Fire1");
            GameplayActions.SecondaryAction = Input.GetButton("Fire2");
            GameplayActions.SecondaryActionDown = Input.GetButtonDown("Fire2");
            GameplayActions.RunAction = Input.GetButton("Run");
            GameplayActions.DodgeAction = Input.GetButtonDown("Dodge");
            GameplayActions.ReloadAction = Input.GetButtonDown("Reload");
            GameplayActions.QuitShootAction = Input.GetKeyDown(KeyCode.Q);

        //Direccion que se movera Donovan
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        //Metodo donde se Gestiona el sistema
        RaycastHit2D Hit = Physics2D.CircleCast(transform.position, 27, Vector2.zero, 0, Layer);
        bool action = Hit.collider != null;
        if (action){
            ArmsManager(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 
        }
        else{
            float mult;

            if (direction.x != 0)
            {
                transform.localScale = new Vector3(direction.x, 1, 1);
            }


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
            anim.SetFloat("Movx", Mathf.Abs(direction.x / mult));
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded) {
            jump = true;
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
            Direction = direction;
            if (!jump){
                tempspeed = Speed;
                tempdirection = Direction;
            }
        }
        else {
            Direction = tempdirection;
            Speed = tempspeed;
        }
        anim.SetBool("Action", action);
        anim.SetBool("Withgun", WithGun);
        anim.SetBool("OnCover", OnCover);
        anim.SetBool("Aiming", GameplayActions.SecondaryAction);
        anim.SetBool("Grounded", Grounded);
        
    }
    //Metodo de muerte
    public override void Dead()
    {
        //Se pausara y aparecera el mensaje de la muerte
        PauseMenu.Died = true;
    }

    public override void FixedUpdateThis()
    {
        base.FixedUpdateThis();
        if (jump)
        {
            Rb2D.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            jump = false;
        }
    }
}
