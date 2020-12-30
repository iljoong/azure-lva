# How to build this sample darknet app

> Original source from this [link](https://github.com/Azure/live-video-analytics/tree/master/utilities/video-analysis/yolov4-darknet/lvaextension)

## Prerequisites

- Build [Darknet](https://github.com/AlexeyAB/darknet) library.
    > see this [readme](../3_Darknet/README.md) for more information.
    - Use `LIBSO=1` to get `libdarknet.so`. You would need this file to link to executable file.  

- Prepare _name_, _cfg_ and _weights_ files.
    - You can copy `coco.names` and `yolov3.cfg` file in `darknet/cfg` folder.
    - You need to download `yolov3.weights` from this (link)[https://pjreddie.com/media/files/yolov3.weights].

## Build

- Update the path of `darknet/include/yolo_v2_class.hpp` in `Makefile` and `yolov4.h`.
- Run `make` to build

## Test

- Run
    ```bash
export NAME_FILE="coco.names"
export CFG_FILE="yolov3.cfg"
export WEIGHTS_FILE="yolov3.weights"

    ./lvaextension
    ```
- Test
    ```bash
    curl -X POST -H "Content-type: image/jpeg" --data-binary @"people.jpg" localhost:44000/score

    {"inferences":[{"type":"entity","entity":{"tag":{"value": "person","confidence":0.994922},"box":{"l":0.526442,"t":0.189904,"w":0.209135,"h":0.656250}}},
    ...}
    ```

## Docker

Build (after completed test)

```bash
docker build -t lvaextension:latest .
```

Run yolov3

```bash
docker run --name lvaext -e "NAME_FILE=coco.names" -e "FG_FILE=yolov3.cfg" - "WEIGHTS_FILE=yolov3.weights" -p 44000:44000 -d lvaextension:latest
```

Run other model

```bash
docker run --name lvaext -e "NAME_FILE=/data/helmet.names" -e "CFG_FILE=/data/helmet.cfg" -e "WEIGHTS_FILE=/data/helmet.weights" -p 44000:44000 -v  <model-location>:/data -d lvaextension:latest
```
