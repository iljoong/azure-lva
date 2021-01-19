# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

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

import cv2
import matplotlib.pyplot as plt

from gauge_reader import detectCircle, detectLine, get_current_value
from gauge_utils import drawImage, cropImage, removeShadow

# default value
ROI = [196,98,467,360]
MINMAXMETER = [40, 310, 0, 100]
MINLINELENGTH = 50

def gauge_read(frame):
    # parameters - get this value from env variables
    ## min/max radius for detecting circle
    min_r = 0.35
    max_r = 0.48

    min_angle = MINMAXMETER[0]
    max_angle = MINMAXMETER[1]
    min_value = MINMAXMETER[2]
    max_value = MINMAXMETER[3]
    minLineLength = MINLINELENGTH

    cx1 = ROI[0]
    cy1 = ROI[1]
    cx2 = ROI[2]
    cy2 = ROI[3]

    try:
        img = frame[cy1:cy2, cx1:cx2]
        img = removeShadow(img)
        x, y, r = detectCircle(img, min_r, max_r, False, False)

        # fine tune parameter minLineLength = 50 (default 100)
        final_line_list = detectLine(img, x, y, r, minLineLength, False, False, False)

        if (len(final_line_list) > 0):
            value = get_current_value(img, final_line_list, min_angle, max_angle, min_value, max_value, x, y, r)
            x1 = final_line_list[0][0]+cx1
            y1 = final_line_list[0][1]+cy1
            x2 = final_line_list[0][2]+cx1
            y2 = final_line_list[0][3]+cy1

            return value, [(x1, y1), (x2, y2)]
        else:
            return -1, [(0, 0), (0, 0)]
    except Exception as e:
        return -1, [(0, 0), (0, 0)]

def process_image(image_data):
    inference_time_start = time.time()

    value, linePt = gauge_read(image_data)
    
    inference_time_end = time.time()
    inference_duration_s = inference_time_end - inference_time_start
    
    read = [{
        "type": "entity",
        "entity": {
            "tag": {
                "value" : value,
                "confidence": float(value),
            },
            "box" : {
                "l" : int(linePt[0][0]),
                "t" : int(linePt[0][1]),
                "w" : int(linePt[1][0]),
                "h" : int(linePt[1][1])
            }
        }
    }]

    return read, inference_duration_s

app = Flask(__name__)

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
    read, _ = process_image(image)

    respBody = {
        "inferences" : read
    }

    respBody = json.dumps(respBody)
    return Response(respBody, status= 200, mimetype ='application/json')

if __name__ == '__main__':

    # environment variables
    roi_s = os.getenv("ROI")
    if (roi_s != None):
        roi_a = roi_s.split(",")
        ROI = [int(s) for s in roi_a]
        print(ROI)

    meter_s = os.getenv("MINMAXMETER")
    if (meter_s != None):
        meter_a = meter_s.split(",")
        MINMAXMETER = [int(s) for s in meter_a]
        print(MINMAXMETER)

    minl_s = os.getenv("MINLINELENGTH")
    if (minl_s != None):
        MINLINELENGTH = int(minl_s)
        print(MINLINELENGTH)

    # Running the file directly
    app.run(host='0.0.0.0', port=8088)
