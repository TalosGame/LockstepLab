package com.net.handle;

import java.net.InetSocketAddress;

import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.socket.DatagramPacket;
import io.netty.util.CharsetUtil;

public class UDPServiceHandler extends ChannelInboundHandlerAdapter {
	@Override
	public void channelRead(ChannelHandlerContext ctx, Object msg) throws Exception {
		DatagramPacket packet = (DatagramPacket) msg;
		ByteBuf buf = packet.content();
		int len = buf.readableBytes();
		byte[] d = new byte[len];
		buf.readBytes(d);

		InetSocketAddress address = packet.sender();
		String hostName = address.getHostName();
		int port = address.getPort();

		String a = new String(d, 3, len - 3, CharsetUtil.UTF_8);
		System.out.println("read from host:" + hostName + " port:" + port + " message:" + a);
	}
}
