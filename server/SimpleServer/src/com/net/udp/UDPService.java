package com.net.udp;
import io.netty.bootstrap.Bootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioDatagramChannel;

public class UDPService implements Runnable {
	private int port;

	public UDPService(int port) {
		this.port = port;
	}

	@Override
	public void run() {
		EventLoopGroup acceptor = null;

		try {
			acceptor = new NioEventLoopGroup();
			
			Bootstrap bs = new Bootstrap();
			bs.group(acceptor);
			bs.channel(NioDatagramChannel.class);
			bs.option(ChannelOption.SO_BROADCAST, true);
			bs.handler(new ChannelInitializer<NioDatagramChannel>() {
				@Override
				protected void initChannel(NioDatagramChannel ch) throws Exception {
					ch.pipeline().addLast(new UDPServiceHandler());
				}
			});

			ChannelFuture f = bs.bind(port).sync();
			f.channel().closeFuture().sync();
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			acceptor.shutdownGracefully();
		}
	}
}
