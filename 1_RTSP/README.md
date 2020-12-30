# RTSP for LVA

There are various ways to send RTSP source to LVA.

```
[Camera]--->[RTSP app]--->[RTSP server]--->[LVA]
```

## RTSP Streaming

### RSTP Simulator

https://github.com/Azure/live-video-analytics/tree/master/utilities/rtspsim-live555

### Windows

download [rtsp server](https://github.com/aler9/rtsp-simple-server), config and start rtsp server.
```
rtsp-simple-sever.exe
```

download [ffmpeg](https://ffmpeg.org/download.html) and stream video to rtsp server
```
ffmpeg -f dshow -i video="Microsoft Camera Rear" -filter:v scale=320:-1 -f rtsp -rtsp_transport tcp rtsp://localhost:8554/visual

# fps to 10/resize
ffmpeg -f dshow -i video="Microsoft Camera Front" -vf "fps=fps=10, scale=320:-1" -f rtsp -rtsp_transport tcp rtsp://localhost:8554/visual
```

### Linux

https://github.com/Azure/live-video-analytics/tree/master/utilities/USB-to-RTSP

```
docker run --name usb-to-rtsp --device=/dev/video0 --env VIDEO_PIPELINE="v4l2src device=/dev/video0 ! videoconvert ! videoscale! video/x-raw ! x264enc tune=zerolatency ! rtph264pay name=pay0" -p 554:8554 -d -i usb-to-rtsp:v1  
```

### Mobile App

> This is the recommended option for sending RTSP source to LVA

Install [Live Reporter app](https://apps.apple.com/us/app/live-reporter-live-camera/id996017825).
This app can send video stream to RTSP server but also runs as RTSP server.

## Play RTSP source

use `ffplay` for play from rtsp server 

```
ffplay -framedrop -probesize 32 -sync ext -analyzeduration 0 -rtsp_transport tcp rtsp://{source}
```
