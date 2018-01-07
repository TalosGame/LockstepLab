package com.net.Util;

import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

public class SingletonBase<T> {
	private static final ConcurrentMap<Class<?>, Object> instances = new ConcurrentHashMap<Class<?>, Object>();

	public static <T> T GetInstance(Class<T> type) {
		Object obj = instances.get(type);

		try {
			if (obj == null) {
				obj = type.newInstance();
				instances.put(type, obj);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}

		return (T) obj;
	}

	public static <T> void Remove(Class<T> type) {
		instances.remove(type);
	}
}
