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
    private string[] stagePaths =
    {
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
    private int stageNumber;
    private Dictionary<int, List<NotePattern>> _patternDictionary = new Dictionary<int, List<NotePattern>>();
    private static bool _isLoaded = false;
    
    /// CSV 파일을 읽어들여 패턴을 파싱하고, Dictionary에 저장하는 역할
    /// </summary>
    public IEnumerator LoadAllPatterns()
    {
        List<UnityWebRequest> requests = new List<UnityWebRequest>();
        // 각 stagePaths에 대한 요청을 준비하고 시작.
        for (int i = 0; i < stagePaths.Length; i++)
        {
            UnityWebRequest request = UnityWebRequest.Get(stagePaths[i]);
            requests.Add(request);
            // 각 요청을 비동기적으로 전송
            request.SendWebRequest();
        }
        // 모든 요청이 완료될 때까지 대기
        foreach (var request in requests)
        {
            while (!request.isDone)
            {
                yield return null;
            }
            // 요청 실패 확인
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"데이터를 불러오는데 실패 : {request.error}");
                yield break;
            }
        }
        Debug.Log("모든 CSV 데이터 로드 완료 시작...");
        // 요청이 완료된 모든 데이터를 처리
        for (int i = 0; i < requests.Count; i++)
        {
            string csvData = requests[i].downloadHandler.text;
            int stageNumber = i + 1;
            ParseCSVData(csvData, stageNumber);
            Debug.Log($"CSV 데이터 로드 완료 (스테이지 {stageNumber}).");
        }
        Debug.Log("모든 스테이지의 CSV 데이터 로드 및 저장 완료.");
    }
    /// <summary>
    /// CSV 데이터를 파싱하여 NotePattern 리스트로 변환합니다.
    /// </summary>
    private void ParseCSVData(string csvData, int stageNumber)
    {
        StringReader reader = new StringReader(csvData);
        bool isHeader = true;
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
            // 패턴을 파싱하여 개별 NotePattern을 DataManager에 등록
            NotePattern pattern = ParsePatternString(values[0], patternString);
            if (pattern != null)
            {
                DataManager.Instance.CSVData.RegistPattern(stageNumber, pattern);
            }
        }
    }
    /// <summary>
    /// patternString을 NotePattern 객체로 변환하는 함수
    /// </summary>
    private NotePattern ParsePatternString(string values, string patternString)
    {
        NotePattern currentPattern = new NotePattern();
        for (int i = 0; i < patternString.Length; i += 2)
        {
            if (i + 1 >= patternString.Length)
            {
                Debug.LogError($"잘못된 패턴 데이터 길이: {patternString} , {patternString.Length}");
                continue;
            }
            if (int.TryParse(patternString[i].ToString(), out int position) &&
                int.TryParse(patternString[i + 1].ToString(), out int noteType))
            {
                if (noteType == 0)
                {
                    currentPattern.AddNoteData(position, E_NoteType.None);
                    continue;
                }
                if (IsValidPosition(position) && IsValidNoteType(noteType))
                {
                    currentPattern.AddNoteData(position, (E_NoteType)(noteType - 5));
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
        return currentPattern;
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