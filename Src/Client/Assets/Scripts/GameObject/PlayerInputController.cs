using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Services;

public class PlayerInputController : MonoBehaviour {

    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;

    public Character character;

    public float rotateSpeed = 2.0f;

    public float turnAngle = 10;

    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    // Use this for initialization
    void Start () {
        state = SkillBridge.Message.CharacterState.Idle;
        if(this.character == null)
        {
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.ConfigId = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            //通过 NCharacterInfo 创建 Character
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }
    }
    float jump_cd = 0f;

    private Vector3 old_v_pos;
    public float time = 0.1f;
    float time_space = 0f;
    void FixedUpdate()
    {
        time_space -= Time.deltaTime;
        jump_cd -= Time.deltaTime;
        if (character == null)
            return;
        

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 v_pos = new Vector3(h, 0, v).normalized;
        
        Quaternion quaternion = Quaternion.Euler(0, MainPlayerCamera.Instance.x, 0);
        v_pos = quaternion * v_pos;
        if (v_pos.sqrMagnitude > 0.1f)
        {
            if (v_pos != old_v_pos)
            {
                old_v_pos = v_pos;
                //Debug.LogErrorFormat("v:{0},h:{1},v_pos.sqrMagnitude:{2}", v, h, v_pos.sqrMagnitude);
                this.transform.rotation = Quaternion.Euler(v_pos);
                this.transform.eulerAngles = new Vector3(0, Quaternion.FromToRotation(Vector3.forward, v_pos).eulerAngles.y, 0);
                character.SetDirection(GameObjectTool.WorldToLogic(v_pos));
                rb.transform.forward = this.transform.forward;
                if(time_space<0)
                {
                    time_space = time;
                    this.SendEntityEvent(EntityEvent.None);
                }
                
            }
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
            
        }
        /*
        if (v > 0.01 || h > 0.01 || v < -0.01 || h < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        /*
        else if (v < -0.01 || h < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }*/
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jump_cd > 0) return;
            else
            {
                jump_cd = 0.5f;
                this.SendEntityEvent(EntityEvent.Jump);
            }
            
        }

        
        //转弯
        /*
        if (h<-0.01 || h>0.01)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);
            
            if(rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }
        }
        */
        
        //Debug.LogFormat("velocity {0}", this.rb.velocity.magnitude);
    }
    Vector3 lastPos;
   
    float lastSync = 0;
    
    public float x=40f;
    

    
    /// <summary>
    /// 每帧结束后更新刚体的位置
    /// </summary>
    private void LateUpdate()
    {
        
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > x)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
        time_space -= Time.deltaTime;
    }

    /// <summary>
    /// 执行动画
    /// </summary>
    /// <param name="entityEvent"></param>
    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
        
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData);

    }
    
}
