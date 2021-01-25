# Darknet

[Darknet](https://pjreddie.com/darknet/) is an open source neural network framework written in C and CUDA. It is recommended to use `https://github.com/AlexeyAB/darknet` repo instead of original implementation(`https://github.com/pjreddie/darknet`). AlexeyAB's darknet support both Linux and Windows.

## Build Darknet

Prerequisites:

- GPU-enabled __Linux__ VM/PC.
- Install latest nvidia driver, `cuda` and `cudnn`.
    - for installing cudnn, see this [link](https://docs.nvidia.com/deeplearning/cudnn/install-guide/index.html)

Build:

- Clone and compile [OpenCV](https://github.com/opencv/opencv.git)
- Clone and compile [Darknet(AlexeyAB/darknet)](https://github.com/AlexeyAB/darknet)

Test (from source directory):

```
./darknet detector test cfg/coco.data cfg/yolov3.cfg yolov3.weights data/dog.jpg
```

troubleshooting
    - (1) modify Makefile (set NVCC = /usr/local/cuda/bin/nvcc) and make it as root (sudo make)
        - https://github.com/pjreddie/darknet/issues/1246
    - (2) issue `libcudnn.so.8: cannot open shared object file: No such file or directory`
        ``` 
        export PATH=/usr/local/cuda/bin:${PATH}
        export LD_LIBRARY_PATH=/usr/local/cuda/lib64:${LD_LIBRARY_PATH}
        export CUDA_HOME=/usr/local/cuda
        export LD_LIBRARY_PATH=/usr/local/lib:${LD_LIBRARY_PATH}
        ```
> see this [sample Dockerfile](https://github.com/Azure/live-video-analytics/blob/master/utilities/video-analysis/yolov4-darknet/Dockerfile) for step by step darknet setup. Note that this is **NOT** GPU-supported dockerfile.

## Train Darknet

See [training](training/readme.md)

### Sample Datasets

Mask detection
- https://www.kaggle.com/andrewmvd/face-mask-detection

Hardhat detection (+MOT)
- https://github.com/incheon-kim/yolov4-deepsort-helmet-detection

## Build Darknet docker image

See [darknetbase](darknetbase/readme.md)

## Reference

Darknet/Yolo (AI guy)
- https://www.youtube.com/channel/UCrydcKaojc44XnuXrfhlV8Q/playlists

