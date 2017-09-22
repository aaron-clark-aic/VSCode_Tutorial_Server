using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;

namespace VSCodeTutorial.ConsoleServer
{
    public class UserHandler : ChannelHandlerAdapter
    {
        override public void ChannelActive(IChannelHandlerContext context)
        {
            SingleMem _s = SingleMem.Getinstance();
            _s.AddChannel(context);
        }
   
        //override public  Task WriteAsync(IChannelHandlerContext context, object message) => context.WriteAsync(message);
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}