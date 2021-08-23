using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Memory
{
    [Serializable]
    public class Data
    {
        public string RoomName;
        public string PlayerName;
        public float Time;
    }
    [Serializable]
    public class MemoryData
    {
        public List<Data> resultList = new List<Data>();
    }
}
