using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ù ��° �ڸ�: 0(����), 5(���), 6(�ߴ�), 7(�ϴ�)
// �� ��° �ڸ�: 0(����), 1(����), 2(��ֹ�)

public class CSVLoader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    private Dictionary<int, List<NotePattern>> _patternDictionary = new Dictionary<int, List<NotePattern>>();

    /// <summary>
    /// CSV ������ �о�鿩 ������ �Ľ��ϰ�, Dictionary�� �����ϴ� ����
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

            // ���� �Ľ�
            List<NotePattern> notePatterns = ParsePatternString(patternString);

            _patternDictionary.Add(rowNum, notePatterns);
            rowNum++;
        }
    }

    /// <summary>
    /// patternString�� �޾Ƽ�, �̸� NotePattern ����Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    private List<NotePattern> ParsePatternString(string patternString)
    {
        List<NotePattern> notePatterns = new List<NotePattern>();

        for(int i = 0; i < patternString.Length; i += 2)
        {
            // ù��° ��ġ, �ι�° Ÿ��
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