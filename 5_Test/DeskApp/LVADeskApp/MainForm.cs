using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Devices;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Messaging.EventHubs.Consumer;
using System.Threading;

namespace LVADeskApp
{
    enum MODE { MODE_OBJECTDETECT = 0, MODE_ANALOGGAUGE };
    public partial class MainForm : Form
    {
        CancellationTokenSource cancellationTokenSource;

        static ServiceClient serviceClient;
        static string deviceId;
        static string moduleId;

        static string eventHubConnectionString;
        static string eventHubName;

        static string rtspSource;
        static string aiUrl;
        static string scaleMode;
        static int frameWidth, frameHeight;
        static string[] _scaleMode = { "preserveAspectRatio", "pad"};

        static MODE detectMode = MODE.MODE_ANALOGGAUGE;

        public MainForm()
        {
            InitializeComponent();

            // update `appsettings.json`
            IConfigurationRoot appSettings = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            serviceClient = ServiceClient.CreateFromConnectionString(appSettings["IoThubConnectionString"]);
            deviceId = appSettings["deviceId"];
            moduleId = appSettings["moduleId"];

            eventHubConnectionString = appSettings["eventHubConnectionString"];
            eventHubName = appSettings["eventHubName"];

            // instance properties
            rtspSource = appSettings["rtspSource"];
            aiUrl = appSettings["aiUri"];
            scaleMode = appSettings["scaleMode"];
            frameWidth = int.Parse(appSettings["frameWidth"]);
            frameHeight = int.Parse(appSettings["frameHeight"]);

            // default mode = analog gauge
            cmbMode.SelectedIndex = 0;

            TextBoxWriter writer = new TextBoxWriter(txtConsole);
            Console.SetOut(writer);
        }


        private async void btnInstSet_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnInstSet_Click");

            var frm = new frmInstProp();
            frm.txtRTSPsource.Text = rtspSource;
            frm.txtAIUrl.Text = aiUrl;
            frm.cmbScale.SelectedIndex = (scaleMode == _scaleMode[0]) ? 0 : 1;
            frm.txtWidth.Text = frameWidth.ToString();
            frm.txtHeight.Text = frameHeight.ToString();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                rtspSource = frm.txtRTSPsource.Text;
                aiUrl = frm.txtAIUrl.Text;
                scaleMode = _scaleMode[frm.cmbScale.SelectedIndex];
                frameWidth = int.Parse(frm.txtWidth.Text);
                frameHeight = int.Parse(frm.txtHeight.Text);

                Console.WriteLine("properties changed");

                // TODO: save to file

                JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
                JObject jobj = new JObject{
                {"name", "AI-Graph-1"},
                {"properties", new JObject {
                    {"topologyName", "AIHttpExtension"},
                    {"description", "Sample graph description"},
                    {"parameters", new JArray{
                        new JObject { {"name", "rtspUrl"},{"value", rtspSource } },
                        new JObject { {"name", "httpAIServerAddress"},{"value", aiUrl } },
                        new JObject { {"name", "imageScaleMode"},{"value", scaleMode } },
                        new JObject { {"name", "frameWidth"},{"value", frameWidth } },
                        new JObject { {"name", "frameHeight"},{"value", frameHeight } }
                        }}
                    }}
                };

