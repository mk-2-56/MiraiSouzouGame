using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;

using System.Linq;

/// <summary>
/// ukoh-2024-10-28
/// Real-time Debug Text
/// メッセージを毎フレイムcanvasに表示
/// 
/// dependency: TextMeshPro
/// 
/// 使い方:
///     1. 使用先のclassでGetMsgBuffer()呼び出し、
///     返還値のMsgBuffer(class)のreferenceを保存
///     
///     2.表示したいデータを文字列として入れるだけ
///     
///     3.毎update/fixedUpdate/LateUpdate開始時に中身reset(前レフームデータクリア)
///     Note: 
///     こちらでresetしない理由:
///     スクリプトお互いの更新処理が異なるため同期できていない？
///     
///     update/fixedUpdate/LateUpdateごと使い分ける必要があるかも(update-timingが違うから)
///     todo:
///         update/fixedUpdate/LateUpdateごと自動にresetする機能?
///     
/// </summary>
/// 
public class MsgBuffer
{
    public string UpdateText;
    public string FixedText;
    public string LateText;
}

namespace AU
{ 
    public class Debug : MonoBehaviour
    {
        static public MsgBuffer GetMsgBuffer()
        {
            MsgBuffer buffer = new MsgBuffer();
            _bufferList.Add(buffer);
            return buffer;
        }
    
        TMP_Text debugMsg;
        static List<MsgBuffer> _bufferList = new List<MsgBuffer>();
    
        // Start is called before the first frame update
        void Start()
        {
            debugMsg = transform.Find("DebugText").gameObject.GetComponent<TMP_Text>();
            debugMsg.text = "starting";
        }
    
        // Update is called once per frame
        void Update()
        {
            debugMsg.text = "";
            _bufferList.ForEach( x => { debugMsg.text += x.UpdateText + "<br>"; x.UpdateText = "";} );
            _bufferList.ForEach( x => { debugMsg.text += x.FixedText + "<br>" ;} );
            _bufferList.ForEach( x => { debugMsg.text += x.LateText + "<br>"  ;} );
        }

        private void LateUpdate()
        {
            _bufferList.ForEach(x => { x.LateText = ""; });
        }
        private void FixedUpdate()
        {
            _bufferList.ForEach(x => { x.FixedText = ""; });
        }
    }
}