using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public class SoundTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.MAIN_BGM);
        SEManager.Instance.Play(SEPath.CANDY_GET);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
