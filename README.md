# README

//overview image//

This repo contains some useful resources for Azure LVA (Live Video Analytics)

1. [RTSP](./1_RTSP)

RTSP source is one of important setup for LVA. This section provides how to send RTSP streaming to LVA edge using various tools. 

2. [Edge](./2_Edge)

This section provides a guide for IoT Edge setup and sample [media graphs](https://docs.microsoft.com/en-us/azure/media-services/live-video-analytics-edge/media-graph-concept) for LVA

3. [Darknet](./3_Darknet)

This section provides how to prepare AI-enabled extensions using one of well-known AI framework and object detector, [Darknet](https://pjreddie.com/darknet/).

4. [Extension](./4_Extension)

This section provides sample extensions for LVA.
- [analog gauge reader](./4_Extension/gauge)
- [object detection](./4_Extension/yolov4):
    - yolo: pre-trained yolo3/4 model
    - helmet(hardhat): custom dataset trained model

5. [Test](./5_Test)

Testing live video analytics with _IoT Edge_ is somewhat difficult. This section provides testing tools for LVA. 
- jupyter notebook for testing AI model with video
- Windows desktop app for testing _IoT Edge(LVA)_
