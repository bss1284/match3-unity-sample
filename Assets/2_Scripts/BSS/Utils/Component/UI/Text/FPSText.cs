using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace BSS {
    [RequireComponent(typeof(Text))]
    public class FPSText : MonoBehaviour {
        public float updateInterval = 0.5F;
        private Text t;
        private float accum = 0; 
        private int frames = 0;
        private float timeleft; 
        void Start() {
            timeleft = updateInterval;
            t = GetComponent<Text>();
        }
        void Update() {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;
            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0) {
                // display two fractional digits (f2 format)
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                t.text = format;
                if (fps < 30)
                    t.color = Color.yellow;
                else
                    if (fps < 10)
                    t.color = Color.red;
                else
                    t.color = Color.green;
                //DebugConsole.Log(format, level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}
