using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lvamon
{
    public class lvaSecrets
    {
        public string IoThubConnectionString { get; set; }
        public string eventHubConnectionString { get; set; }
    }

    public class LvaScenario
    {
        public string name { get; set; }
        public string rtspSource { get; set; }
        public string aiUri { get; set; }
        public string aiMode { get; set; }
        public string scaleMode { get; set; }
        public string frameWidth { get; set; }
        public string frameHeight { get; set; }
    }

    public class IoTMessage
    {
        public long timestamp { get; set; }
        public List<Inference> inferences { get; set; }
    }

    public class ErrorMessage
    {
        public string code { get; set; }
        public string target { get; set; }
    }

    public class Inference
    {
        public string type { get; set; }
        public Entity entity { get; set; }
    }

    public class Entity
    {
        public Tag tag { get; set; }
        public Box box { get; set; }
    }

    public class Tag
    {
        public string value { get; set; }
        public float confidence { get; set; }
    }

    public class Box
    {
        public float l { get; set; }
        public float t { get; set; }
        public float w { get; set; }
        public float h { get; set; }
    }
}
