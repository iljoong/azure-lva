from datetime import datetime
import io
import json
import os
import logging

from flask import Flask, Response, Request, abort, request, jsonify

from keras import backend as K
from keras.models import load_model
from keras.preprocessing import image
from keras.optimizers import Adam
from imageio import imread
import numpy as np
from matplotlib import pyplot as plt
import cv2
import time

from models.keras_ssd300 import ssd_300
from keras_loss_function.keras_ssd_loss import SSDLoss
from keras_layers.keras_layer_AnchorBoxes import AnchorBoxes
from keras_layers.keras_layer_DecodeDetections import DecodeDetections
from keras_layers.keras_layer_DecodeDetectionsFast import DecodeDetectionsFast
from keras_layers.keras_layer_L2Normalization import L2Normalization

from ssd_encoder_decoder.ssd_output_decoder import decode_detections, decode_detections_fast

from data_generator.object_detection_2d_data_generator import DataGenerator
from data_generator.object_detection_2d_photometric_ops import ConvertTo3Channels
from data_generator.object_detection_2d_geometric_ops import Resize
from data_generator.object_detection_2d_misc_utils import apply_inverse_transforms

import tensorflow as tf

# note: https://stackoverflow.com/questions/51127344/tensor-is-not-an-element-of-this-graph-deploying-keras-model
class SSDModel:
    def __init__(self):

        self.img_height = 300
        self.img_width = 300
        self.classes = ['background','crack','noncrack']
        
        self.load_weight()
        
    def load_model(self): 
        
        self.model_path = './ssd_keras_crack.h5'

        # We need to create an SSDLoss object in order to pass that to the model loader.
        ssd_loss = SSDLoss(neg_pos_ratio=3, n_neg_min=0, alpha=1.0)

        K.clear_session() # Clear previous models from memory.

        self.session = tf.Session()
        self.graph = tf.get_default_graph()

        with self.graph.as_default():
            with self.session.as_default():
                self.model = load_model(self.model_path, custom_objects={'AnchorBoxes': AnchorBoxes,
                                                            'L2Normalization': L2Normalization,
                                                            'DecodeDetections': DecodeDetections,
                                                            'compute_loss': ssd_loss.compute_loss})

    def load_weight(self): 
        
        self.weights_path = 'ssd300_pascal_07+12_epoch-08_loss-1.9471_val_loss-1.9156.h5'

        adam = Adam(lr=0.001, beta_1=0.9, beta_2=0.999, epsilon=1e-08, decay=0.0)
        ssd_loss = SSDLoss(neg_pos_ratio=3, alpha=1.0)
                
        K.clear_session() # Clear previous models from memory.

        self.session = tf.Session()
        self.graph = tf.get_default_graph()

        with self.graph.as_default():
            with self.session.as_default():
                self.model = ssd_300(image_size=(self.img_height, self.img_width, 3),
                                n_classes=2,
                                mode='inference',
                                l2_regularization=0.0005,
                                scales=[0.1, 0.2, 0.37, 0.54, 0.71, 0.88, 1.05], # The scales for MS COCO are [0.07, 0.15, 0.33, 0.51, 0.69, 0.87, 1.05]
                                aspect_ratios_per_layer=[[1.0, 2.0, 0.5],
                                                         [1.0, 2.0, 0.5, 3.0, 1.0/3.0],
                                                         [1.0, 2.0, 0.5, 3.0, 1.0/3.0],
                                                         [1.0, 2.0, 0.5, 3.0, 1.0/3.0],
                                                         [1.0, 2.0, 0.5],
                                                         [1.0, 2.0, 0.5]],
                                two_boxes_for_ar1=True,
                                steps=[8, 16, 32, 64, 100, 300],
                                offsets=[0.5, 0.5, 0.5, 0.5, 0.5, 0.5],
                                clip_boxes=False,
                                variances=[0.1, 0.1, 0.2, 0.2],
                                normalize_coords=True,
                                subtract_mean=[123, 117, 104],
                                swap_channels=[2, 1, 0],
                                confidence_thresh=0.5,
                                iou_threshold=0.45,
                                top_k=200,
                                nms_max_output_size=400)


                self.model.load_weights(self.weights_path, by_name=True)
                self.model.compile(optimizer=adam, loss=ssd_loss.compute_loss)

    def process_image(self, image_data):
        
        results = []
        inference_duration_s = 0

        try:

            input_images = []
            input_images.append(image_data)
            input_images = np.array(input_images)
            print(input_images.shape)

            with self.graph.as_default():
                with self.session.as_default():

                    inference_time_start = time.time()

                    y_pred = self.model.predict(input_images)
                    
                    inference_time_end = time.time()
                    inference_duration_s = inference_time_end - inference_time_start

                    confidence_threshold = 0.5

                    y_pred_thresh = [y_pred[k][y_pred[k,:,1] > confidence_threshold] for k in range(y_pred.shape[0])]

                    np.set_printoptions(precision=2, suppress=True, linewidth=90)

                    results = []
                    for box in y_pred_thresh[0]:
                        r = {
                            "type": "entity",
                            "entity": {
                                "tag": {
                                    "value": self.classes[int(box[0])],
                                    "confidence": box[1].item()
                                },
                                "box": {
                                    "l": box[2].item() / self.img_width,
                                    "t": box[3].item() / self.img_height,
                                    "w": (box[4].item() - box[2].item()) / self.img_width,
                                    "h": (box[5].item() - box[3].item()) / self.img_height
                                }
                            }
                        }
                        results.append(r)

                    return results, inference_duration_s

        except Exception as e:
            # todo: log
            print("Error: ", e)

        return results, inference_duration_s

app = Flask(__name__)
ssd = None

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
    image = cv2.resize(image, (300, 300), interpolation = cv2.INTER_AREA)
    
    results, _ = ssd.process_image(image)

    respBody = {
        "inferences" : results
    }

    respBody = json.dumps(respBody)

    return Response(respBody, status= 200, mimetype ='application/json')

if __name__ == '__main__':

    ssd = SSDModel()
    
    # Running the file directly
    app.run(host='0.0.0.0', port=8090)
