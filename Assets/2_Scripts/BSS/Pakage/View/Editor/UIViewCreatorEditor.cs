using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace BSS.UI {
    public class UIViewCreatorEditor {
        [MenuItem("Tools/BSS/UI/Create View")]
        public static void CreateAsset() {
            string basePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string path = EditorUtility.SaveFilePanel(
                    "Create New View",
                    basePath,
                     "NewView.cs",
                    "cs");
            string className = (path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1)).Replace(".cs", "");
            Debug.Log(path);

            
            if (File.Exists(path) == false) {
                using (StreamWriter outfile = new StreamWriter(path)) {
                    outfile.WriteLine("using System;");
                    outfile.WriteLine("using System.Collections;");
                    outfile.WriteLine("using System.Collections.Generic;");
                    outfile.WriteLine("using UnityEngine;");
                    outfile.WriteLine("using UnityEngine.UI;");
                    outfile.WriteLine("using BSS;");
                    outfile.WriteLine("using BSS.Extension;");
                    outfile.WriteLine("using BSS.UI;");
                    outfile.WriteLine("");
                    outfile.WriteLine("");
                    outfile.WriteLine("public class " + className + " : BaseView {");
                    outfile.WriteLine("");
                    outfile.WriteLine("    protected override void OnAwake () {");
                    outfile.WriteLine("        base.OnAwake();");
                    outfile.WriteLine("    }");
                    outfile.WriteLine("");
                    outfile.WriteLine("    protected override void OnStart () {");
                    outfile.WriteLine("        base.OnStart();");
                    outfile.WriteLine("    }");
                    outfile.WriteLine("");
                    outfile.WriteLine("    public override void Show () {");
                    outfile.WriteLine("        base.Show();");
                    outfile.WriteLine("    }");
                    outfile.WriteLine("");
                    outfile.WriteLine("    public override void Close () {");
                    outfile.WriteLine("        base.Close();");
                    outfile.WriteLine("    }");
                    outfile.WriteLine("");
                    outfile.WriteLine("}");
                }//File written
            }
            UnityEditor.AssetDatabase.Refresh();
            AssetDatabase.Refresh();
        }
        
    }
}
