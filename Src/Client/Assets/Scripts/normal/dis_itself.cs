using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dis_itself : MonoBehaviour {

    [Header("销毁时间")]
    public float dis_time;
	IEnumerator Start () {
        yield return new WaitForSeconds(dis_time);
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
