using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class TextManager : MonoBehaviour {

    public Text textLine1;
    public Text textLine2;
    public Text textLine3;

    string textFeeder1;
    string textFeeder2;

    int textIndexer1 = 0;
    int textIndexer2 = 1;
    int textIndexer3 = 0;

    StreamReader streamRead = new StreamReader(@"Assets\Resources\test.txt");
    StreamReader streamRead2 = new StreamReader(@"Assets\Resources\test2.txt");

    string[] textArray;
    string[] textArray2;

    public bool textOver = false;

    // Use this for initialization
    void Start ()
    {
        TextPrep();
    }
	
	// Update is called once per frame
	void Update ()
    {
        TextAdvance();
	}

    void TextPrep()
    {
        textFeeder1 = streamRead.ReadLine();
        textFeeder2 = streamRead2.ReadLine();
        //string testerino = "test1,test2,test";
        textArray = textFeeder1.Split(',');
        textArray2 = textFeeder2.Split(',');

        //Debug.Log(textFeeder1);
        textLine1.text = textArray[textIndexer1];
        textLine2.text = textArray[textIndexer2];
        textLine3.text = textArray2[textIndexer3];

        streamRead.Close();
        streamRead2.Close();
    }

    void TextAdvance()
    {
        if (Input.GetKeyDown("space"))
        {
            textIndexer1 = textIndexer1 + 2;
            textIndexer2 = textIndexer2 + 2;
            textIndexer3++;
            textLine1.text = textArray[textIndexer1];
            textLine2.text = textArray[textIndexer2];
            textLine3.text = textArray2[textIndexer3];
        }

        if (textLine1.text == "LINEOVER" || textLine2.text == "LINEOVER")
        {
            this.gameObject.SetActive(false);
            textOver = true;
        }
    }
}
