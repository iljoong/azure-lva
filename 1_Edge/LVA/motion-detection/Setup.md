## IoT Direct Method

> Reference: https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-direct-methods

Get SAS key using azure cli

```
az iot hub generate-sas-token -n <iothub_name>
```

Sample direct method

```powershell
# saskey
$sas = $(az iot hub generate-sas-token -n <iothub_name> | ConvertFrom-Json)
$url = "https://<iothub-name>.azure-devices.net/twins/<device>/modules/<module>/methods?api-version=2018-06-30"

#GraphTopologySet
curl -X POST $url -H "Authorization: $($sas.sas)" -H 'Content-Type: application/json' `
  -d "@1_GraphTopologySet.json" | jq

#GraphTopologyGet
curl -X POST $url -H "Authorization: $($sas.sas)" -H 'Content-Type: application/json' `
  -d "@1_GraphTopologyGet.json" | jq

#GraphInstanceSet
curl -X POST $url -H "Authorization: $($sas.sas)" -H 'Content-Type: application/json' `
  -d "@2_GraphInstanceSet.json"

#GraphInstanceActivate
curl -X POST $url -H "Authorization: $($sas.sas)" -H 'Content-Type: application/json' `
  -d "@3_GraphInstanceActivate.json"

#GraphInstanceDeactivate
curl -X POST $url -H "Authorization: $($sas.sas)" -H 'Content-Type: application/json' `
  -d "@4_GraphInstanceDeactivate.json"

```
