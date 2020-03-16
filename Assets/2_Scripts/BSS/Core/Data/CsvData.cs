using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace BSS {
    /// <summary>
    /// CSV파일의 정보를 가지고 있는 Class (첫번째 행은 반드시 Key 값)
    /// </summary>
    [System.Serializable]
    public class CsvData {
        public enum LogType {
            Keys,Maps,Values
        }

        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

        public List<string> Keys;
        public List<Dictionary<string, string>> Maps = new List<Dictionary<string, string>>();
        public Dictionary<string, List<string>> Values = new Dictionary<string, List<string>>();

        //Log Value
        private bool isCreateLog;
        private string keyLog;
        private string mapsLog;
        private string valuesLog;

        public CsvData(string text,bool createLog=false) {
            string[] lines = Regex.Split(text, LINE_SPLIT_RE);

            Keys = Regex.Split(lines[0], SPLIT_RE).ToList();
            foreach (var key in Keys) {
                Values[key] = new List<string>();
            }

            for (var i = 1; i < lines.Length; i++) {
                if (string.IsNullOrEmpty(lines[i])) continue;
                var dic = new Dictionary<string, string>();
                string[] values = Regex.Split(lines[i], SPLIT_RE);
                for (int j = 0; j < values.Length; j++) {
                    string key = Keys[j];
                    string value = values[j];
                    dic[key] = value;
                    Values[key].Add(value);
                }
                Maps.Add(dic);
            }

            // 로그 생성
            isCreateLog = createLog;
            if (!isCreateLog) return;
            CreateLog();
        }

        public Dictionary<string, string> GetRowData(string key,Func<string,bool> predicate) {
            int index=Values[key].FindIndex(x => predicate(x));
            if (index == -1) return null;
            return Maps[index];
        }
        public List<Dictionary<string, string>> GetRowDataAll(string key, Func<string, bool> predicate) {
            var keyValues=Values[key].FindAll(x => predicate(x));
            return Maps.FindAll(x => keyValues.Exists(xx => xx == x[key]));
        }

        public void Log(LogType logType) {
            if (!isCreateLog) throw new Exception("이 CsvData는 로그를 생성하지 않았습니다.");
            string logText="";
            switch (logType) {
                case LogType.Keys:
                    logText = keyLog;
                    break;
                case LogType.Maps:
                    logText = mapsLog;
                    break;
                case LogType.Values:
                    logText = valuesLog;
                    break;
            }
            Debug.Log(logText);
        }


        private void CreateLog() {
            StringBuilder sb = new StringBuilder("[Keys]\n");
            foreach (var key in Keys) {
                sb.Append($"{key}, ");
            }
            keyLog = sb.ToString();
            sb.Clear();

            sb.Append("[Maps]\n");
            int k = 0;
            foreach (var map in Maps) {
                sb.Append($"[{k.ToString()}번째]");
                k++;
                foreach (var it in map) {
                    sb.Append($"(KEY: {it.Key} - VALUE: {it.Value}), ");
                }
                sb.Append("\n");
            }
            mapsLog = sb.ToString();
            sb.Clear();

            sb.Append($"[Values]\n");
            int k1 = 0;
            foreach (var value in Values) {
                k1 = 0;
                sb.Append($"[KEY:{value.Key}] ");
                foreach (var it in Values[value.Key]) {
                    sb.Append($"({k1.ToString()}) {it}, ");
                    k1++;
                }
                sb.Append("\n");
            }
            valuesLog = sb.ToString();
            sb.Clear();
        }
    }
}
