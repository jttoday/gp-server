using System;
using System.Net.Sockets;
using System.Text;
using LitJson;

namespace GPClient
{
	public class MessageType
    {
        static public string JoinGame = "JoinGame";
        static public string ChangeLevel = "ChangeCharacterLevel";
        static public string CharacterMove = "CharacterMove";
        static public string AddCharacter = "AddCharacter";
        static public string AddRoom = "AddRoom";
        static public string Poll = "Poll";
    }

    public class CharacterLevelData
    {
        public int speed;
        public int might;
        public int sanity;
        public int knowledge;
        
        public CharacterLevelData()
        {

        }

        public CharacterLevelData(int speed, int might, int sanity, int knowledge)
        {
            this.speed = speed;
            this.might = might;
            this.sanity = sanity;
            this.knowledge = knowledge;
        }
    }

	public class PositionData
    {
		public int level;
		public int x;
		public int y;

        public PositionData()
        {
				
        }

        public PositionData(int level, int x, int y)
        {
            this.level = level;
            this.x = x;
            this.y = y;
        }

		public PositionData(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.level = -1;
        }

        // override object.Equals
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            PositionData rhs = (PositionData)obj;
            return this.x == rhs.x && this.y == rhs.y && this.level == rhs.level;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode();
        }
	}

	public class CharacterInfo
    {
        public int modelId;
        public string name;
        public PositionData pos;
        public CharacterLevelData levelInfo;
        public int[] havingCards;

        public CharacterInfo()
        {
				
        }

        public CharacterInfo(int modelId, string name, PositionData pos, CharacterLevelData levelInfo, int[] havingCards)
        {
            this.modelId = modelId;
            this.name = name;
            this.pos = pos;
            this.levelInfo = levelInfo;
            this.havingCards = havingCards;
        }
    }

	public class RoomInfo
    {
        public int roomId;
        public int rotation;
        public Boolean[] openDoors;
        public PositionData pos;

        public RoomInfo()
        {

        }

        public RoomInfo(int roomId, int rotation, PositionData pos)
        {
            this.roomId = roomId;
            this.rotation = rotation;
            this.pos = pos;
        }
    }

    public class PollResult
    {
        CharacterInfo[] otherCharacterInfo;
        RoomInfo[] roomsInfo;

    }

    class Client
    {
        private TcpClient tcp;
        private NetworkStream ns;
        private int clientId = 110;

		private class DataWrapper
        {
            public int clientId;
            public string method;
            public string data;
			public DataWrapper(int clientId, string method, string data)
            {
                this.clientId = clientId;
                this.method = method;
                this.data = data;
            }
        }

        public void connect()
        {
            tcp = new TcpClient("127.0.0.1", 3370);
            ns = tcp.GetStream();
        }

		public void close()
        {
            ns.Close();
            tcp.Close();
        }


        public void changeChacterLevel(CharacterLevelData data)
        {

            string dataStr = JsonMapper.ToJson(data);
            String str = wrapWith(MessageType.ChangeLevel, dataStr);
            send(str);
        }

		public void characterMoveTo(PositionData data)
        {
            string dataStr = JsonMapper.ToJson(data);
            String str = wrapWith(MessageType.CharacterMove, dataStr);
            send(str);
        }

		public void addRoom(RoomInfo data)
        {
            string dataStr = JsonMapper.ToJson(data);
            String str = wrapWith(MessageType.AddRoom, dataStr);
            send(str);
        }

		public void addCharacter(CharacterInfo data)
        {
            string dataStr = JsonMapper.ToJson(data);
            String str = wrapWith(MessageType.AddCharacter, dataStr);
            send(str);
        }


		public PollResult poll()
        {
            String str = wrapWith(MessageType.Poll, "");
            ns.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            ns.Flush();
            byte[] data = new byte[1024];
            int recv = ns.Read(data, 0, data.Length);
            string stringData = Encoding.ASCII.GetString(data, 0, recv);
            PollResult result = JsonMapper.ToObject<PollResult>(stringData);
            return result;
        }

        private string wrapWith(string method, string data)
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            DataWrapper dw = new DataWrapper(clientId, method, data);
            return JsonMapper.ToJson(dw);
        }

		private void send(String str)
        {
            Console.WriteLine("Sending....");
            Console.WriteLine("data is :");
            Console.WriteLine(str);
            ns.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            ns.Flush();
        }

        //static void Main(String[] args)
        //{
        //    Client client = new Client();
        //    client.connect();
        //    PositionData pos = new PositionData(0, 2, 3);
        //    CharacterLevelData level = new CharacterLevelData(10, 20, 30, 40);
        //    int[] card = { 1, 2, 3, 4 };
        //    CharacterInfo info = new CharacterInfo(0, "hello", pos, level, card);
        //    client.addCharacter(info);
        //    Console.ReadLine();
        //    client.close();
        //}


    }
}
