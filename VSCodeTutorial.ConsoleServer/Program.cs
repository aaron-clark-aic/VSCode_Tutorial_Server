namespace VSCodeTutorial.ConsoleServer
{
    using System;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Codecs;
    using DotNetty.Handlers.Logging;
    using DotNetty.Handlers.Tls;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    using Echo.Server;
    using Examples.Common;

    delegate void DisplayMessage(string message);

    class Program
    {
        static async Task RunServerAsync()
        {
            ExampleHelper.SetConsoleLogger();

            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            X509Certificate2 tlsCertificate = null;
            if (ServerSettings.IsSsl)
            {
                //tlsCertificate = new X509Certificate2(Path.Combine(ExampleHelper.ProcessDirectory, "dotnetty.com.pfx"), "password");
            }
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);
                bootstrap.Channel<TcpServerSocketChannel>();
                bootstrap.Option(ChannelOption.SoBacklog, 100);
                bootstrap.Handler(new LoggingHandler("SRV-LSTN"));
                bootstrap.ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (tlsCertificate != null)
                        {
                            pipeline.AddLast("tls", TlsHandler.Server(tlsCertificate));
                        }
                        //pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        // pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        // pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        pipeline.AddLast("echo", new EchoServerHandler());
                        pipeline.AddLast("User", new UserHandler());
                        // Action<string> messageTarget = ShowMessage;


                    }));

                IChannel boundChannel = await bootstrap.BindAsync(ServerSettings.Port);

                Console.ReadLine();

                await boundChannel.CloseAsync();
            }
            finally
            {

                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));

            }
        }




        static void Main()
        {
            Thread _userthread = new Thread(new UserTest().UserTaskAsync);
            _userthread.Start();
            RunServerAsync().Wait();
        }
    }

  public  class UserTest
    {
        
        public void UserTaskAsync()
        {
            
            SingleMem _s = SingleMem.Getinstance();
            
            
            while (true)
            {
                Thread.Sleep(3000);
                _s.FlushContext( String.Format("{0}+{1}","aaron-clark-aic",DateTime.Now.ToString()));
            }
        }
    }
}
