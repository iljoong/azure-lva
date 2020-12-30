# README

Win Desktop App for Debugging LVA

- Config media graph
	- set/get instance
	- activate/deactivate instance

- Support for followin detection
	- Analog gauge reading
	- Object dection (Yolo)

- Monitor and visualize LVA result from event hub
	- visualize reading/detection on top of overlayed video streaming 

## Setup

- Update appsettings.json
- Play RTSP video streaming, such as ffplay
	```
	ffplay -framedrop -probesize 32 -sync ext -analyzeduration 0 -rtsp_transport tcp rtsp://192.168.50.72
	```
- Start this app and overlay on top of video stream.
- Click `Start Monitor` to enable monitoring then click `Activate` to start LVA. You may configure graph instance by clicking `Instance Set`.

[add picture]

## Reference

- Application

dotnet issue: https://github.com/dotnet/sdk/issues/14916
    => fix: set-alias dotnet 'C:\Program Files\dotnet\dotnet.exe"

- IOT

servicebus explorer: https://github.com/paolosalvatori/ServiceBusExplorer

iot sample: https://github.com/Azure-Samples/azure-iot-samples-csharp/blob/master/iot-hub/Quickstarts/

https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/eventhub/Azure.Messaging.EventHubs/samples/Sample05_ReadingEvents.md

- Dotnet

Console output: http://www.csharp411.com/console-output-from-winforms-application/

Thread(Task): https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation

json serialize/deserialize: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0