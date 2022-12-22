using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {

    public int npcId;

    SkinnedMeshRenderer renderer;
    Animator animation;
    Color origonColor;

    private bool inInteractive = false;
    NpcDefine npcDefine;

    private void Start()
    {
        renderer = this.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        animation = this.gameObject.GetComponent<Animator>();
        origonColor = renderer.sharedMaterial.color;
        npcDefine = NpcManager.Instance.GetNpcDefine(npcId);
        this.StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        while(true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);
            else yield return new WaitForSeconds(UnityEngine.Random.Range(5f,10f));
            this.Relax();
        }
    }

    private void Relax()
    {
        animation.SetTrigger("Relax");
    }
    void Interactive()
    {
        if(!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());

        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if(NpcManager.Instance.Interactive(npcId))
        {
            animation.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }
    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(this.transform.forward,faceTo))>5f)
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }
    private void OnMouseDown()
    {
        Interactive();
    }
    private void OnMouseOver()
    {
        Highlight(true);
    }
    private void OnMouseEnter()
    {
        Highlight(true);
    }
    private void OnMouseExit()
    {
        Highlight(false);
    }
    private void Highlight(bool highlight)
    {
        if(highlight)
        {
            if(renderer.sharedMaterial.color!=Color.red)
            {
                renderer.sharedMaterial.color = Color.red;
            }
        }
        else
        {
            if (renderer.sharedMaterial.color != origonColor)
            {
                renderer.sharedMaterial.color = origonColor;
            }
        }
    }
}
