# LVA Edge & IoTEdge

## Setup

Please refer [Get Started](https://docs.microsoft.com/en-us/azure/media-services/live-video-analytics-edge/get-started-detect-motion-emit-events-quickstart).

You would need following Azure services and edge for LVA.

- AAD service principal
- Azure Media Service
- IoT hub
- ACR
- Edge device (Azure VM, Azure Stack Edge or Laptop)

For detailed setup see following scripts
- [setup overview](https://github.com/Azure/live-video-analytics/tree/master/edge/setup)
    - [setup script](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/setup.sh)
    - [Azure deployment template](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/deploy.json)
    - [cloud-init for VM setup](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/cloud-init.yml)

## Edge setup

> Ubuntu 18.04

- NVIDIA driver, CUDA, CuDNN
- Docker, Nvidia-docker
- Directories: `/var/media, /var/local/azuremediaservices`

> NOTE: `lvaEdge` module version 2 has been changed!! You need to create directories (`/var/media, /var/local/azuremediaservices`) on edge devices. See [cloud-init](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/cloud-init.yml) of edge device for more details.

##  Deploy

Deploy LVA modules to iot edge.

```
az iot edge set-modules --hub-name <hubname> --device-id <device> --content [deployment.motion-detection.json](./deployment.motion-detection.json)
```

