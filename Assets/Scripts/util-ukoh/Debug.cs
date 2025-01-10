using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using TMPro;

/// <summary>
/// ukoh-2024-10-28
/// Real-time Debug Text
/// メッセージを毎フレイムcanvasに表示
/// 
/// dependency: TextMeshPro
/// 
/// 使い方:
/// 
/// </summary>
///

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}

namespace AU
{
    using System.Runtime.CompilerServices;
    public enum LogTiming
    {
        Update,
        Fixed,
        Late,
    }

    public class Debug : MonoBehaviour
    {

        static MsgBuffer _debugMessage;

        static public void Log(object msg, LogTiming timing, [CallerArgumentExpression("msg")] string objName = null)
        {
            return;
            string tmp = msg.ToString();
            _debugMessage.text[timing] += "\t";
            if(objName != null)
                _debugMessage.text[timing] += objName + ":  ";

            _debugMessage.text[timing] += tmp;

            if (!(tmp == System.String.Empty))
                _debugMessage.text[timing] += "<br>";
            return;
        }

        static public void Log(string msg, LogTiming timing)
        {
            return;
            string tmp = msg;
            _debugMessage.text[timing] += "\t" + tmp;

            if (!(tmp == System.String.Empty))
                _debugMessage.text[timing] += "<br>";
            return;
        }

        class MsgBuffer
        {
            public Dictionary<LogTiming, string> text;
            public MsgBuffer()
            {
                text = new Dictionary<LogTiming, string>();
                string[] strs = new string[5];

                foreach (LogTiming timing in System.Enum.GetValues(typeof(LogTiming)))
                {
                    text.Add(timing, strs[((int)timing)]);
                }
            }
        }
        [SerializeField] TMP_Text _rText;

        // Start is called before the first frame update
        void Start()
        {
            _debugMessage = new MsgBuffer();
        }
        
        void Update()
        {
            _rText.text = "";
            foreach (LogTiming timing in System.Enum.GetValues(typeof(LogTiming)))
            { 
                string tmp = _debugMessage.text[timing];
                _rText.text += tmp;
                if(!(tmp == System.String.Empty))
                    _rText.text += "<br>";
            }
            _debugMessage.text[LogTiming.Update] = System.String.Empty;
        }
        private void FixedUpdate()
        {
            _debugMessage.text[LogTiming.Fixed] = System.String.Empty;
        }

        private void LateUpdate()
        {
            _debugMessage.text[LogTiming.Late] = System.String.Empty;
        }
    }
}