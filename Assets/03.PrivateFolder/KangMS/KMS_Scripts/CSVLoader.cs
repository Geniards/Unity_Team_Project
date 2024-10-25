using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 첫 번째 자리: 0(없음), 5(상단), 6(중단), 7(하단)
// 두 번째 자리: 0(없음), 1(몬스터), 2(장애물)

public class CSVLoader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    private Dictionary<int, List<NotePattern>> _patternDictionary = new Dictionary<int, List<NotePattern>>();

    /// <summary>
    /// CSV 파일을 읽어들여 패턴을 파싱하고, Dictionary에 저장하는 역할
    /// </summary>
    public void LoadPatterns()
    {
        StringReader reader = new StringReader(csvFile.text);
        bool isHeader = true;
        int rowNum = 1;

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            if(isHeader)
            {
                isHeader = false;
                continue;
            }

            string[] values = line.Split(',');
            string patternString = values[1];

            // 패턴 파싱
            List<NotePattern> notePatterns = ParsePatternString(patternString);

            _patternDictionary.Add(rowNum, notePatterns);
            rowNum++;
        }
    }

    /// <summary>
    /// patternString을 받아서, 이를 NotePattern 리스트로 변환하는 함수
    /// </summary>
    private List<NotePattern> ParsePatternString(string patternString)
    {
        List<NotePattern> notePatterns = new List<NotePattern>();

        for(int i = 0; i < patternString.Length; i += 2)
        {
            // 첫번째 위치, 두번째 타입
            int position = int.Parse(patternString[i].ToString());
            int noteType = int.Parse(patternString[i+1].ToString());

            NotePattern pattern = new NotePattern(position, (E_NoteType)noteType);
        }

        return notePatterns;
    }
}

public class NotePattern
{
    public int position;
    public E_NoteType noteType;

    public NotePattern(int position, E_NoteType noteType)
    {
        this.position = position;
        this.noteType = noteType;
    }
}