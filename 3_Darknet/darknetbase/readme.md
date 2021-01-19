# Darknet base image

This folder contains a `Dockerfile` that how to build a base image with darknet library for AI application

## Build

To build locally,

```
docker build -t darknetbase:latest -f Dockerfile .
```

To build using ACR,

```
az acr build -r <acrname> -t <acrname>.azurecr.io/darknetbase:latest .
```

## Build note

This dockerfile uses multi-stage builds to optimize image size. It will produce 7 GB of image size if you're build with just `devel` image but it will produce only 3 GB of size using multi-stage build.

For more information about multi-stage build, see this [link](https://docs.docker.com/develop/develop-images/multistage-build/)

## Container tips (for troubleshooting):

- use interactive run:
    ```
    docker run --rm -it -p 8080:8080 --entrypoint bash <image>
    ```
    you can troubleshoot GPU(`nvidia-smi`), app and others.
    
    for kubernetes,
    ```
    kubectl exec -it <pod_id> -n <ns> -- /bin/sh
    ```
- copy local file to/from container
    ```
    docker cp <local_file> <containerid>:<container_path>
    ```

    for kubernetes,
    ```
    kubectl cp <local_file> <ns>/<pod_id>:<pod_path>
    ```
    > ignore some warnings.
