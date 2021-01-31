# Base image
FROM tensorflow/tensorflow:1.15.5-gpu-py3

ARG WORK_DIR=/ssd_keras

RUN apt-get update && apt-get upgrade -y \
    && apt-get install -y --no-install-recommends \
        git wget unzip libgl1-mesa-glx \
    && git clone https://github.com/pierluigiferrari/ssd_keras.git \
    && cd ssd_keras \
    && sed -i "s/K.image_dim_ordering() == 'tf'/K.image_data_format() == 'channels_last'/g" keras_layers/keras_layer_L2Normalization.py \
    && sed -i "s/K.image_dim_ordering() == 'tf'/K.image_data_format() == 'channels_last'/g" keras_layers/keras_layer_AnchorBoxes.py \
    && wget https://github.com/Garamda/Concrete_Crack_Detection_and_Analysis_SW/raw/master/ssd300_pascal_07%2B12_epoch-08_loss-1.9471_val_loss-1.9156.zip \
    && unzip *.zip

WORKDIR ${WORK_DIR}

COPY app/* ${WORK_DIR}/

RUN pip install -r requirement.txt

ENTRYPOINT ["python", "main.py"]