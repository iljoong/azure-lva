'''
analog gauge reader
'''

import numpy as np
import cv2

from PIL import Image
import matplotlib.pyplot as plt

from gauge_utils import drawImage, drawCalibration

def avg_circles(circles, b):
    avg_x=0
    avg_y=0
    avg_r=0
    for i in range(b):
        #optional - average for multiple circles (can happen when a gauge is at a slight angle)
        avg_x = avg_x + circles[0][i][0]
        avg_y = avg_y + circles[0][i][1]
        avg_r = avg_r + circles[0][i][2]
    avg_x = int(avg_x/(b))
    avg_y = int(avg_y/(b))
    avg_r = int(avg_r/(b))
    return avg_x, avg_y, avg_r

def dist_2_pts(x1, y1, x2, y2):
    #print np.sqrt((x2-x1)^2+(y2-y1)^2)
    return np.sqrt((x2 - x1)**2 + (y2 - y1)**2)

def get_current_value(img, final_line_list, min_angle, max_angle, min_value, max_value, x, y, r):

    # assumes the first line is the best one
    x1 = final_line_list[0][0]
    y1 = final_line_list[0][1]
    x2 = final_line_list[0][2]
    y2 = final_line_list[0][3]

    #find the farthest point from the center to be what is used to determine the angle
    dist_pt_0 = dist_2_pts(x, y, x1, y1)
    dist_pt_1 = dist_2_pts(x, y, x2, y2)
    if (dist_pt_0 > dist_pt_1):
        x_angle = x1 - x
        y_angle = y - y1
    else:
        x_angle = x2 - x
        y_angle = y - y2
    # take the arc tan of y/x to find the angle
    res = np.arctan(np.divide(float(y_angle), float(x_angle)))
    #np.rad2deg(res) #coverts to degrees

    # print x_angle
    # print y_angle
    # print res
    # print np.rad2deg(res)

    #these were determined by trial and error
    res = np.rad2deg(res)
    if x_angle > 0 and y_angle > 0:  #in quadrant I
        final_angle = 270 - res
    if x_angle < 0 and y_angle > 0:  #in quadrant II
        final_angle = 90 - res
    if x_angle < 0 and y_angle < 0:  #in quadrant III
        final_angle = 90 - res
    if x_angle > 0 and y_angle < 0:  #in quadrant IV
        final_angle = 270 - res

    #print final_angle

    old_min = float(min_angle)
    old_max = float(max_angle)

    new_min = float(min_value)
    new_max = float(max_value)

    old_value = final_angle

    old_range = (old_max - old_min)
    new_range = (new_max - new_min)
    new_value = (((old_value - old_min) * new_range) / old_range) + new_min

    return new_value

def detectCircle(img, min_r, max_r, debug=False, tofile=False):
    x = 0
    y = 0
    r = 0
    # get circles
    height, width = img.shape[:2]

    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)  #convert to gray

    circles = cv2.HoughCircles(gray, cv2.HOUGH_GRADIENT, 1, 20, np.array([]), 100, 50, int(height*min_r), int(height*max_r))

    if (type(circles) == type(None)):
        print("No circle found: try different radius size")
        
    else:
        #print(circles.shape)
        cimg = img.copy()

        circles = np.uint16(np.around(circles))

        if (debug):
            for i in circles[0,:]:
                # draw the outer circle
                cv2.circle(cimg,(i[0],i[1]),i[2],(0,255,0),2)
                # draw the center of the circle
                cv2.circle(cimg,(i[0],i[1]),2,(0,0,255),3)
            drawImage(cimg, tofile, 1)
            
        # get average circle
        a, b, c = circles.shape
        x,y,r = avg_circles(circles, b)

    if (debug):
        #draw center and circle
        cv2.circle(cimg, (x, y), r, (255, 0, 0), 3, cv2.LINE_AA)  # draw circle
        cv2.circle(cimg, (x, y), 2, (255, 0, 0), 3, cv2.LINE_AA)  # draw center of circle

        print("center & radious of circle", x, y, r)
        drawImage(cimg, tofile, 2)
    
    return x, y, r

