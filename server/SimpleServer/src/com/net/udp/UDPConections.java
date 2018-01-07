package com.net.udp;

import java.net.InetSocketAddress;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

import io.netty.channel.Channel;
import io.netty.channel.group.ChannelGroup;
import io.netty.channel.group.DefaultChannelGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.util.concurrent.GlobalEventExecutor;

public class UDPConections {
	private static Map<InetSocketAddress, NetPeer> map = new ConcurrentHashMap<InetSocketAddress, NetPeer>();

//	public void name() {
//		
//	}
	
//	public void AddChanel(Channel channel) {
//
//		if (channels.contains(channel)) {
//			System.out.println("[Client] ip:" + channel.id().asShortText() + " aleady join!\n");
//			return;
//		}
//
//		channels.add(channel);
//	}
	
	private long GenerateUUID(){
		UUID uuid = UUID.randomUUID();
		long ret = uuid.getMostSignificantBits() & Long.MAX_VALUE;
		return ret;
	}
	
	
	
	
	
}
