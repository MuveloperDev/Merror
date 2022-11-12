using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private void Anim_Move()
    {
        _Animator.SetFloat("HorizontalSpeed", Speed * Horizontal);
        _Animator.SetFloat("VerticalSpeed", Speed * Vertical);
        _Animator.SetBool("IsMove", IsMove);
    }
    private void Anim_Crouch()
    {
        _Animator.SetBool("IsCrouch", IsCrouch);
    }
}
