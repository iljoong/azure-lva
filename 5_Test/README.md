# LVA Test

## Notebook

Sample jupyter notebook for testing LVA.

## Desktop App

Desktop app for visualizing and managing LVA.

![gaugeread](./gaugeread.png)

### Run

> Require _Visual Studio 2019 (any edition)_ to build this app.

Prerequisites:
    - Deploy AI-enabled media graph topology and instance. see this [link](./2_Edge/mediagraph/httpext) for more info. 

1. update `appsettings.json` in source
2. build source
3. start __ffplay__ to monitor RTSP source
4. Run app
    - move the app on top of the __ffplay__.
    - click `Instance Get` to check the overall status.
    - click `Instance Set` if you need to change the settings.
    - click `Start Monitor` to enable monitoring.
    - click `Activate` to start LVA instance.
    - click `Deactivate` if you want to stop LVA instance. 