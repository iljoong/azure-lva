## NOTE

Output of LVA

Video streaming initiated

```
{
  "sdp": "SDP:\nv=0\r\no=-0 0 IN IP4 127.0.0.1\r\ns=No Name\r\nc=IN IP4 0.0.0.0\r\nt=0 0\r\na=control:*\r\nm=video 0 RTP/AVP 96\r\nb=AS:189\r\na=rtpmap:96 H264/90000\r\na=fmtp:96 packetization-mode=1; spr=fmtp:96 packetization-mode=1; sprop-parameter-sets=J2QAHqxWwKA9pqAgIMBA,KO48sA==; profile-level-id=64001E\r\na=control:trackID=0\r\n"
}
```

Inference

> See [inference metadata schema](https://docs.microsoft.com/en-us/azure/media-services/live-video-analytics-edge/inference-metadata-schema) for more info

```
{
  "timestamp": 144785629130765,
  "inferences": [
    {
      "type": "entity",
      "entity": {
        "tag": {
          "value": "person",
          "confidence": 0.9895599
        },
        "box": {
          "l": 0.28072026,
          "t": 0.12533382,
          "w": 0.61489147,
          "h": 0.7449602
        }
      }
    },
    {
      "type": "entity",
      "entity": {
        "tag": {
          "value": "laptop",
          "confidence": 0.9647765
        },
        "box": {
          "l": 0.00057433546,
          "t": 0.5581506,
          "w": 0.44293007,
          "h": 0.32075846
        }
      }
    }
  ]
}

```