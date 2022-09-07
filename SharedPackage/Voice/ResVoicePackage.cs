using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPackage.Voice
{
    public class ResVoicePackage : CPackage
    {
        /// <summary>
        /// 所有音轨的声音流
        /// </summary>
        private Dictionary<string, byte[]> voiceBytes;

        public ResVoicePackage()
            //: base("ResVoicePackage")
        {
        }

        public ResVoicePackage(Dictionary<string, byte[]> voiceBytes)
            //: base("ResVoicePackage")
        {
            this.voiceBytes = voiceBytes;
        }

        public Dictionary<string, byte[]> VoiceBytes { get => voiceBytes; set => voiceBytes = value; }
    }
}
