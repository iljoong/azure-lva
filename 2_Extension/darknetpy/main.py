from datetime import datetime
import io
import json
import os
import logging
import time
from typing import Tuple

from flask import Flask, Response, Request, abort, request
import numpy as np
from PIL import Image, ImageDraw, ImageFont

import darknet

import cv2
import matplotlib.pyplot as plt

netMain = None
metaMain = None
imgSize = 416.0

class YoloV4Model:
    def __init__(self):
        configPath = "./yolov4-helmet-detection.cfg"
        weightPath = "./yolov4-helmet-detection.weights"
        metaPath = "./yolov4-helmet-detection.data"

        self.netMain = darknet.load_net_custom(configPath.encode("ascii"), weightPath.encode("ascii"), 0, 1)  # batch size = 1
        self.metaMain = darknet.load_meta(metaPath.encode("ascii"))

    def process_image(self, image_data):
        results = []
        inference_duration_s = 0

        try:
            inference_time_start = time.time()
            darknet_image = darknet.make_image(darknet.network_width(self.netMain),
                                            darknet.network_height(self.netMain),3)

            darknet.copy_image_from_bytes(darknet_image, image_data.tobytes())

            detections = darknet.detect_image(self.netMain, self.metaMain, darknet_image, thresh=0.25)
            
            inference_time_end = time.time()
            inference_duration_s = inference_time_end - inference_time_start

            for d in detections:
                w = d[2][2]
                h = d[2][3]
                r = {
                    "type": "entity",
                    "entity": {
                        "tag": {
                            "value" : d[0].decode('utf-8'),
                            "confidence": round(d[1], 4),
                        },
                        "box" : {
                            "l" : (d[2][0]-w/2)/imgSize,
                            "t" : (d[2][1]-h/2)/imgSize,
                            "w" : d[2][2]/imgSize,
                            "h" : d[2][3]/imgSize
                        }
                    }
                }
                results.append(r)
        except Exception as e:
            # todo: log
            print("Error: ", e)

        return results, inference_duration_s

app = Flask(__name__)
model = YoloV4Model()

# / routes to the default function
@app.route('/', methods=['GET'])
def default_page():
    return Response(response='Ping', status=200)

# /score routes to scoring function 
@app.route("/score", methods=['POST'])
def score():

    nparr = np.frombuffer(request.data, np.uint8)
    # decode image
    image = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    results, _ = model.process_image(image)

    respBody = {
        "inferences" : results
    }

    respBody = json.dumps(respBody)
    return Response(respBody, status= 200, mimetype ='application/json')

if __name__ == '__main__':

    # Running the file directly
    app.run(host='0.0.0.0', port=8089)
