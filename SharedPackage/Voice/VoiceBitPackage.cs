using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPackage.Voice
{
    /// <summary>
    /// 声音包
    /// </summary>
    public class VoiceBitPackage : CPackage
    {
        /// <summary>
        /// 房间Id
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// 语音流
        /// </summary>
        public byte[] VoiceData { get; set; }

        public VoiceBitPackage()
           // : base("VoiceBitPackage")
        {
        }

        public VoiceBitPackage(string roomId, byte[] voiceData)
            //: base("VoiceBitPackage")
        {
            RoomId = roomId;
            VoiceData = voiceData;
        }
    }
}
