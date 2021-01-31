# Crack detection

Concrete crack detection using [ssd_keras](https://github.com/pierluigiferrari/ssd_keras). 

Trained model and weight of this sample is using the source from [https://github.com/Garamda/Concrete_Crack_Detection_and_Analysis_SW](https://github.com/Garamda/Concrete_Crack_Detection_and_Analysis_SW).

## Test for LVA extension

Dockerize the app for gauge reader .

> Since `ssd_keras` no longer maintained by author, this sample depends on old version of python(3.6.9), tensorflow(1.15.0) and keras(2.3.1).

```
docker build -t crackdetect:latest -f Dockerfile .
```

Run and test locally.

```
docker run --name crackdetect -p 8090:8090 -d crackdetect:latest

curl -X POST -H "Content-Type: image/jpeg" --data-binary @"7012-187.jpg" http://localhost:8091/score
```

## Dataset

https://digitalcommons.usu.edu/all_datasets/48/
