using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class EnumDic
    {
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
        private static Dictionary<GulidTitle, string> Gulid_posts_dic;
        public static Dictionary<GulidTitle, string> Gulid_Posts_Dic
        {
            get
            {
                if (Gulid_posts_dic == null)
                {
                    Gulid_posts_dic = new Dictionary<GulidTitle, string>();
                    Gulid_posts_dic[GulidTitle.None] = "精英";
                    Gulid_posts_dic[GulidTitle.President] = "会长";
                    Gulid_posts_dic[GulidTitle.VicePresident] = "副会长";
                }
                return Gulid_posts_dic;
            }
        }
    }
}
