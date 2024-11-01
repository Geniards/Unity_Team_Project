using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


// 첫 번째 자리: 0(하단), 1(상단), 2(중단)
// 두 번째 자리: 0(없음), 6(점수) 7(몬스터), 8(장애물), 9(동시버튼)

public class CSVLoader : MonoBehaviour
{
    [Header("CSV_FILE")]
    // [SerializeField] private TextAsset csvFile;
    private string[] stagePaths = {
    "https://docs.google.com/spreadsheets/d/1ePsHhe1aytlantyxEtWgOqB1_wqfzXi7pOcflHKH3Ao/export?format=csv",
    "https://docs.google.com/spreadsheets/d/1nLP9UALDVYAVuiPoQK1cLqt1lJ6YszJCvEss5kPX-vc/export?format=csv",
    "https://docs.google.com/spreadsheets/d/1wXeC8fm5HGoImtESPGjdL9Z1AJu3_kfprVBQ6K1Kxq4/export?format=csv",
    "https://docs.google.com/spreadsheets/d/1F2LLPogwUYH766xLctyAUlFKiE0-_7rvEb7A_QGSKQU/export?format=csv",
    "https://docs.google.com/spreadsheets/d/1ZMV03suV-nM5YGjuqFOtDkE3LGtJXzjsfBPa8xvewwQ/export?format=csv"
    };

    [Header("1_CSV_NOTE_POS_INDEX")]
    [SerializeField] private int _notePosStarIndex = 1;
    [SerializeField] private int _notePosEndIndex = 3;

    [Header("2_CSV_NOTE_TYPE_INDEX")]
    [SerializeField] private int _noteTypeStartIndex = 6;
    [SerializeField] private int _noteTypeEndIndex = 9;

    private Dictionary<int, List<NotePattern>> _patternDictionary = new Dictionary<int, List<NotePattern>>();

    private static bool _isLoaded = false;
    public int stageNumber; // 일단 임의로 설정해뒀습니다

    private void Awake()
    {
        StartCoroutine(LoadPatterns(stageNumber));
    }

    /// <summary>
    /// CSV 파일을 읽어들여 패턴을 파싱하고, Dictionary에 저장하는 역할
    /// </summary>
    IEnumerator LoadPatterns(int stageNumber)
    {

        UnityWebRequest request = UnityWebRequest.Get(stagePaths[stageNumber - 1]);
        yield return request.SendWebRequest();

        string receiveText = request.downloadHandler.text;
        Debug.Log("CSV 데이터 로드 시작...");
        ParseCSVData(receiveText);
        Debug.Log("CSV 데이터 로드 완료.");
    }

    private void ParseCSVData(string csvData)
    { 
        StringReader reader = new StringReader(csvData);
        bool isHeader = true;
        int rowNum = 1;

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();

                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                string[] values = line.Split(',');
                string patternString = values[1];

                // 패턴 파싱
                List<NotePattern> notePatterns = ParsePatternString(values[0], patternString);

                _patternDictionary.Add(rowNum, notePatterns);
                rowNum++;
            }
    }

    /// <summary>
    /// patternString을 받아서, 이를 NotePattern 리스트로 변환하는 함수
    /// </summary>
    private List<NotePattern> ParsePatternString(string values, string patternString)
    {
        List<NotePattern> notePatterns = new List<NotePattern>();
        NotePattern currentPattern = new NotePattern();

        for (int i = 0; i < patternString.Length; i += 2)
        {
            // 짝이 맞지 않는 경우는 무시
            if (i + 1 >= patternString.Length)
            {
                Debug.LogError($"잘못된 패턴 데이터 길이: {patternString} , {patternString.Length}");
                continue;
            }
            // 위치 값과 노트 타입 값 유효성 검사
            if (int.TryParse(patternString[i].ToString(), out int position) &&
                int.TryParse(patternString[i + 1].ToString(), out int noteType))
            {
                // n0 : 빈타입인 경우.
                if (noteType == 0)
                {
                    currentPattern.AddNoteData(position, E_NoteType.None);
                    continue;
                }

                // 빈 타입이 아닌 경우
                if (IsValidPosition(position) && IsValidNoteType(noteType))
                {
                    currentPattern.AddNoteData(position, (E_NoteType)(noteType - (_noteTypeStartIndex - 1)));
                }
                else
                {
                    Debug.LogWarning($"잘못된 위치 또는 노트 타입: 패턴={values}, 위치={position}, 타입={noteType}");
                }
            }
            else
            {
                Debug.LogError("숫자로 변환할 수 없는 값이 포함되어 있음.");
            }
        }
        notePatterns.Add(currentPattern);
        return notePatterns;
    }

    /// <summary>
    /// 위치 값이 유효한지 확인 (0, 1, 2)
    /// </summary>
    private bool IsValidPosition(int position)
    {
        return position >= _notePosStarIndex && position <= _notePosEndIndex;
    }

    /// <summary>
    /// 노트 타입이 유효한지 확인 (6, 7, 8, 9)
    /// </summary>
    private bool IsValidNoteType(int noteType)
    {
        return noteType >= _noteTypeStartIndex && noteType <= _noteTypeEndIndex;
    }

    /// <summary>
    /// CSV에서 파싱한 데이터를 외부에서 접근
    /// </summary>
    public Dictionary<int, List<NotePattern>> GetPatternDictionary()
    {
        return _patternDictionary;
    }
}


/// <summary>
/// 다양한 패턴 데이터를 DataTable과 연계
/// </summary>
public class NotePattern
{
    public List<NoteData> Notes { get; private set; } = new List<NoteData>();

    public void AddNoteData(int Position, E_NoteType noteType)
    {
        Notes.Add(new NoteData(Position, noteType));
    }

    public void AddNoteData(NoteData noteData)
    {
        Notes.Add(noteData);
    }

    public List<NoteData> GetNoteDataList()
    {
        return Notes;
    }
}

public class NoteData
{
    public int position;
    public E_NoteType noteType;

    public NoteData(int position, E_NoteType noteType)
    {
        this.position = position;
        this.noteType = noteType;
    }
   
}

/*
    if (!_isLoaded)
    {
        LoadPatterns();
        _isLoaded = true;
    }
    else
    {
        Debug.Log("CSV 데이터는 이미 로드되었습니다.");
    }
    */