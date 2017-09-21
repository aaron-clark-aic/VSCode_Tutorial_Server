using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Echo.Server;

namespace VSCodeTutorial.ConsoleServer
{
    public class SingleMem
    {
        private static SingleMem _instance = new SingleMem();
     
        
        private IChannelHandlerContext _channelContext;
        private SingleMem() { }

        public void SetChannel(IChannelHandlerContext context)
        {
            this._channelContext = context;
        }

        static public SingleMem Getinstance()
        {
            return _instance;
        }

        public void FlushContext(string message)
        {
            if (this._channelContext != null)
            {
            IByteBuffer initialMessage;
            initialMessage=Unpooled.Buffer(256);
                this._channelContext.Channel.WriteAndFlushAsync(initialMessage.WriteBytes( Encoding.UTF8.GetBytes(message)));
            }else{
               Console.WriteLine("channel context is null");
            }

        }
    }


}