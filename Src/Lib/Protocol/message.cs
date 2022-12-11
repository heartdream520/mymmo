// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: message.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace SkillBridge.Message
{

    [global::ProtoBuf.ProtoContract()]
    public partial class NUserInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id")]
        public int Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"player")]
        public NPlayerInfo Player { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NPlayerInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id")]
        public int Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"characters")]
        public global::System.Collections.Generic.List<NCharacterInfo> Characters { get; } = new global::System.Collections.Generic.List<NCharacterInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NCharacterInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id")]
        public int Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"tid")]
        public int Tid { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Name { get; set; } = "";

        [global::ProtoBuf.ProtoMember(4, Name = @"type")]
        public CharacterType Type { get; set; }

        [global::ProtoBuf.ProtoMember(5, Name = @"class")]
        public CharacterClass Class { get; set; }

        [global::ProtoBuf.ProtoMember(6, Name = @"level")]
        public int Level { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public int mapId { get; set; }

        [global::ProtoBuf.ProtoMember(8, Name = @"entity")]
        public NEntity Entity { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NVector3 : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        private int v1;
        private int v2;
        private int v3;

        public NVector3(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public NVector3()
        {
        }

        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"x")]
        public int X { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"y")]
        public int Y { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"z")]
        public int Z { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NEntity : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id")]
        public int Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"position")]
        public NVector3 Position { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"direction")]
        public NVector3 Direction { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"speed")]
        public int Speed { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NEntitySync : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id")]
        public int Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"event")]
        public EntityEvent Event { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"entity")]
        public NEntity Entity { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessage : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public NetMessageRequest Request { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public NetMessageResponse Response { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public UserRegisterRequest userRegister { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public UserLoginRequest userLogin { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public UserCreateCharacterRequest createChar { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public UserGameEnterRequest gameEnter { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public UserGameLeaveRequest gameLeave { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public MapCharacterEnterRequest mapCharacterEnter { get; set; }

        [global::ProtoBuf.ProtoMember(8)]
        public MapEntitySyncRequest mapEntitySync { get; set; }

        [global::ProtoBuf.ProtoMember(9)]
        public MapTeleportRequest mapTeleport { get; set; }

        [global::ProtoBuf.ProtoMember(10)]
        public FirstTestRequest firstRequest { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public UserRegisterResponse userRegister { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public UserLoginResponse userLogin { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public UserCreateCharacterResponse createChar { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public UserGameEnterResponse gameEnter { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public UserGameLeaveResponse gameLeave { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public MapCharacterEnterResponse mapCharacterEnter { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public MapCharacterLeaveResponse mapCharacterLeave { get; set; }

        [global::ProtoBuf.ProtoMember(8)]
        public MapEntitySyncResponse mapEntitySync { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class FirstTestRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"helloword")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Helloword { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserLoginRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"user")]
        [global::System.ComponentModel.DefaultValue("")]
        public string User { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2, Name = @"passward")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Passward { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserLoginResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result")]
        public Result Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"errormsg")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Errormsg { get; set; } = "";

        [global::ProtoBuf.ProtoMember(3, Name = @"userinfo")]
        public NUserInfo Userinfo { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserRegisterRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"user")]
        [global::System.ComponentModel.DefaultValue("")]
        public string User { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2, Name = @"passward")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Passward { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserRegisterResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result")]
        public Result Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"errormsg")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Errormsg { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserCreateCharacterRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Name { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2, Name = @"class")]
        public CharacterClass Class { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserCreateCharacterResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result")]
        public Result Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"errormsg")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Errormsg { get; set; } = "";

        [global::ProtoBuf.ProtoMember(3, Name = @"characters")]
        public global::System.Collections.Generic.List<NCharacterInfo> Characters { get; } = new global::System.Collections.Generic.List<NCharacterInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserGameEnterRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int characterIdx { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserGameEnterResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result")]
        public Result Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"errormsg")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Errormsg { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserGameLeaveRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserGameLeaveResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result")]
        public Result Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"errormsg")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Errormsg { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapCharacterEnterRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int mapId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapCharacterEnterResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int mapId { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"characters")]
        public global::System.Collections.Generic.List<NCharacterInfo> Characters { get; } = new global::System.Collections.Generic.List<NCharacterInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapCharacterLeaveResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int characterId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapEntitySyncRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public NEntitySync entitySync { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapEntitySyncResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<NEntitySync> entitySyncs { get; } = new global::System.Collections.Generic.List<NEntitySync>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MapTeleportRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int teleporterId { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"RESULT")]
    public enum Result
    {
        [global::ProtoBuf.ProtoEnum(Name = @"SUCCESS")]
        Success = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"FAILED")]
        Failed = 1,
    }

    [global::ProtoBuf.ProtoContract(Name = @"CHARACTER_TYPE")]
    public enum CharacterType
    {
        Player = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"NPC")]
        Npc = 1,
        Monster = 2,
    }

    [global::ProtoBuf.ProtoContract(Name = @"CHARACTER_CLASS")]
    public enum CharacterClass
    {
        [global::ProtoBuf.ProtoEnum(Name = @"NONE")]
        None = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"WARRIOR")]
        Warrior = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"WIZARD")]
        Wizard = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"ARCHER")]
        Archer = 3,
    }

    [global::ProtoBuf.ProtoContract(Name = @"CHARACTER_STATE")]
    public enum CharacterState
    {
        [global::ProtoBuf.ProtoEnum(Name = @"IDLE")]
        Idle = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"MOVE")]
        Move = 1,
    }

    [global::ProtoBuf.ProtoContract(Name = @"ENTITY_EVENT")]
    public enum EntityEvent
    {
        [global::ProtoBuf.ProtoEnum(Name = @"NONE")]
        None = 0,
        [global::ProtoBuf.ProtoEnum(Name = @"IDLE")]
        Idle = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"MOVE_FWD")]
        MoveFwd = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"MOVE_BACK")]
        MoveBack = 3,
        [global::ProtoBuf.ProtoEnum(Name = @"JUMP")]
        Jump = 4,
    }

}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006
