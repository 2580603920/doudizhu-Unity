//消息处理类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoketDoudizhuProtocol;
using Google.Protobuf;

namespace Doudizhu
{
    class Message
    {
        private byte[] buffer = new byte[1024];

        private int startindex;

        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }

        public int StartIndex
        {
            get
            {
                return startindex;
            }
        }

        public int Remsize
        {
            get
            {
                return buffer.Length - startindex;
            }
        }

        //解析消息
        public void ReadBuffer(int len,Action<MainPack> HandleResponse)
        {
            startindex += len;
            while (true)
            {
                if (startindex <= 4) return; //包头占4字节，比这个小或等于就是无效包
                int count = BitConverter.ToInt32(buffer, 0);
                if (startindex >= (count + 4))
                {
                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                    HandleResponse(pack);
                    Array.Copy(buffer, count + 4, buffer, 0, startindex - count - 4); //解析完无用的包丢掉
                    startindex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

        public static byte[] PackData(MainPack pack)
        {
            byte[] data = pack.ToByteArray();//包体
            byte[] head = BitConverter.GetBytes(data.Length);//包头
            return head.Concat(data).ToArray(); //拼接成完整的包
        } 

        public static Byte[] PackDataUDP(MainPack pack)
        {
            return pack.ToByteArray();
        }

    }
}