                await ExecuteGraphOperationAsync(jobj, "GraphInstanceSet", apiVersionProperty);
            }
        }

        private async void btnInstGet_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnInstGet_Click");

            //string a = await CallPostAsync();
            JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
            JObject jobj = new JObject{
                { "name", "AI-Graph-1" }
            };

            await ExecuteGraphOperationAsync(jobj, "GraphInstanceGet", apiVersionProperty);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnStart_Click");

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;

            Random rand = new Random(Guid.NewGuid().GetHashCode());

            Task task = new Task(() =>
            {
                ct.ThrowIfCancellationRequested();

                ReceiveMessagesFromDeviceAsync(cancellationTokenSource.Token).Wait();

            }, cancellationTokenSource.Token);
            task.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnStop_Click");

            cancellationTokenSource.Cancel();
        }

        private async void btnActivate_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnActivate_Click");

            //string a = await CallPostAsync();
            JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
            JObject jobj = new JObject{
                { "name", "AI-Graph-1" }
            };

            await ExecuteGraphOperationAsync(jobj, "GraphInstanceActivate", apiVersionProperty);
        }

        private async void btnDeactivate_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnDeactivate_Click");

            //string a = await CallPostAsync();
            JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
            JObject jobj = new JObject{
                { "name", "AI-Graph-1" }
            };

            await ExecuteGraphOperationAsync(jobj, "GraphInstanceDeactivate", apiVersionProperty);
        }

        private async Task ExecuteGraphOperationAsync(JObject operationParams, string operationName, JProperty apiVersionProperty)
        {
            try
            {
                JObject lvaGraphObject = operationParams;
                lvaGraphObject.AddFirst(apiVersionProperty);

                await InvokeMethodWithPayloadAsync(operationName, lvaGraphObject.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tLVA error");
            }
        }
        private async Task InvokeMethodWithPayloadAsync(string methodName, string payload)
        {
            // Create a direct method call
            var methodInvocation = new CloudToDeviceMethod(methodName)
            {
                ResponseTimeout = TimeSpan.FromSeconds(10)
            }
            .SetPayloadJson(payload);

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, moduleId, methodInvocation);
            var responseString = response.GetPayloadAsJson();

            Console.WriteLine($"\t{response.Status}");

            if (responseString != null)
            {
                // formated string
                Console.WriteLine(JToken.Parse(responseString).ToString());
            }
        }

        // EventHub
        private void ClearData()
        {
            Graphics g = this.panel1.CreateGraphics();
            var b = new SolidBrush(Color.Yellow);
            g.FillRectangle(b, new Rectangle(0, 0, 1280, 960));
            b.Dispose();
            g.Dispose();
        }
        private void DrawLine(float conf, int x1, int y1, int x2, int y2)
        {
            if (conf > 0.0)
            {
                Graphics g = this.panel1.CreateGraphics();
                var p = new Pen(Color.Blue, 3);
                var b = new SolidBrush(Color.Blue);
                var fb = new SolidBrush(Color.White);
                var f = new Font("Consolas", 12);

                string value = $"Reading: {conf:0}";
                g.DrawLine(p, x1, y1, x2, y2);
                g.FillRectangle(b, new Rectangle(10, 10, value.Length * 10, 20));
                g.DrawString(value, f, fb, 10, 10, new StringFormat());
                p.Dispose();
                b.Dispose();
                g.Dispose();
            }
        }

        private void DrawBox(string value, int x, int y, int w, int h)
        {
            Graphics g = this.panel1.CreateGraphics();
            var p = new Pen(Color.Blue, 1);
            var b = new SolidBrush(Color.Blue);
            var fb = new SolidBrush(Color.White);
            var f = new Font("Consolas", 10);

            g.DrawRectangle(p, new Rectangle(x, y, w, h));
            g.FillRectangle(b, new Rectangle(x, y-16, value.Length*10, 16));
            g.DrawString(value, f, fb, x+1, y - 15, new StringFormat());
            p.Dispose();
            b.Dispose();
            g.Dispose();
        }

        private void DrawData(long time, string value, float conf, float l, float t, float w, float h)
        {
            //DateTime dt = UnixTimeToDateTime(time);
            switch (detectMode)
            {
                case MODE.MODE_ANALOGGAUGE:
                    // use conf as value, and use box info for draw line
                    int x1 = (int)l;
                    int y1 = (int)t;
                    int x2 = (int)w;
                    int y2 = (int)h;
                    DrawLine(conf, x1, y1, x2, y2);
                    break;
                case MODE.MODE_OBJECTDETECT:
                    int x = (int)(416 * l * 640 / 416 + 0.5);
                    int y = (int)(416 * t * 480 / 416 + 0.5);
                    int w1 = (int)(416 * w * 640 / 416 + 0.5);
                    int h1 = (int)(416 * h * 480 / 416 + 0.5);
                    DrawBox(value, x, y, w1, h1);
                    break;
            }
        }

        // enable c# language version 8.0
        //https://dirkstrauss.com/enabling-c-8-in-visual-studio-2019/
        private async Task ReceiveMessagesFromDeviceAsync(CancellationToken ct)
        {
            await using var consumer = new EventHubConsumerClient(
                EventHubConsumerClient.DefaultConsumerGroupName,
                eventHubConnectionString,
                eventHubName);

            Console.WriteLine("Listening for messages on all partitions.");

            try
            {
                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(startReadingAtEarliestEvent: false,
                    cancellationToken: ct))
                {
                    ClearData();

                    //Console.WriteLine($"\nMessage received on partition {partitionEvent.Partition.PartitionId}:");
                    string data = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                    if (data.Contains("inferences")) // TODO: need to better handling other message
                    {
                        IoTMessage message = System.Text.Json.JsonSerializer.Deserialize<IoTMessage>(data);
                        //Console.WriteLine($"{message.timestamp} Inference:");
                        foreach (var inf in message.inferences)
                        {
                            DrawData(message.timestamp, inf.entity.tag.value, inf.entity.tag.confidence,
                                inf.entity.box.l, inf.entity.box.t, inf.entity.box.w, inf.entity.box.h);
                        }
                    }

                }
            }
            catch (TaskCanceledException)
            {
                // This is expected when the token is signaled; it should not be considered an
                // error in this scenario.
            }
            finally
            {
                await consumer.CloseAsync();
            }
        }

        private void txtConsole_TextChanged(object sender, EventArgs e)
        {
            txtConsole.SelectionStart = txtConsole.Text.Length;
            txtConsole.ScrollToCaret();
        }

        private void cbxTop_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxTop.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMode.SelectedIndex == 0)
            {
                detectMode = MODE.MODE_ANALOGGAUGE;
            }
            else
            {
                detectMode = MODE.MODE_OBJECTDETECT;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtConsole.Text = "";
        }

        public DateTime UnixTimeToDateTime(double time)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(time).ToLocalTime();
            return dtDateTime;
        }
    }
}
