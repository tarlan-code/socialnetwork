using System.Collections.Generic;

namespace Zust.Application.Extensions
{
    public static class ChatExtension
    {
        public static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();


        public static void AddUser(this string ConnectionId,string Username)
        {
            ConnectedUsers.Add(Username,ConnectionId);
        }

        public static void RemoveUser(this string ConnectionId)
        {
            ConnectedUsers.Remove(ConnectedUsers.FirstOrDefault(x => x.Value.Equals(ConnectionId)).Key);
             ;
        }

        public static string? GetConnectionIdByUsername(this string username)
        {
            ConnectedUsers.TryGetValue(username, out string? value);
            return value;
        }
    }
}
