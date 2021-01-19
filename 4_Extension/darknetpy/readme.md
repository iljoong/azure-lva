# Build extension for hardhat detection

> hardhat/helmet: https://github.com/incheon-kim/yolov4-deepsort-helmet-detection

## local setup

- build _darknet_ as libso
    - required libs must be installed
- set path/copy `libdarknet.so` to this path
- download [weights](https://drive.google.com/file/d/1uOWZGx1oR1bRwp_mnvxobaXZcWs1X9ar) file to this path
- run
    ```
    python main.py
    ```
- test
    ```
    curl -X POST -H "Content-type: image/jpeg" --data-binary @"saved-1.jpg" localhost:8089/score
    ```

# build container

> build [darknetbase](../3_Darknet/darknetbase) image and update base image in `Dockerfile` before run this command.

```
docker build -t darknethelmet:latest -f Dockerfile .
```

# run locally

> assume using `nvidia` rundtime.

```
docker run --rm --name darknethelmet -p 8089:8089 -d darknethelmet:latest
```


