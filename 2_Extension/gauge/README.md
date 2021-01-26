# README

Sample live detection app (LVA extension) for analog gauge

## Test

open [Video-gauge-reader.ipynb](Video-gauge-reader.ipynb) notebook.

Use [sample video](./sample/sample.avi) or use live video stream from your built-in cam or RTSP server.
If you're using live stream then use [simulated analog gauge](./gauge.html). Click up/down/left/right to move gauge.

## Test for LVA extension

Dockerize the app for gauge reader .

```
docker build -t gaugereader:latest -f Dockerfile .
```

Run and test locally.

```
docker run --name gaugereader -e "ROI=196,98,467,360" -e "MINMAXMETER=40,310,0,100" -e "MINLINELENGTH=50" -p 8080:8080 -d gaugereader

curl -X POST -H "Content-type: image/jpeg" --data-binary @"captured.jpg" localhost:8080/score 
```

Change ROI (region of interest)

```
curl -X PUT -H "Content-Type: application/json" -d "{\"cx1\": 200, \"cy1\": 100, \"cx2\": 470, \"cy2\": 3600}" localhost:8088/api/roi
```

## Reference

- analog reader: https://github.com/intel-iot-devkit/python-cv-samples/tree/master/examples/analog-gauge-reader
- chart: https://developers.google.com/chart/interactive/docs/gallery/gauge
