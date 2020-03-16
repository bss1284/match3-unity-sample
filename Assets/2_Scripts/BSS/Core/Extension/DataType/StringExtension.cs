using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace BSS.Extension {
    public static class StringExtension {
        public static string SetColor(this string str,string hexCode) {
            return "<color=" + hexCode + ">" + str + "</color>";
        }
        public static string SetColor(this string str, Color color) {
            return "<color=" + color.ToHex() + ">" + str + "</color>";
        }
    }
    public static class StringEx {
        public static List<string> GetSingleLineList(string multiLineStr) {
            List<string> list = new List<string>();
            using (StringReader sr = new StringReader(multiLineStr)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    list.Add(line);
                }
            }
            return list;
        }
    }
}
