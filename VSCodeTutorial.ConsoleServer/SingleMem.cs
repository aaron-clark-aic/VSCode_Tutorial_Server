using DotNetty.Transport.Channels;
using Echo.Server;

namespace VSCodeTutorial.ConsoleServer
{
    public class SingleMem
    {
        private static SingleMem _instance =new SingleMem();
        private  IChannelHandlerContext _channel;
        private SingleMem(){}
 
        public void SetChannel(IChannelHandlerContext context){
            this._channel = context;
        }

        static public SingleMem Getinstance(){
            return _instance;
        }

        public void FlushContext(){
            EchoServerHandler _e =new EchoServerHandler();
            if(this._channel!=null)
            _e.FlushOrder(this._channel);
        }


    }
}