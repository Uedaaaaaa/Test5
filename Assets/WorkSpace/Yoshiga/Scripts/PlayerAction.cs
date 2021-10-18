using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    //ゲームマネージャー
    private GameManager manager;
    //自身のナンバー
    private int MyNo;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        MyNo = int.Parse(gameObject.name.Substring(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
