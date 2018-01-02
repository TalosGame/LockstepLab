import com.net.udp.UDPServiceHandler;

import io.netty.bootstrap.Bootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioDatagramChannel;

public class UDPService implements Runnable {
	private Bootstrap bs;
	private EventLoopGroup nioEventLoopGroup;
	private int port;
	
	public UDPService(int port)
	{
		this.port = port;
	}

	@Override
	public void run()
	{
		try
		{
			nioEventLoopGroup = new NioEventLoopGroup();

			bs = new Bootstrap();
			bs.group(nioEventLoopGroup);
			bs.channel(NioDatagramChannel.class);
			bs.handler(new ChannelInitializer<NioDatagramChannel>()
			{
				@Override
				protected void initChannel(NioDatagramChannel ch) throws Exception
				{
					//  receive client request
					ch.pipeline().addLast(new UDPServiceHandler());
				}

				@Override
				public void channelActive(ChannelHandlerContext ctx) throws Exception
				{
					super.channelActive(ctx);
				}
			});

			ChannelFuture f = bs.bind(port).sync();
			f.channel().closeFuture().sync();
		} catch (Exception e)
		{
			e.printStackTrace();
		} finally
		{
			nioEventLoopGroup.shutdownGracefully();
		}
	}
}
