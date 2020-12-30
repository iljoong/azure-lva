# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

FROM ubuntu:18.04

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
    apt-get install --no-install-recommends -y libgl1-mesa-glx libgtk2.0-dev

# Download the model
RUN mkdir /app && \
    cd /app && \
    rm -rf /var/lib/apt/lists/* && \
    apt-get clean

# Copy the app file and the tags file
COPY *.py /app/

EXPOSE 8888

# Start app
CMD ["python","/app/main.py"]