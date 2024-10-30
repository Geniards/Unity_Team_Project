using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false)
    where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        { 
        
        
        }
    
    }



    /*
     제네릭함수. (gqmeObject go, 이름, 재귀) => 부모통해 자식 찾는데, 1대손만 찾을건지 더 찾을건지
    
    if 부모 없을시
        리턴
    if 재귀x
        transform을 통해 접근해 GetChild()
    else
        for ~
     
     
     
     */

}
