//WARNING: DON'T EDIT THIS FILE!!!
using Common;

namespace Network
{
    public class MessageDispatch<T> : Singleton<MessageDispatch<T>>
    {
        //������Ϣ
        public void Dispatch(T sender, SkillBridge.Message.NetMessageResponse message)
        {
            if (message.userRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userRegister); }
            if (message.userLogin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userLogin); }
            if (message.createChar != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.createChar); }
            if (message.gameEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameEnter); }
            if (message.gameLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameLeave); }
            if (message.mapCharacterEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterEnter); }
            if (message.mapCharacterLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterLeave); }
            if (message.mapEntitySync != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapEntitySync); }   

            if (message.Bagsave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.Bagsave); }   
            if (message.itemBuy != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.itemBuy); }   

            if (message.statusNotify != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.statusNotify); }   
            if (message.itemEquip != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.itemEquip); }   

            if (message.questList!= null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questList); }   
            if (message.questAccept!= null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questAccept); }   
            if (message.questSubmit != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questSubmit); }   
            if (message.questAbandon != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questAbandon); }   

            if (message.friendAddRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendAddRequest); }   
            if (message.friendAddResponset != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendAddResponset); }   
            if (message.friendList != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendList); }   
            if (message.friendRemove != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendRemove); }

            if (message.teamInviteRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInviteRequest); }   
            if (message.teamInviteResponse != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInviteResponse); }   
            if (message.teamLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamLeave); }   
            if (message.teamInfo != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInfo); }   

            if (message.gulidCreat != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidCreat); }   
            if (message.gulidInfo != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidInfo); }   
            if (message.gulidJoinRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidJoinRequest); }   
            if (message.gulidJoinResponse != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidJoinResponse); }   
            if (message.gulidLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidLeave); }   
            if (message.gulidList != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidList); }   
            if (message.gulidAdmin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidAdmin); }   
            if (message.Chat != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.Chat); }   
            

        }
        //������Ϣ
        public void Dispatch(T sender, SkillBridge.Message.NetMessageRequest message)
        {
            if (message.userRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender,message.userRegister); }
            if (message.userLogin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.userLogin); }
            if (message.createChar != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.createChar); }
            if (message.gameEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameEnter); }
            if (message.gameLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gameLeave); }
            if (message.mapCharacterEnter != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapCharacterEnter); }
            if (message.mapEntitySync != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapEntitySync); }
            if (message.mapTeleport != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.mapTeleport); }

            //��Ϣ�ַ�
            if (message.firstRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.firstRequest); }

            if (message.Bagsave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.Bagsave); }

            if (message.itemBuy != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.itemBuy); }
            if (message.itemEquip != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.itemEquip); }

            if (message.questList != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questList); }
            if (message.questAccept != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questAccept); }
            if (message.questSubmit != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questSubmit); }
            if (message.questAbandon != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.questAbandon); }
            //if (message.statusNotify != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.statusNotify); }

            if (message.friendAddRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendAddRequest); }
            if (message.friendAddResponset != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendAddResponset); }
            if (message.friendList != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendList); }
            if (message.friendRemove != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.friendRemove); }

            if (message.teamInviteRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInviteRequest); }
            if (message.teamInviteResponse != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInviteResponse); }
            if (message.teamLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamLeave); }
            if (message.teamInfo != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.teamInfo); }

            if (message.gulidCreat != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidCreat); }
            if (message.gulidInfo != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidInfo); }
            if (message.gulidJoinRequest != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidJoinRequest); }
            if (message.gulidJoinResponse != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidJoinResponse); }
            if (message.gulidLeave != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidLeave); }
            if (message.gulidList != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidList); }
            if (message.gulidAdmin != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.gulidAdmin); }

            if (message.Chat != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.Chat); }

        }
    }
}