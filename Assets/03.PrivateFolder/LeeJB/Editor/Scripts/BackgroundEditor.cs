using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackgroundEditor : EditorWindow
{
    private GameObject tilePrefab;  // 타일 프리팹
    private int tileCount = 5;      // 생성할 타일 개수
    private float tileSpacing = 1.0f; // 타일 간격

    [MenuItem("도구/배경 편집기")]
    public static void ShowWindow()
    {
        GetWindow<BackgroundEditor>("배경 편집기");
    }

    private void OnGUI()
    {
        GUILayout.Label("배경 설정", EditorStyles.boldLabel);

        tilePrefab = (GameObject)EditorGUILayout.ObjectField("타일 프리팹", tilePrefab, typeof(GameObject), false);
        tileCount = EditorGUILayout.IntField("타일 개수", tileCount);
        tileSpacing = EditorGUILayout.FloatField("타일 간격", tileSpacing);

        if (GUILayout.Button("타일 생성"))
        {
            GenerateTiles();
        }
    }

    private void GenerateTiles()
    {
        if (tilePrefab == null)
        {
            Debug.LogWarning("타일 프리팹이 설정되지 않았습니다!");
            return;
        }

        for (int i = 0; i < tileCount; i++)
        {
            Vector3 position = new Vector3(i * tileSpacing, 0, 0);
            Instantiate(tilePrefab, position, Quaternion.identity);
        }
    }
}
