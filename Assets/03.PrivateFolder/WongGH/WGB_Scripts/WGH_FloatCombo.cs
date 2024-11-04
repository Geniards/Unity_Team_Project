using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_FloatCombo : MonoBehaviour
{
    [SerializeField] WGH_AreaJudge _judge;
    private int _count;
    private bool _isFirst = true;
    private Stack<IPoolingObj> _lastObjs = new Stack<IPoolingObj>();

    private void Start()
    {
        _judge = GetComponent<WGH_AreaJudge>();
        EventManager.Instance.AddAction(E_Event.STAGE_END, ResetCombo, this);
    }

    private void OnDestroy()
    {
        ResetCombo();
    }
    /// <summary>
    /// 콤보를 리셋하는 메서드
    /// </summary>
    public void ResetCombo()
    {
        _judge.SetComboReset();
        _isFirst = true;

        while (true)
        {
            if (_lastObjs.Count <= 0)
                break;
        
            _lastObjs.Pop().Return();
        }
    }

    public void SpawnCombo(int comboNum)
    {
        _count = comboNum;
        if (_isFirst == true)
        {
            _isFirst = false;
            return;
        }

        else
        {
            while (true)
            {
                if (_lastObjs.Count <= 0)
                    break;

                _lastObjs.Pop().Return();
            }
        }

        WGH_Combo combo;
        Vector3 standardPos = new Vector3(0, 2.5f, 0);

        int[] arr = new int[4]; // 100의 자리 숫자라고 가정
        float[] poses = new float[4];
        Array.Fill(poses, float.MaxValue);
        arr[0] = comboNum / 1000;
        arr[1] = (comboNum / 100) % 10;
        arr[2] = (comboNum / 10) % 10;
        arr[3] = comboNum % 10;
        int numLength = 0;
        int firstIdx = arr.Length - 1;       // 첫 숫자 위치
        int midIdx = -1;
        for (int i = 0; i < arr.Length; i++) // 첫 숫자 위치 확인
        {
            if (arr[i] != 0)
            {
                numLength = arr.Length - i; // 총 길이 확인 1 = 4 - 3
                firstIdx = i;                              // 3
                midIdx = numLength / 2; // 3 => 1 , 2 => 1 , 4 => 2 짝수 기준 : 우측
                break;
            }
        }

        if(midIdx == 0)
        {
            combo = ObjPoolManager.Instance.GetObject<WGH_Combo>(E_Pool.COMBO);
            combo.Initialize(standardPos, comboNum);
            combo.ChangeColor(comboNum);
            _lastObjs.Push(combo);

            return;
        }

        
        float IntervalDist = 0.8f;
        // ex 0100 1 1 2 3
        // 총 4   -3-1 1 3
        //    2       -1 1
        // 홀수면 가운데숫자를 가운데 두고, 짝수면 양방향셋팅
        float posX;
        //float startPos = IntervalDist * ? //=> -3
        int right;
        int left;
        // midIx가 0이 아닌 경우에만 poses[midIdx-1]를 설정
        if (midIdx != 0)
        {
            poses[midIdx - 1] = -1 * IntervalDist / 2; // idx 1
            poses[midIdx] = IntervalDist / 2; // idx 2
        }
        else
        {
            poses[midIdx] = 0;
        }

        if (numLength % 2 == 0) // 짝수라면
        {
            //              2           4           need 0 , 3 idx   // 한번 루프
            for (int i = midIdx; i < numLength; i = i + 2) // 0 1 2 3
            {
                // i = 2
                //      1   -   1 = 0
                left = (midIdx - 1) - (i / 2);
                right = midIdx + (i / 2);
                poses[left] = poses[left + 1] - IntervalDist;
                poses[right] = poses[right - 1] + IntervalDist;
            }
        }
        else
        {
            poses[midIdx] = standardPos.x;
            //              1           3
            for (int i = midIdx; i < numLength; i = i + 2)
            {
                //      1    -      0 -1  = 0
                if (numLength % 2 != 0)                                                 // midIdx 가 0이면서 numLength가 1일 경우에는 문제가 발생
                {                                                                       // numLength 를 2로 나누면 0이 되는가의 조건에 따라 Left의 값이 음수가 되지 않도록 조정
                    left = midIdx - (i / 2) - 1;
                }
                else
                {
                    left = midIdx - (i / 2);
                }
                //      1   +       0 + 1 = 2
                right = midIdx + (i / 2) + 1;
                if(left >= 0)
                {
                    //  [0]     =   [1 + 0]
                    poses[left] = poses[midIdx - (i / 2)] - IntervalDist; // midIdx가 0보다 작은 음수일 경우에는 0으로 설정하기 위해 Mathf.max 사용
                }
                
                
                poses[right] = poses[midIdx + (i / 2)] + IntervalDist;
            }
        }
        // 2,4 = [v] [v] [] []
        // 1,2,3,4 = [v] [v] [v] [v]
        // 1,2,3 = [v] [v] [v]
        
        for (int i = 0; i < poses.Length; i++)
        {
            if (poses[i] == float.MaxValue)
                break;

            combo = ObjPoolManager.Instance.GetObject<WGH_Combo>(E_Pool.COMBO);
            combo.ChangeColor(comboNum);
            combo.Initialize(standardPos + Vector3.right * poses[i], arr[firstIdx + i]);
            
            _lastObjs.Push(combo);
        }
    }
}
