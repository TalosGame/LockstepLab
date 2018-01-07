package com.net.udp;

import java.net.InetSocketAddress;
import java.util.HashMap;

import com.net.core.PacketProperty;

import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.socket.DatagramPacket;
import io.netty.util.CharsetUtil;

public class UDPServiceHandler extends ChannelInboundHandlerAdapter {
	private HashMap<InetSocketAddress, Integer> clients = new HashMap<InetSocketAddress, Integer>();
	
	@Override
	public void channelRead(ChannelHandlerContext ctx, Object msg) throws Exception {
//		UDPConections connections = SingletonBase.GetInstance(UDPConections.class);
//		connections.AddChanel(ctx.channel());
		DatagramPacket packet = (DatagramPacket) msg;
		ByteBuf buf = packet.content();
		int len = buf.readableBytes();
		byte[] d = new byte[len];
		buf.readBytes(d);
		
		//PacketProperty 

		InetSocketAddress address = packet.sender();
		String hostName = address.getAddress().getHostAddress();
		int port = address.getPort();
		
		if(!clients.containsKey(address)){
			clients.put(address, port);
		}

		String a = new String(d, 3, len - 3, CharsetUtil.UTF_8);
		System.out.println("read from host:" + hostName + " port:" + port + " message:" + a + " cnt:" + clients.size());
	}
}
