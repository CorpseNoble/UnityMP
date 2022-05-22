using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Assets.Scripts
{
    public class Log_Script : MonoBehaviour
    {
        public Text text1;

        private void OnEnable()
        {
            ControllerSliderPanel_Script.LogEvent += Log;
            ControllerButtonPanel_Script.LogEvent += Log;
        }

        private void OnDisable()
        {
            ControllerSliderPanel_Script.LogEvent -= Log;
            ControllerButtonPanel_Script.LogEvent -= Log;
        }

        public void Log(string text)
        {
            text1.text = text + "\n\r" + text1.text;
        }
    }
}
