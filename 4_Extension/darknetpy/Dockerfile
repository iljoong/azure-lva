# update with your own darknetbase image
FROM darknetbase:latest

ARG WORK_DIR=/lvaextension
WORKDIR ${WORK_DIR}
ENV LD_LIBRARY_PATH=${WORK_DIR}

# Install python
RUN apt-get update -y && \
    apt-get install -y --no-install-recommends python3-pip python3-dev && \
    cd /usr/local/bin && \
    ln -s /usr/bin/python3 python && \
    pip3 install --upgrade pip

# Install python packages
RUN pip install numpy flask pillow matplotlib opencv-python && \
    apt-get clean

# Install package for opencv
RUN apt-get update -y && \
    apt-get install --no-install-recommends -y libgl1-mesa-glx libgtk2.0-dev wget

# Copy or download weights from gdrive location
RUN cd ${WORK_DIR} && chmod +x gdown.sh && ./gdown.sh 1uOWZGx1oR1bRwp_mnvxobaXZcWs1X9ar yolov4-helmet-detection.weights

# Copy the app file and the tags file
COPY * ${WORK_DIR}/

# cleanup
RUN apt-get purge -y --auto-remove wget \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

EXPOSE 8089

# Start app
ENTRYPOINT ["python","/lvaextension/main.py"]