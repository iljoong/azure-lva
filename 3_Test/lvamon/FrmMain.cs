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
using WindowsFormsApp1;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace lvamon
{
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
        static string frameWidth, frameHeight;
        static string[] _scaleMode = { "preserveAspectRatio", "pad"};

        static List<LvaScenario> lvaScenarios = new List<LvaScenario>();
        static int selectedScenario = 0;
        static int detectMode = 0;

        static bool appMonitoring = false;

        static int HIDPI = 1;

        public MainForm()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");

#if DEBUG
            // NOTE: For debug, add user secrets before run this app
            // credential https://medium.com/@granthair5/how-to-add-and-use-user-secrets-to-a-net-core-console-app-a0f169a8713f
            builder.AddUserSecrets<lvaSecrets>();
#endif
            var appSettings = builder.Build();

            // IoT Hub settings
            serviceClient = ServiceClient.CreateFromConnectionString(appSettings["IoThubConnectionString"]);
            deviceId = appSettings["deviceId"];
            moduleId = appSettings["moduleId"];

            // Eventhub settings
            eventHubConnectionString = appSettings["eventHubConnectionString"];
            eventHubName = appSettings["eventHubName"];

            // support hidpi screen
            HIDPI = (appSettings["hidpi"].ToLower() == "true") ? 2 : 1;

            // get user scenario
            var section = appSettings.GetSection("scenario");
            foreach (var s in section.GetChildren())
            {
                LvaScenario sc = new LvaScenario();
                sc.name = s["name"];
                sc.rtspSource = s["rtspSource"];
                sc.aiUri = s["aiUri"];
                sc.aiMode = s["aiMode"];
                sc.scaleMode = s["scaleMode"];
                sc.frameWidth = s["frameWidth"];
                sc.frameHeight = s["frameHeight"];

                lvaScenarios.Add(sc);
            }

            // default mode = analog gauge
            cmbMode.SelectedIndex = 0;
            btnStop.Enabled = false;

            TextBoxWriter writer = new TextBoxWriter(txtConsole);
            Console.SetOut(writer);
        }

        private async void btnTopology_Click(object sender, EventArgs e)
        {
            var dlgTopology = new DlgTopology();
            if (dlgTopology.ShowDialog() == DialogResult.OK)
            {
                await InvokeMethodWithPayloadAsync("GraphTopologySet", dlgTopology.topology);
            }
        }

        private int getAiModeIndex(string aimode)
        {
            switch (aimode)
            {
                case "analogGauge":
                    return 0;
                case "ojbectdetection":
                    return 1;
                case "helmetdetection":
                    return 2;
            }

            return 0;
        }
        private async void btnInstSet_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnInstSet_Click");

            var frm = new frmInstProp();
            frm.scenarios = lvaScenarios;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                selectedScenario = frm.cmbScenario.SelectedIndex;
                cmbMode.SelectedIndex = getAiModeIndex(lvaScenarios[selectedScenario].aiMode);
                detectMode = cmbMode.SelectedIndex;

                rtspSource = frm.txtRTSPsource.Text;
                aiUrl = frm.txtAIUrl.Text;
                scaleMode = _scaleMode[frm.cmbScale.SelectedIndex];
                frameWidth = frm.txtWidth.Text;
                frameHeight = frm.txtHeight.Text;

                // TODO: ability to add new scenario and update to 'appsettings.json' file

                Console.WriteLine("properties changed");

                // TODO: use different instance name for scenario

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

            JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
            JObject jobj = new JObject{
                { "name", "AI-Graph-1" }
            };

            await ExecuteGraphOperationAsync(jobj, "GraphInstanceGet", apiVersionProperty);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnStart_Click");
            appMonitoring = true;
            btnStart.Enabled = !appMonitoring;
            btnStop.Enabled = appMonitoring;

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
            appMonitoring = true;
            btnStart.Enabled = appMonitoring;
            btnStop.Enabled = !appMonitoring;

            cancellationTokenSource.Cancel();
        }

        private async void btnActivate_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnActivate_Click");

            JProperty apiVersionProperty = new JProperty("@apiVersion", "2.0");
            JObject jobj = new JObject{
                { "name", "AI-Graph-1" }
            };

            await ExecuteGraphOperationAsync(jobj, "GraphInstanceActivate", apiVersionProperty);
        }

        private async void btnDeactivate_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnDeactivate_Click");

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
                ResponseTimeout = TimeSpan.FromSeconds(60)
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
            // HIDPI
            x1 *= HIDPI;
            y1 *= HIDPI;
            x2 *= HIDPI;
            y2 *= HIDPI;

            if (conf > 0.0)
            {
                Graphics g = this.panel1.CreateGraphics();
                var p = new Pen(Color.Blue, 3);
                var b = new SolidBrush(Color.Blue);
                var fb = new SolidBrush(Color.White);
                var f = new Font("Consolas", 12);

                string value = $"Reading: {conf:0}";
                g.DrawLine(p, x1, y1, x2, y2);
                g.FillRectangle(b, new Rectangle(10*HIDPI, 10*HIDPI, value.Length * 10*HIDPI, 20*HIDPI));
                g.DrawString(value, f, fb, 10*HIDPI, 10*HIDPI, new StringFormat());
                p.Dispose();
                b.Dispose();
                g.Dispose();
            }
        }

        private void DrawBox(string value, int x, int y, int w, int h, bool red = false)
        {
            // HIDPI
            x *= HIDPI;
            y *= HIDPI;
            w *= HIDPI;
            h *= HIDPI;

            Graphics g = this.panel1.CreateGraphics();
            var fb = new SolidBrush(Color.White);
            var f = new Font("Consolas", 10);

            SolidBrush b;
            Pen p;
            if (red)
            {
                p = new Pen(Color.Red, 1);
                b = new SolidBrush(Color.Red);
            }
            else
            {
                p = new Pen(Color.Blue, 1);
                b = new SolidBrush(Color.Blue);
            }

            g.DrawRectangle(p, new Rectangle(x, y, w, h));
            g.FillRectangle(b, new Rectangle(x, y-16*HIDPI, value.Length*10*HIDPI, 16*HIDPI));
            g.DrawString(value, f, fb, x+1*HIDPI, y - 15*HIDPI, new StringFormat());
            p.Dispose();
            b.Dispose();
            g.Dispose();
        }

        private void DrawHelmet(string value, int x, int y, int w, int h)
        {
            if (value == "helmet")
            {
                DrawBox(value, x, y, w, h);
            }
            else
            {
                DrawBox(value, x, y, w, h, true);
            }
        }

        private void DrawData(long time, string value, float conf, float l, float t, float w, float h)
        {
            switch (detectMode)
            {
                case 0:
                    // use conf as value, and use box info for draw line
                    int x1 = (int)l;
                    int y1 = (int)t;
                    int x2 = (int)w;
                    int y2 = (int)h;
                    DrawLine(conf, x1, y1, x2, y2);
                    break;
                case 1:
                    {
                        int x = (int)(416 * l * 640 / 416 + 0.5);
                        int y = (int)(416 * t * 480 / 416 + 0.5);
                        int w1 = (int)(416 * w * 640 / 416 + 0.5);
                        int h1 = (int)(416 * h * 480 / 416 + 0.5);
                        DrawBox(value, x, y, w1, h1);
                    }
                    break;
                case 2:
                    {
                        int x = (int)(416 * l * 640 / 416 + 0.5);
                        int y = (int)(416 * t * 480 / 416 + 0.5);
                        int w1 = (int)(416 * w * 640 / 416 + 0.5);
                        int h1 = (int)(416 * h * 480 / 416 + 0.5);
                        DrawHelmet(value, x, y, w1, h1);
                    }
                    break;
            }
        }

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
                    else if (data.Contains("code")) // error
                    {
                        ErrorMessage message = System.Text.Json.JsonSerializer.Deserialize<ErrorMessage>(data);
                        Console.WriteLine($"Error: {message.code}, {message.target}");
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
            detectMode = cmbMode.SelectedIndex;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtConsole.Text = "";
        }

        private void btnExe_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "ffplay.exe";
            startInfo.Arguments = $"-fflags nobuffer {lvaScenarios[selectedScenario].rtspSource}";
            p.StartInfo = startInfo;
            p.Start();
        }
    }
}
