> This is modified version of yolov3-onnx to support GPU. Original source is located in this [link](https://github.com/Azure/live-video-analytics/tree/master/utilities/video-analysis/yolov3-onnx)

# YOLOv3 and Tiny YOLOv3 ONNX models

The following instructions will enable you to build a Docker container with a [YOLOv3](http://pjreddie.com/darknet/yolo/) [ONNX](http://onnx.ai/) model or a Tiny YOLOv3 ONNX model using [nginx](https://www.nginx.com/), [gunicorn](https://gunicorn.org/), [flask](https://github.com/pallets/flask), [runit](http://smarden.org/runit/), and [pillow](https://pillow.readthedocs.io/en/stable/index.html).

Note: References to third-party software in this repo are for informational and convenience purposes only. Microsoft does not endorse nor provide rights for the third-party software. For more information on third-party software please see the links provided above.

## Contributions needed

* Improved logging
* Graceful shutdown of nginx and gunicorn

## Prerequisites

1. [Install Docker](http://docs.docker.com/docker-for-windows/install/) on your machine
2. Install [curl](http://curl.haxx.se/)

## Building the Docker container

To build the container image locally, run the following Docker command from a terminal in that directory. Choose between the full YOLOv3 model (237 MB) or the lightweight Tiny YOLOv3 model (34 MB). The process should take a few minutes to complete. 

YOLOv3:
```bash
    docker build -f yolov3.dockerfile . -t lvaextension:http-yolov3-onnx-v1.0
```

Tiny YOLOv3:
```bash
    docker build -f yolov3-tiny.dockerfile . -t lvaextension:http-yolov3-tiny-onnx-v1.0
```

> <span> [!TIP] </span>  
> If you do not wish to build the local Dockerfile, you may pull it off of Microsoft Container Registry and skip the following step <br>
> `docker run --name my_yolo_container -p 8080:80 -d  -i mcr.microsoft.com/lva-utilities/lvaextension:http-yolov3-onnx-v1.0` for the full YOLOv3 model or <br>
> `docker run --name my_yolo_container -p 8080:80 -d  -i mcr.microsoft.com/lva-utilities/lvaextension:http-yolov3-tiny-onnx-v1.0` for the Tiny YOLOv3 model.


## Running and testing
> <span style="color:red; font-weight: bold"> [!IMPORTANT] </span>  
> The REST endpoint accepts images with size 416 px x 416 px. Since the LVA Edge module is already capable of sending specifically sized images in specific formats, we do not need to preprocess the incoming images, thus decreasing the computational time needed to run the model.

Run the container image using either of the following Docker commands:

YOLOv3:
```bash
    docker run --name my_yolo_container -p 8080:80 -d  -i lvaextension:http-yolov3-onnx-v1.0
```

Tiny YOLOv3:
```bash
    docker run --name my_yolo_container -p 8080:80 -d  -i lvaextension:http-yolov3-tiny-onnx-v1.0
```

Note that you can use any host port that is available instead of 8080.

Test the container using the following commands:

### /score

To get a list of detected objects using the following command

```bash
   curl -X POST http://127.0.0.1:8080/score -H "Content-Type: image/jpeg" --data-binary @<image_file_in_jpeg>
```

If successful, you will see JSON printed on your screen that looks something like this

```JSON
{
    "inferences": [
        {
            "type": "entity",
            "entity": {
                "tag": {
                    "value": "person",
                    "confidence": 0.959613
                },
                "box": {
                    "l": 0.692427,
                    "t": 0.364723,
                    "w": 0.084010,
                    "h": 0.077655
                }
            }
        },
        {
            "type": "entity",
            "entity": {
                "tag": {
                "value": "vehicle",
                "confidence": 0.929751
                },
                "box": {
                    "l": 0.521143,
                    "t": 0.446333,
                    "w": 0.166306,
                    "h": 0.126898
                }
            }
        }
    ]
}
```

#### Filter objects of interest

To filter the list of detected objects use the following command

```bash
   curl -X POST "http://127.0.0.1:8080/score?object=<objectType>" -H "Content-Type: image/jpeg" --data-binary @<image_file_in_jpeg>
```

The above command will only return objects of objectType

#### Filter objects of interest above a confidence threshold

To filter the list of detected objects above a certain confidence threshold use the following command

```bash
   curl -X POST "http://127.0.0.1:8080/score?object=<objectType>&confidence=<confidenceThreshold>" -H "Content-Type: image/jpeg" --data-binary @<image_file_in_jpeg>
```

In the above command, confidenceThreshold should be specified as a float value.

#### View video stream with inferencing overlays

You can use the container to process video and view the output video with inferencing overlays in a browser. To do so, you can post video frames to the container with stream=<stream-id> as a query parameter (i.e. you will need to post video frames to [http://127.0.0.1:8080/score?stream=test-stream](http://127.0.0.1/score?stream=test-stream). The output video can then be viewed by going to [http://127.0.0.1:8080/stream/test-stream](http://127.0.0.1:8080/stream/test-stream).

### /annotate

To see the bounding boxes overlaid on the image run the following command

```bash
   curl -X POST http://127.0.0.1:8080/annotate -H "Content-Type: image/jpeg" --data-binary @<image_file_in_jpeg> --output out.jpeg
```

If successful, you will see a file out.jpeg with bounding boxes overlaid on the input image.

### /score-debug

To get the list of detected objects and also generate an annotated image run the following command

```bash
   curl -X POST http://127.0.0.1:8080/score-debug -H "Content-Type: image/jpeg" --data-binary @<image_file_in_jpeg>
```

If successful, you will see a list of detected objected in JSON. The annotated image will be generated in the /app/images directory inside the container. You can copy the images out to your host machine by using the following command

```bash
    docker cp my_yolo_container:/app/images ./
```

> <span> [!NOTE] </span>  
> The entire /images folder will be copied to ./images on your host machine. Image files have the following format dd_mm_yyyy_HH_MM_SS.jpeg

## Terminating

Terminate the container using the following Docker commands

```bash
    docker stop my_yolo_container
    docker rm my_yolo_container
```

## Upload Docker image to Azure container registry

Follow instruction on the `/utilities/video-analysis/readme.md`, in the section [Instructions on pushing the container image to Azure Container Registry](../readme.md#instructions-on-pushing-the-container-image-to-azure-container-registry).

## Deploy as an Azure IoT Edge module

Follow instruction in [Deploy module from Azure portal](https://docs.microsoft.com/en-us/azure/iot-edge/how-to-deploy-modules-portal) to deploy the container image as an IoT Edge module (use the IoT Edge module option).
