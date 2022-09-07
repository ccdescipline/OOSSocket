using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedPackage.Voice;

namespace TestServer.VoiceProc
{
    class VoiceRoomCenter
    {
        public static ConcurrentDictionary<string, VoiceRoom> RoomList = new ConcurrentDictionary<string, VoiceRoom>();


        /// <summary>
        /// 创建一个房间 ,返回房间ID
        /// </summary>
        public static string CreateRoom(string RoomName)
        {
            Guid roomId = Guid.NewGuid();
            RoomList.TryAdd(roomId.ToString(),new VoiceRoom(roomId.ToString(), RoomName));

            return roomId.ToString();
        }

        /// <summary>
        /// 根据房间名称获取房间名称
        /// </summary>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        public static string GetRoomName(string RoomId)
        {
            try
            {
                return RoomList[RoomId].RoomName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
            
        }

        /// <summary>
        /// 获取所有房间信息
        /// </summary>
        /// <returns></returns>
        public static List<VoiceRoom> GetRoomsInfo()
        {
            try
            {
                return RoomList.Values.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <summary>
        /// 根据房间ID 写入声音流数据
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="clientID"></param>
        /// <param name="voiceStream"></param>
        public static void Writebyte(string RoomId,string clientID, byte[] voiceStream)
        {
            try
            {
                RoomList[RoomId].WriteByteByclientId(clientID, voiceStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        /// <summary>
        /// 获取除自己之外的所有声音信息
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public static Dictionary<string, byte[]> GetAnthorVoice(string RoomId, string clientID)
        {
            return RoomList[RoomId].Voices
                .Where((x) => { return x.Key != clientID; })
                .ToDictionary(item => item.Key, item => item.Value);
        }


        /// <summary>
        /// 加入某个房间
        /// </summary>
        public static void JoinRoom(string RoomID,string ClientID)
        {
            VoiceRoom CurrentRoom = RoomList[RoomID];

            //判断有没有客户端
            if (!CurrentRoom.Voices.Keys.Contains(ClientID))
            {
            }
        }

        /// <summary>
        /// 离开某个房间
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="ClientID"></param>
        public static void leaveRoom(string RoomID, string ClientID)
        {
            VoiceRoom CurrentRoom = RoomList[RoomID];

            CurrentRoom.Voices.Remove(ClientID, out byte[] voice);

            //判断如果一个人都没了就把房间也关了
            if (CurrentRoom.Voices.Count==0)
            {
                RoomList.Remove(RoomID, out VoiceRoom C);
            }
        }

        /// <summary>
        /// 根据clientID 离开某个房间
        /// </summary>
        /// <param name="ClientID"></param>
        public static void leaveRoom(string ClientID)
        {
            //找clienid在那些房间
            //var rooms = (x.Value.Voices.Where((x) => { return x.Key == ClientID; }).ToDictionary(item => item.Key, item => item.Value)).Keys


            //RoomList.Where((x) => { return x.Value.Voices.Where((x) => { return x.Key == ClientID; }); });

            var res = RoomList.Where(
                (x) => {
                    //找声音列表的key包含clientID
                    return x.Value.Voices.ContainsKey(ClientID);
                
                });

            foreach (var item in res)
            {
                leaveRoom(item.Key,ClientID);
            }
            
        }



    }
}
