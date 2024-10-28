using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;     // ���������� ��� �迭
    [SerializeField] private GameObject[] floors;          // ���������� �ٴ� �迭
    private int _currentStageIndex = 0;                    // ���� �������� �ε���

    private void Start()
    {
        SetStage(_currentStageIndex);                      // �ʱ� �������� ����
    }

    /// <summary>
    /// ���������� �����ϴ� �޼���
    /// </summary>
    public void ChangeStage(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < backgrounds.Length && stageIndex < floors.Length) // ��ȿ�� �ε������� Ȯ��
        {
            _currentStageIndex = stageIndex;             // ���� �������� �ε��� ������Ʈ
            SetStage(_currentStageIndex);                // �������� ���� �޼��� ȣ��
        }
    }

    private void SetStage(int index)
    {
        for (int i = 0; i < backgrounds.Length; i++)     // ��� ����� ��Ȱ��ȭ �� ���� ���������� Ȱ��ȭ
        {
            backgrounds[i].SetActive(i == index);        // ���� ���������� ��ġ�ϴ� ��游 Ȱ��ȭ
        }

        for (int i = 0; i < floors.Length; i++)          // ��� �ٴ��� ��Ȱ��ȭ �� ���� ���������� Ȱ��ȭ
        {
            floors[i].SetActive(i == index);             // ���� ���������� ��ġ�ϴ� �ٴڸ� Ȱ��ȭ
        }
    }
}
