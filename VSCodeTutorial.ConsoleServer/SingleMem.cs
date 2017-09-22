using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Echo.Server;

namespace VSCodeTutorial.ConsoleServer
{
    public class SingleMem
    {
        private static SingleMem _instance = new SingleMem();


        private List<IChannelHandlerContext> _channelContexts =new List<IChannelHandlerContext>();
        private DefaultAttributeMap _attributeMap =new DefaultAttributeMap();
        private SingleMem() { }

        public void AddChannel(IChannelHandlerContext context)
        {
            if(_channelContexts.Contains(context)){
                 _channelContexts.Remove(context);                
            }
            _channelContexts.Add(context);
        }

        static public SingleMem Getinstance()
        {
            return _instance;
        }

        public void FlushContext(string message)
        {
            if (this._channelContexts.Count > 0)
            {
                IByteBuffer initialMessage;
                initialMessage = Unpooled.Buffer(256);
                
　　             long tick = DateTime.Now.Ticks;
　　             Random _ran = new Random((int)(tick & 0xffffffffL) | (int) (tick >> 32));
                this._channelContexts[_ran.Next(this._channelContexts.Count)].WriteAndFlushAsync(initialMessage.WriteBytes(Encoding.UTF8.GetBytes(message)));
                
            }
            else
            {
                Console.WriteLine("channel context is null");
            }

        }

    }


}