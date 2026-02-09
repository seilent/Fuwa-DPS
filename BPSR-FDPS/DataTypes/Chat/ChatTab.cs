namespace BPSR_FDPS.DataTypes.Chat
{
    public class ChatTab(ChatTabConfig config)
    {
        public ChatTabConfig Config = config;
        public List<long> MessageIds = [];
    }
}