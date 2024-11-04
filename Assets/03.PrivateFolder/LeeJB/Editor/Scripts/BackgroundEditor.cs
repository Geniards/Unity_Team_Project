using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackgroundEditor : EditorWindow
{
    private GameObject _tilePrefab;        // 타일 프리팹
    private int _tileCount;                // 생성할 타일 개수
    private float _tileSpacing;            // 타일 간격
    private Vector3 _tileScale = Vector3.one;     // 타일 스케일
    private Vector3 _tileRotation = Vector3.zero; // 타일 회전
    private MonoScript _scriptToAdd;       // 추가할 스크립트

    [MenuItem("도구/배경 에디터")]
    public static void ShowWindow()
    {
        GetWindow<BackgroundEditor>("배경 편집기");
    }

    private void OnGUI()
    {
        GUILayout.Label("배경 설정", EditorStyles.boldLabel);

        _tilePrefab = (GameObject)EditorGUILayout.ObjectField("타일 프리팹", _tilePrefab, typeof(GameObject), false);
        _tileCount = EditorGUILayout.IntField("타일 개수", _tileCount);
        _tileSpacing = EditorGUILayout.FloatField("타일 간격", _tileSpacing);
        _tileScale = EditorGUILayout.Vector3Field("타일 스케일", _tileScale);
        _tileRotation = EditorGUILayout.Vector3Field("타일 회전", _tileRotation);
        _scriptToAdd = (MonoScript)EditorGUILayout.ObjectField("추가할 스크립트", _scriptToAdd, typeof(MonoScript), false);

        if (GUILayout.Button("타일 생성"))
        {
            GenerateTiles();
        }
    }

    private void GenerateTiles()
    {
        if (_tilePrefab == null)
        {
            Debug.LogWarning("프리팹이 설정되지 않았습니다");
            return;
        }

        for (int i = 0; i < _tileCount; i++)
        {
            Vector3 position = new Vector3(i * _tileSpacing, 0, 0);
            GameObject newTile = Instantiate(_tilePrefab, position, Quaternion.Euler(_tileRotation));
            newTile.transform.localScale = _tileScale;

            if (_scriptToAdd != null)
            {
                var scriptType = _scriptToAdd.GetClass();
                if (scriptType != null)
                {
                    newTile.AddComponent(scriptType);
                }
                else
                {
                    Debug.LogWarning("스크립트가 유효하지 않습니다");
                }
            }
        }
    }
}
