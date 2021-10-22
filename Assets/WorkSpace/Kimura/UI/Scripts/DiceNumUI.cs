using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceNumUI : MonoBehaviour
{
    public Image MyImage;
    public Sprite[] Num = new Sprite[6];
    int sampleNum = 0;
    bool nullflg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!nullflg)
        {
            sampleNum++;
            if (sampleNum == 6)
            {
                sampleNum = 0;
            }
            MyImage.sprite = Num[sampleNum];
        }
        else
        {
            Destroy(this.gameObject);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            nullflg = true;
        }
    }
}
