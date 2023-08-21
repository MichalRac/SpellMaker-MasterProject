using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAnimationHandler : MonoBehaviour
{
    private const int SPEED_MULTIPLIER = 1000;

    [SerializeField] private Animator animator;
    [SerializeField] private int speed = 20;

    public async Task RotateTowards(Vector3 pos)
    {
        var direction = (pos - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public async Task WalkTo(Vector3 pos)
    {
        SetAnimBool(GlobalAnimationParameters.RUN, true);

        RotateTowards(pos);

        while ((transform.position - pos).magnitude >= float.Epsilon)
        {
            var newPos = Vector3.MoveTowards(transform.position, pos, ((float)speed / SPEED_MULTIPLIER) * Time.deltaTime);
            transform.position = newPos;
            await UniTask.Yield();
        }

        transform.position = pos;
        SetAnimBool(GlobalAnimationParameters.RUN, false);
    }

    public void SetAnimTrigger(int parameter)
    {
        animator.SetTrigger(parameter);
    }

    public void SetAnimBool(int parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    public async Task PlaySingleAnim(int paramter, float duration = 0.1f)
    {
        SetAnimBool(paramter, true);
        await UniTask.Delay((int)(duration * GlobalAnimationParameters.MILLISECONDS_IN_SECONDS));
        SetAnimBool(paramter, false);
    }

}
