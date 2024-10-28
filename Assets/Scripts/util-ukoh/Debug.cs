using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;

using System.Linq;

/// <summary>
/// ukoh-2024-10-28
/// Real-time Debug Text
/// ���b�Z�[�W�𖈃t���C��canvas�ɕ\��
/// 
/// dependency: TextMeshPro
/// 
/// �g����:
///     1. �g�p���class��GetMsgBuffer()�Ăяo���A
///     �ԊҒl��MsgBuffer(class)��reference��ۑ�
///     
///     2.�\���������f�[�^�𕶎���Ƃ��ē���邾��
///     
///     3.��update/fixedUpdate/LateUpdate�J�n���ɒ��greset(�O���t�[���f�[�^�N���A)
///     Note: 
///     �������reset���Ȃ����R:
///     �X�N���v�g���݂��̍X�V�������قȂ邽�ߓ����ł��Ă��Ȃ��H
///     
///     update/fixedUpdate/LateUpdate���Ǝg��������K�v�����邩��(update-timing���Ⴄ����)
///     todo:
///         update/fixedUpdate/LateUpdate���Ǝ�����reset����@�\?
///     
/// </summary>
/// 
public class MsgBuffer
{
    public string Text;
}


public class Debug : MonoBehaviour
{
    static public MsgBuffer GetMsgBuffer()
    {
        MsgBuffer buffer = new MsgBuffer();
        _bufferList.Add(buffer);
        return buffer;
    }

    TMP_Text debugDisplay;
    static List<MsgBuffer> _bufferList = new List<MsgBuffer>();

    // Start is called before the first frame update
    void Start()
    {
        debugDisplay = transform.Find("DebugText").gameObject.GetComponent<TMP_Text>();
        debugDisplay.text = "starting";
    }

    // Update is called once per frame
    void Update()
    {
        debugDisplay.text = "";
        _bufferList.ForEach( x => { debugDisplay.text += x.Text + "<br>";} );
    }
}