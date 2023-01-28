using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using UnityEngine;
namespace GameServer.Services
{
    class DBService : Singleton<DBService>
    {
        ExtremeWorldEntities entities;

        public ExtremeWorldEntities Entities
        {
            get { return this.entities; }
        }

        public void Init()
        {
            entities = new ExtremeWorldEntities();
            
        }
        private float time;
        private bool is_save;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="async">同步还是异步</param>
        internal void Save(bool async = false)
        {
            if (async)
            {
                Log.InfoFormat("DBSaveAsync");
                entities.SaveChangesAsync();
            }
            else
            {
                //this.is_save = true;
                entities.SaveChanges();
            }
               
            
        }
        public void Update()
        {
            if(is_save)
            {
                this.time = 1f;
                is_save = false;
            }
            if(time>0)
            {
                time -= Time.deltaTime;
                if (time < 0)
                {
                    Log.InfoFormat("DBSave");
                    entities.SaveChanges();
                }
                    
            }
        }
    }
}
