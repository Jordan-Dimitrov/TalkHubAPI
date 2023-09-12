using System.Collections.Generic;
using System.Linq;

namespace ChatAppTest
{
    public class ChatRegistry
    {
        private readonly Dictionary<string, List<UserMessage>> _roomMessages = new();
        public ChatRegistry()
        {
            _roomMessages["staq"] = new List<UserMessage>();
        }

        public void CreateRoom(string room)
        {
            _roomMessages[room] = new();
        }

        public void AddMessage(string room, UserMessage message)
        {
            _roomMessages[room].Add(message);
        }

        public List<UserMessage> GetMessages(string room)
        {
            return _roomMessages[room];
        }

        public List<string> GetRooms()
        {
            return _roomMessages.Keys.ToList();
        }
    }
}