using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    GameObject InfoBlockOne;
    GameObject CreditsBlock;
    GameObject BTStartButton;
    GameObject EndButton;
    GameObject SYWButton;
    Vector3 CreditsPos;
    Vector3 InfoBlockOnePos;
    Vector3 BTStartButtonPos;
    Vector3 EndGameButtonPos;
    Vector3 SYWButtonPos;
    float fadeTime = 0.01f;
    float scrollTime = .01f;
    float scrollSpeed;


    // Start is called before the first frame update
    void Start()
    {
        InfoBlockOne = GameObject.Find("InfoOne");
        CreditsBlock = GameObject.Find("Credits");
        BTStartButton = GameObject.Find("BTStartButton");
        EndButton = GameObject.Find("EndButton");
        SYWButton = GameObject.Find("ShowWorkButton");

        InfoBlockOnePos = new Vector3(0, -400, 0);
        CreditsPos = new Vector3(0, -200, 0);
        BTStartButtonPos = new Vector3(0, -2200, 0);
        EndGameButtonPos = new Vector3(0, -2500, 0);
        SYWButtonPos = new Vector3(0, -2400, 0);

        scrollSpeed = 2.5f;


        InfoBlockOne.transform.Translate(InfoBlockOnePos);
        CreditsBlock.transform.Translate(CreditsPos);
        BTStartButton.transform.Translate(BTStartButtonPos);
        EndButton.transform.Translate(EndGameButtonPos);
        SYWButton.transform.Translate(SYWButtonPos);

        StartCoroutine(ScrollUp());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            BTStartButton.transform.position = new Vector2(960, 800);
            SYWButton.transform.position = new Vector2(960, 300);
            EndButton.transform.position = new Vector2(960, 600);
            InfoBlockOne.SetActive(false);
            CreditsBlock.SetActive(false);
        }
    }

    IEnumerator ScrollUp()
    {
        int time = 0;
        int time2 = 0;
        yield return new WaitForSeconds(1f);

        while (time < 1050)
        {
            InfoBlockOne.transform.Translate(0,scrollSpeed,0);
            CreditsBlock.transform.Translate(0,scrollSpeed,0);
            BTStartButton.transform.Translate(0, scrollSpeed, 0);
            EndButton.transform.Translate(0, scrollSpeed, 0);
            SYWButton.transform.Translate(0, scrollSpeed, 0);
            yield return new WaitForSeconds(scrollTime);
            time++;
        }

        while (time2 < 200)
	    {
            InfoBlockOne.transform.Translate(0, scrollSpeed, 0);
            CreditsBlock.transform.Translate(0, scrollSpeed, 0);
            yield return new WaitForSeconds(scrollTime);
            time2++;
        }
    }
}
