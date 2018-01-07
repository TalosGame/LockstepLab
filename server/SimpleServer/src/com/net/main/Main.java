package com.net.main;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicInteger;

import com.net.udp.UDPService;

public class Main {
	private static final AtomicInteger counter = new AtomicInteger();

	public static int nextValue() {
		return counter.getAndIncrement();
	}

	public static void main(String[] args) throws Exception {
		Thread th = new Thread(new UDPService(8090));
		th.start();

//		HashMap<Long, String> uuids = new HashMap<Long, String>(100000);
//		for (int i = 0; i < 500000; i++) {
//			UUID uuid = UUID.randomUUID();
//			long id = uuid.getMostSignificantBits() & Long.MAX_VALUE;
//			if (uuids.containsKey(id)) {
//				System.out.println("Have same uuid:" + id);
//				continue;
//			}
//
//			if (id < 0) {
//				System.out.println("Have negative uuid:" + id);
//			}
//
//			uuids.put(id, String.valueOf(id));
//		}
//
//		System.out.println("Over!!!!!" + Long.MAX_VALUE);
//
//		int i = 0;
//		i++;
	}
}
