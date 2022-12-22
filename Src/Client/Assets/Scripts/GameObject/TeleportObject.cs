using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour {

    public int ID;
    Mesh mesh = null;
	// Use this for initialization
	void Start () {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("触发进行");
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if(playerInputController!=null&&playerInputController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if(td==null)
            {
                Debug.LogErrorFormat("TeleportObject :Character:{0} Enter Teleporter{1} But not existed"
                    , playerInputController.character.Info.Name,this.ID);
                return;
            }
            Debug.LogFormat("TeleportObject :Character:{0} Enter Teleporter{1} "
                    , playerInputController.character.Info.Name, this.ID);
            if(td.LinkTo>0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleport(this.ID);
                }
                else Debug.LogErrorFormat("Teleporter ID:{0} LinkID:{1} Error", this.ID, td.LinkTo);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(this.mesh!=null)
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
