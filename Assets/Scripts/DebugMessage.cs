using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UnityEngine.UI を追記
public class DebugMessage : MonoBehaviour
{
    [SerializeField]
    private Text cameraPosition; //カメラの位置を表示するテキスト
    [SerializeField]
    private Text cameraRotation; //カメラの回転を表示するテキスト
    void Update()
    {
        //MainCamera のposition、rotation をUI に表示する
        cameraPosition.text = Camera.main.transform.position.ToString();
        cameraRotation.text = Camera.main.transform.rotation.ToString();
    }
}