def detectLine(img, x, y, r,  minLineLength=100, show_calibration=False, debug=False, tofile=False):
    # Set thresold and maxValue
    thresh = 175
    maxValue = 255

    # line detection bound
    diff1LowerBound = 0.01 #diff1LowerBound and diff1UpperBound determine how close the line should be from the center
    diff1UpperBound = 0.25
    diff2LowerBound = 0.5 #diff2LowerBound and diff2UpperBound determine how close the other point of the line should be to the outside of the gauge
    diff2UpperBound = 1.0
    
    # apply thresholding which helps for finding lines
    gray2 = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    th, dst2 = cv2.threshold(gray2, thresh, maxValue, cv2.THRESH_BINARY_INV)

    # found Hough Lines generally performs better without Canny / blurring, though there were a couple exceptions where it would only work with Canny / blurring
    #dst2 = cv2.medianBlur(dst2, 5)
    #dst2 = cv2.Canny(dst2, 50, 150)
    #dst2 = cv2.GaussianBlur(dst2, (5, 5), 0)

    # for testing, show image after thresholding
    if (debug):
        drawImage(dst2, tofile, 3)

    # find lines
    # rho is set to 3 to detect more lines, easier to get more then filter them out later
    #minLineLength = 100
    maxLineGap = 0
    lines = cv2.HoughLinesP(image=dst2, rho=3, theta=np.pi / 180, threshold=100,minLineLength=minLineLength, maxLineGap=0)

    if (debug):
        print("number of lines detected: %d" % len(lines))
    
        cimg = img.copy()

        #for testing purposes, show all found lines
        for i in range(0, len(lines)):
            for x1, y1, x2, y2 in lines[i]:
                cv2.line(cimg, (x1, y1), (x2, y2), (0, 255, 0), 2)

        cv2.circle(cimg, (x, y), int(r*diff1LowerBound), (0, 0, 255), 1, cv2.LINE_AA)
        cv2.circle(cimg, (x, y), int(r*diff1UpperBound), (0, 0, 255), 1, cv2.LINE_AA)

        cv2.circle(cimg, (x, y), int(r*diff2LowerBound), (255, 0, 0), 1, cv2.LINE_AA)
        cv2.circle(cimg, (x, y), int(r*diff2UpperBound), (255, 0, 0), 1, cv2.LINE_AA)

        drawImage(cimg, tofile, 4)
        
    # remove all lines outside a given radius
    final_line_list = []
    #print "radius: %s" %r

    for i in range(0, len(lines)):
        for x1, y1, x2, y2 in lines[i]:
            diff1 = dist_2_pts(x, y, x1, y1)  # x, y is center of circle
            diff2 = dist_2_pts(x, y, x2, y2)  # x, y is center of circle
            #set diff1 to be the smaller (closest to the center) of the two), makes the math easier
            if (diff1 > diff2):
                temp = diff1
                diff1 = diff2
                diff2 = temp
            # check if line is within an acceptable range
            if (((diff1<diff1UpperBound*r) and (diff1>diff1LowerBound*r) and (diff2<diff2UpperBound*r)) and (diff2>diff2LowerBound*r)):
                line_length = dist_2_pts(x1, y1, x2, y2)
                # add to final list
                final_line_list.append([x1, y1, x2, y2])

    if (len(final_line_list) > 0):
        x1 = final_line_list[0][0]
        y1 = final_line_list[0][1]
        x2 = final_line_list[0][2]
        y2 = final_line_list[0][3]

        if (show_calibration):
            cimg = img.copy()
            cv2.line(cimg, (x1, y1), (x2, y2), (0, 0, 255), 2)

            cimg = drawCalibration(cimg, x, y, r)
            drawImage(cimg, tofile, 5)
            
        return final_line_list
    else:
        return []