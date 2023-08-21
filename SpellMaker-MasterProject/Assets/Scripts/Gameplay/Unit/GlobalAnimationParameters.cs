using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalAnimationParameters
{
    public const int MILLISECONDS_IN_SECONDS = 1000;

    public static readonly int RUN = Animator.StringToHash("Run");

    public static readonly int BASE_ATTACK = Animator.StringToHash("BaseAttack");
    public static readonly float BASE_ATTACK_DURATION = 1.5f;

    public static readonly int BASE_BLOCK_IDLE = Animator.StringToHash("BaseBlockIdle");

    public static readonly int HIT = Animator.StringToHash("Hit");

    public static readonly int DEAD = Animator.StringToHash("Dead");


}
