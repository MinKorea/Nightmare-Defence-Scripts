using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader : MonoBehaviour
{
    static string SPLIT = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();

        TextAsset data = Resources.Load(file) as TextAsset; // ���ҽ� ������ �ִ� ������ TextAsset ���Ϸ� ��ȯ�ؼ� ������

        var lines = Regex.Split(data.text, LINE_SPLIT); // ������ �ؽ�Ʈ�� �� �پ� ���ڿ� �迭�� �����

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT);  // ���ڿ� �迭�� ���� ù��° ��ҿ� ����ִ� ���� �߶� Key�� ���

        for(int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT);      // �迭�� ��ҿ� ����ִ� ���� �ܾ� ���� �߶� ���ڿ� �迭�� ����
            if (values.Length == 0 || values[0] == "") continue;

            var dic = new Dictionary<string, object>();

            for(int j = 0; j < header.Length && j < values.Length; j++)
            {
                var value = values[j];

                object temp = value;
                int n;
                float f;

                if(int.TryParse(value, out n))
                {
                    temp = n;
                }
                else if(float.TryParse(value, out f))
                {
                    temp = f;
                }
                dic[header[j]] = temp;      // ����� Ű�� value���� ����
            }
            list.Add(dic);
        }
        return list;
    }
}
