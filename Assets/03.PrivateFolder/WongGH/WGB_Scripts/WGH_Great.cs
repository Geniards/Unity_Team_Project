using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Great : MonoBehaviour, IPoolingObj
{
    public E_Pool MyPoolType => E_Pool.GREAT;
    float _time;
    Vector3 _destination;

    private void Update()
    {
        _destination = new Vector3(transform.position.x, transform.position.y + _time, transform.position.z);
        _time += Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _destination, 1 * Time.deltaTime);
        if (_time >= 1.5f)
        {
            _time = 0;
            Return();
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(E_Pool.GREAT, this.gameObject);
    }
}
