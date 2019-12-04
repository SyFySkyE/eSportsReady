using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    GameObject InfoBlockOne;
    GameObject CreditsBlock;
    GameObject BTStartButton;
    GameObject EndButton;
    Vector3 CreditsPos;
    Vector3 InfoBlockOnePos;
    Vector3 BTStartButtonPos;
    Vector3 EndGameButtonPos;
    float fadeTime = 0.01f;
    float scrollTime = .001f;


    // Start is called before the first frame update
    void Start()
    {
        InfoBlockOne = GameObject.Find("InfoOne");
        CreditsBlock = GameObject.Find("Credits");
        BTStartButton = GameObject.Find("BTStartButton");
        EndButton = GameObject.Find("EndButton");

        InfoBlockOnePos = new Vector3(0, -400, 0);
        CreditsPos = new Vector3(0, -200, 0);
        BTStartButtonPos = new Vector3(0, -1700, 0);
        EndGameButtonPos = new Vector3(0, -2000, 0);



        InfoBlockOne.transform.Translate(InfoBlockOnePos);
        CreditsBlock.transform.Translate(CreditsPos);
        BTStartButton.transform.Translate(BTStartButtonPos);
        EndButton.transform.Translate(EndGameButtonPos);

        StartCoroutine(ScrollUp());
    }

    // Update is called once per frame
    void Update()
    {
        //CreditsBlock.transform.Translate(0, 1, 0);
    }

    IEnumerator ScrollUp()
    {

        int time = 0;
        int time2 = 0;
        yield return new WaitForSeconds(2f);

        while (time < 2000)
        {
            InfoBlockOne.transform.Translate(0,1,0);
            CreditsBlock.transform.Translate(0,1,0);
            BTStartButton.transform.Translate(0, 1, 0);
            EndButton.transform.Translate(0, 1, 0);
            yield return new WaitForSeconds(scrollTime);
            time++;
        }

        while (time2 < 500)
	    {
            InfoBlockOne.transform.Translate(0, 1, 0);
            CreditsBlock.transform.Translate(0, 1, 0);
            yield return new WaitForSeconds(scrollTime);
            time2++;
        }

    }
}
