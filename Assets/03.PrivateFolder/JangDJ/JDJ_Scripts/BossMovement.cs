using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private Coroutine _moveRoutine;

    public void Move(Vector3 destination, float speed)
    {
        StopAllRoutine();
        _moveRoutine = StartCoroutine(MoveRoutine(destination, speed));
    }

    private IEnumerator MoveRoutine(Vector3 destination, float speed)
    {
        float t = 0;

        while (true)
        {
            if (t == 1)
                break;

            transform.position = Vector3.Lerp(transform.position, destination, t * speed);

            t += Time.deltaTime;

            yield return null;
        }
    }

    private void StopAllRoutine()
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);
    }
}

