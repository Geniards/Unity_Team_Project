using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;     // 스테이지별 배경 배열
    [SerializeField] private GameObject[] floors;          // 스테이지별 바닥 배열
    private int _currentStageIndex = 0;                    // 현재 스테이지 인덱스

    private void Start()
    {
        SetStage(_currentStageIndex);                      // 초기 스테이지 설정
    }

    /// <summary>
    /// 스테이지를 변경하는 메서드
    /// </summary>
    public void ChangeStage(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < backgrounds.Length && stageIndex < floors.Length) // 유효한 인덱스인지 확인
        {
            _currentStageIndex = stageIndex;             // 현재 스테이지 인덱스 업데이트
            SetStage(_currentStageIndex);                // 스테이지 설정 메서드 호출
        }
    }

    private void SetStage(int index)
    {
        for (int i = 0; i < backgrounds.Length; i++)     // 모든 배경을 비활성화 후 현재 스테이지만 활성화
        {
            backgrounds[i].SetActive(i == index);        // 현재 스테이지와 일치하는 배경만 활성화
        }

        for (int i = 0; i < floors.Length; i++)          // 모든 바닥을 비활성화 후 현재 스테이지만 활성화
        {
            floors[i].SetActive(i == index);             // 현재 스테이지와 일치하는 바닥만 활성화
        }
    }
}
