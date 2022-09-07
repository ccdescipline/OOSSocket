using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPackage.Voice
{
    /// <summary>
    /// 语音房间
    /// </summary>
    public class VoiceRoom
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        private string roomId;
        /// <summary>
        /// 房间名字
        /// </summary>
        private string roomName;

        /// <summary>
        /// 根据每个client id存放房间用户的声音流
        /// </summary>
        ConcurrentDictionary<string, byte[]> voices = new ConcurrentDictionary<string, byte[]>();

        public VoiceRoom(string roomId, string roomName)
        {
            this.roomId = roomId;
            this.roomName = roomName;
        }

        public ConcurrentDictionary<string, byte[]> Voices { get => voices; }
        public string RoomId { get => roomId; }
        public string RoomName { get => roomName; }

        /// <summary>
        /// 根据客户端id写流数据
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="data"></param>
        public void WriteByteByclientId(string clientId, byte[] data)
        {
            try
            {
                voices[clientId] = data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
