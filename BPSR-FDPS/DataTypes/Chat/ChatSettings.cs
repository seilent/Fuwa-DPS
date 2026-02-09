using System.Collections.Concurrent;

namespace BPSR_FDPS.DataTypes.Chat
{
    public class ChatSettings
    {
        public int MaxChatHistory { get; set; } = 200;
        public ConcurrentDictionary<long, UserBlock> BlockedUsers { get; set; } = [];
    }
}
