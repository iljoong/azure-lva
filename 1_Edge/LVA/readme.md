# LVA Edge & IoTEdge

## Setup

Please refer [Get Started](https://docs.microsoft.com/en-us/azure/media-services/live-video-analytics-edge/get-started-detect-motion-emit-events-quickstart).

You would need following Azure services and edge for LVA.

> You don't need to setup _edge device_ on Azure Stack Edge. IoT edge module automatically configured with ASE settings and you can link/unlink an IoT hub in the ASE settings.

- AAD service principal (for accessing AMS API)
- Azure Media Service
- IoT hub
- ACR
- Edge device (Azure VM or  Laptop)

### Edge device setup

For VM (Ubuntu 18.04), you need to install followings:
- NVIDIA driver, CUDA, CuDNN
- Docker, Nvidia-docker
- Directories: `/var/media, /var/local/azuremediaservices`

> NOTE: `lvaEdge` module version 2 has been changed!! You need to create directories (`/var/media, /var/local/azuremediaservices`) on edge devices. See [cloud-init](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/cloud-init.yml) of edge device for more details.

For detailed setup see following scripts
- [setup overview](https://github.com/Azure/live-video-analytics/tree/master/edge/setup)
    - [setup script](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/setup.sh)
    - [Azure deployment template](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/deploy.json)
    - [cloud-init for VM setup](https://github.com/Azure/live-video-analytics/blob/master/edge/setup/cloud-init.yml)

##  Deploy

1. Deploy base LVA modules to iot edge for httpext.

    ```
    az iot edge set-modules --hub-name <hubname> --device-id <device> --content deployment.httpext.json
    ```

2. Deploy additional edge extension such as _rtsp server_ or _AI extensions_. You can easily add new modules using _IoT Edge_ settings in the Azure portal.

3. Setup graph topology and instance using direct method. See [httpext setup](./httpext/Setup.md) for more information.

