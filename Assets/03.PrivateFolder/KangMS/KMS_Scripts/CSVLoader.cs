using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

// ù ��° �ڸ�: 0(�ϴ�), 1(���), 2(�ߴ�)
// �� ��° �ڸ�: 0(����), 6(����) 7(����), 8(��ֹ�), 9(���ù�ư)

public class CSVLoader : MonoBehaviour
{
    [Header("CSV_FILE")]
    [SerializeField] private TextAsset csvFile;
    
    [Header("1_CSV_NOTE_POS_INDEX")]
    [SerializeField] private int _notePosStarIndex = 1;
    [SerializeField] private int _notePosEndIndex = 3;

    [Header("2_CSV_NOTE_TYPE_INDEX")]
    [SerializeField] private int _noteTypeStartIndex = 6;
    [SerializeField] private int _noteTypeEndIndex = 9;

    private Dictionary<int, List<NotePattern>> _patternDictionary = new Dictionary<int, List<NotePattern>>();

    private static bool _isLoaded = false;

    private void Awake()
    {
        if (!_isLoaded)
        {
            LoadPatterns();
            _isLoaded = true;
        }
        else
        {
            Debug.Log("CSV �����ʹ� �̹� �ε�Ǿ����ϴ�.");
        }
    }

    /// <summary>
    /// CSV ������ �о�鿩 ������ �Ľ��ϰ�, Dictionary�� �����ϴ� ����
    /// </summary>
    public void LoadPatterns()
    {
        Debug.Log("CSV ������ �ε� ����...");
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

            // ���� �Ľ�
            List<NotePattern> notePatterns = ParsePatternString(values[0], patternString);

            _patternDictionary.Add(rowNum, notePatterns);
            rowNum++;
        }
        Debug.Log("CSV ������ �ε� �Ϸ�.");
    }

    /// <summary>
    /// patternString�� �޾Ƽ�, �̸� NotePattern ����Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    private List<NotePattern> ParsePatternString(string values, string patternString)
    {
        List<NotePattern> notePatterns = new List<NotePattern>();
        NotePattern currentPattern = new NotePattern();

        for(int i = 0; i < patternString.Length; i += 2)
        {
            // ¦�� ���� �ʴ� ���� ����
            if (i + 1 >= patternString.Length)
            {
                Debug.LogError($"�߸��� ���� ������ ����: {patternString} , {patternString.Length}");
                continue;
            }
            // ��ġ ���� ��Ʈ Ÿ�� �� ��ȿ�� �˻�
            if (int.TryParse(patternString[i].ToString(), out int position) &&
                int.TryParse(patternString[i + 1].ToString(), out int noteType))
            {
                // n0 : ��Ÿ���� ���.
                if (noteType == 0)
                {
                    currentPattern.AddNoteData(position, E_NoteType.None);
                    continue;
                }

                // �� Ÿ���� �ƴ� ���
                if (IsValidPosition(position) && IsValidNoteType(noteType))
                {
                    currentPattern.AddNoteData(position, (E_NoteType)(noteType - (_noteTypeStartIndex - 1)));
                }
                else
                {
                    Debug.LogWarning($"�߸��� ��ġ �Ǵ� ��Ʈ Ÿ��: ����={values}, ��ġ={position}, Ÿ��={noteType}");
                }
            }
            else
            {
                Debug.LogError("���ڷ� ��ȯ�� �� ���� ���� ���ԵǾ� ����.");
            }
        }
        notePatterns.Add(currentPattern);
        return notePatterns;
    }

    /// <summary>
    /// ��ġ ���� ��ȿ���� Ȯ�� (0, 1, 2)
    /// </summary>
    private bool IsValidPosition(int position)
    {
        return position >= _notePosStarIndex && position <= _notePosEndIndex;
    }

    /// <summary>
    /// ��Ʈ Ÿ���� ��ȿ���� Ȯ�� (6, 7, 8, 9)
    /// </summary>
    private bool IsValidNoteType(int noteType)
    {
        return noteType >= _noteTypeStartIndex && noteType <= _noteTypeEndIndex;
    }

    /// <summary>
    /// CSV���� �Ľ��� �����͸� �ܺο��� ����
    /// </summary>
    public Dictionary<int, List<NotePattern>> GetPatternDictionary()
    {
        return _patternDictionary;
    }
}


/// <summary>
/// �پ��� ���� �����͸� DataTable�� ����
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