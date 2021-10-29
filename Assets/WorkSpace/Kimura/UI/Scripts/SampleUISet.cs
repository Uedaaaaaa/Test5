using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SampleUISet : MonoBehaviour
{
    SetCharacerUI sample;
    [SerializeField] GameObject UIsample;
    // Start is called before the first frame update
    void Start()
    {
        sample = UIsample.GetComponent<SetCharacerUI>();
    }
    void SetUI(int a,bool flg)
    {
        Debug.Log("int,bool");
    }
    void SetUI(bool flg)
    {
        Debug.Log("bool");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            sample.DiceStopUISet();
            SetUI(1,false);
        }
        else
        {
            sample.DiceStopUIDestroy();
        }
        if(Input.GetKey(KeyCode.B))
        {
            SetUI(false);
            sample.DiceNumUISet(3);
        }
        else
        {
            sample.DiceNumUIDestroy();
        }
    }
}
