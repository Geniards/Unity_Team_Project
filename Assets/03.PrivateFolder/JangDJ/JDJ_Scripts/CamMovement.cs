using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public void ZoomIn(float duration, float size)
    {
        StartCoroutine(ZoomRoutine(duration, size));
    }

    private IEnumerator ZoomRoutine(float duration, float size)
    {
        float time = 0;
        float t = 0;
        float initSize = Camera.main.orthographicSize;

        while (true)
        {
            if (t >= 1)
                break;

            time += Time.deltaTime;
            t = Mathf.Clamp01(time / duration);

            Camera.main.orthographicSize = Mathf.Lerp
                (initSize, size, t);

            yield return null;
        }
    }

    public void Move(Vector3 pos, float duration)
    {
        StartCoroutine(MoveRoutine(pos, duration));
    }

    private IEnumerator MoveRoutine(Vector3 pos, float duration)
    {
        float time = 0;
        float t = 0;
        Vector3 initPos = Camera.main.transform.position;

        while (true)
        {
            if (t >= 1)
                break;

            time += Time.deltaTime;
            t = Mathf.Clamp01(time / duration);

            Camera.main.transform.position
                = Vector3.Lerp(initPos, pos, t);

            yield return null;
        }
    }
}
