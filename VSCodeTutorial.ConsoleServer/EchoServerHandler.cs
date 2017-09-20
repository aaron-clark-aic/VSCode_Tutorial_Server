// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Echo.Server
{
    using System;
    using System.Text;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using VSCodeTutorial.ConsoleServer;
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuffer buffer = NewMethod(message);
            if (buffer != null)
            {
                Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
            }

            
            
            context.WriteAsync(message);
          
          //SingleMem _sg =SingleMem.Getinstance();
          //_sg.SetChannel(context);
        }

        private static IByteBuffer NewMethod(object message)
        {
            return message as IByteBuffer;
        }

        public void FlushOrder(IChannelHandlerContext context)
         {
             context.WriteAsync(String.Format("{0}{1}","aaron-FlushOrder ",DateTime.Now));
             context.Flush();
             
         }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}