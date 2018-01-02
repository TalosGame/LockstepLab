
public class Main {
	public static void main(String[] args) throws Exception {
		Thread th = new Thread(new UDPService(8090));
		th.start();
	}
}
