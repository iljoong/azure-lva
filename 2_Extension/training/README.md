# Training custom dataset using Darknet

Mask/Nomask dataset

## training

> assume base location of training is `darknet` folder.

0. Pre-requisites
    - build `darknet`
    - download train [dataset](https://github.com/rushad7/mask-detection/tree/master/annotations) and save in `mydata` folder
    - download baseline weights, [yolov4.conv.137](https://github.com/AlexeyAB/darknet/releases/download/darknet_yolo_v3_optimal/yolov4.conv.137)

1. cpoy/modify `cfg` file

copy `darknet/config/yolov4-custom.cfg` and save as `mask_yolov4-custom.cfg` from`

modify following properties:
    - `[net]` layer
        - comment batch/subdivisions in `Testing` section
        - In `Training` section
            - max_batches = 4000 #(= 2000 * classes)
            - steps = 3200,3600 #(80%,90% of max_batches)
    - `[yolo]` layer
        - classes = 2
    - `[convolution]` layer just above the `[yolo]`
        - filters = 21 #(classes + 5)*3 

Note that if you're GPU does not have large GPU memory, you may need to modify size of `batch` and `subdivisions`.

2. create/edit `object.names` file

This is needed for testing, not training.

```
mask
nomask
```

3. create/edit `obj.data` file

```
classes = 2
train = mydata/train.txt
valid = mydata/test.txt
names = mydata/obj.names
backup = backup
```

Note that `test.txt` is not provided in this sample.

save under `mydata` folder.

4. create/edit `train.txt` file

Use [gen_train.py](./gen_train.py) to generate train.txt.
Note that `train.txt` file is a list of relative path of training images. 

5. train dataset

```bash
./darknet detector train mydata/obj.data mask_yolov4-custom.cfg yolov4.conv.137 -dont_show
```