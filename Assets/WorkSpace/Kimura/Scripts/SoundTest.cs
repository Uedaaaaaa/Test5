using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public class SoundTest : MonoBehaviour
{
    SetCharacerUI sample;
    [SerializeField] GameObject UIsetobj;
    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.MAIN_BGM);
        SEManager.Instance.Play(SEPath.CANDY_GET);
        sample = UIsetobj.GetComponent<SetCharacerUI>();
        //sample.PlayerStatusUISet();
    }

    // Update is called once per frame
    void Update()
    {
        //sample.PlayerStatusUISet();
    }
}
