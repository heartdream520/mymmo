using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class CharacterDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }

        //基本属性
        public int Speed { get; set; }

        private static Dictionary<CharacterClass, string> characterClass_dic;
        public static Dictionary<CharacterClass, string> CharacterClass_Dic
        {
            get
            {
                if (characterClass_dic == null)
                {
                    characterClass_dic = new Dictionary<CharacterClass, string>();
                    characterClass_dic[(CharacterClass)0] = "无";
                    characterClass_dic[(CharacterClass)1] = "战士";
                    characterClass_dic[(CharacterClass)2] = "法师";
                    characterClass_dic[(CharacterClass)3] = "射手";
                }
                return characterClass_dic;
            }
        }
    }
}
