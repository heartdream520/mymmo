﻿using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnPoint : MonoBehaviour {

    public int ID;
    Mesh mesh = null;
    // Use this for initialization
    void Start () {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh,
                this.transform.position + Vector3.up * this.transform.localPosition.y * .5f,
                this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif

}
