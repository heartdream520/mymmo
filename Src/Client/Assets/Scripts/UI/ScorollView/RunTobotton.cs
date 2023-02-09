using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTobotton : MonoBehaviour {

    ScrollRect Scroll;
    private void Start()
    {
        this.Scroll = this.GetComponent<ScrollRect>();
    }
    public void runTobotton()
    {
        StartCoroutine("run");
    }
    IEnumerator run()
    {
        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        this.Scroll.normalizedPosition = new Vector2(0, 0);
    }
}
