FROM nvidia/cuda:10.2-cudnn8-devel-ubuntu18.04 as build

ENV DEBIAN_FRONTEND=noninteractive

ARG WORK_DIR=/lvaextension
WORKDIR ${WORK_DIR}
ENV LD_LIBRARY_PATH=${WORK_DIR}

RUN apt-get update && apt-get upgrade -y \
    && apt-get install -y --no-install-recommends \
        build-essential cmake git ca-certificates wget libcpprest-dev \
        libtbb2 libtbb-dev pkg-config libswscale-dev libjpeg-dev libeigen3-dev liblapacke-dev libopenblas-dev libatlas-base-dev \
    && git clone https://github.com/opencv/opencv.git \
    && mkdir opencvbuild && cd opencvbuild \
    && cmake -D CMAKE_BUILD_TYPE=Release -D CMAKE_INSTALL_PREFIX=/usr/local -D BUILD_LIST=core,imgcodecs,imgproc,highgui,videoio,video -D OPENCV_GENERATE_PKGCONFIG=ON ../opencv \
    && make -j$(nproc) && make install
    #&& cd ${WORK_DIR} && rm -rf opencv && rm -rf opencvbuild
 
RUN git clone https://github.com/AlexeyAB/darknet.git \
    && cd darknet \
    && sed -i 's/GPU=0/GPU=1/' Makefile \
    && sed -i 's/CUDNN=0/CUDNN=1/' Makefile \
    && sed -i 's/OPENCV=0/OPENCV=1/' Makefile \
    && sed -i 's/AVX=0/AVX=1/' Makefile \
    && sed -i 's/OPENMP=0/OPENMP=1/' Makefile \
    && sed -i 's/LIBSO=0/LIBSO=1/' Makefile \
    && sed -i 's/width=608/width=416/' cfg/yolov4.cfg \
    && sed -i 's/height=608/height=416/' cfg/yolov4.cfg \
    && make

#RUN cp darknet/libdarknet.so ${WORK_DIR}

FROM nvidia/cuda:10.2-cudnn8-runtime-ubuntu18.04 AS runtime

ENV DEBIAN_FRONTEND=noninteractive

ARG WORK_DIR=/lvaextension
WORKDIR ${WORK_DIR}
ENV LD_LIBRARY_PATH=${WORK_DIR}

RUN apt-get update && apt-get upgrade -y

# copy opencv lib
COPY --from=build /usr/local/lib /usr/local/lib

# copy build output
COPY --from=build ${WORK_DIR}/darknet/libdarknet.so ${WORK_DIR}

# install lib
RUN apt-get install -y --no-install-recommends \
        libcpprest-dev libtbb2 libtbb-dev pkg-config libswscale-dev libjpeg-dev libeigen3-dev liblapacke-dev libopenblas-dev libatlas-base-dev

# some clean up
RUN rm -rf /var/lib/apt/lists/* && \
    apt-get clean

