using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Perfect : MonoBehaviour/*, IPoolingObj*/
{
    //private E_Pool my_PoolType => E_Pool.Perfect;
    float _time;
    Vector3 _destination;

    private void OnEnable()
    {
        
    }
    private void Update()
    {
        _destination = new Vector3(transform.position.x, transform.position.y + _time, transform.position.z);
        _time += Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _destination, 1 * Time.deltaTime);
        if (_time >= 1.5f)
        {
            Return();
            // 임시 destroy
            Destroy(gameObject);
        }
    }
    public void Return()
    {
        //ObjPoolManager.Instance.ReturnObj(E_Pool.Perfect, this.gameObject);
    }
}
