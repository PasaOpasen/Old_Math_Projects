
import numpy as np

mat=np.eye(3,4,0)*2+np.eye(3,4,1)
#print(mat)
mat=mat.reshape((3*4,1))
print(mat)






import numpy as np

x_shape = tuple(map(int, input().split()))
X = np.fromiter(map(int, input().split()), np.int).reshape(x_shape)
y_shape = tuple(map(int, input().split()))
Y = np.fromiter(map(int, input().split()), np.int).reshape(y_shape)

if x_shape[1]!=y_shape[1]:
    print("matrix shapes do not match")
else:
    print(X.dot(Y.T))








from urllib.request import urlopen
import numpy as np

filename =input()#"https://stepic.org/media/attachments/lesson/16462/boston_houses.csv" 
f = urlopen(filename)

sbux = np.loadtxt(f,skiprows=1,  delimiter=",")
print(np.array(sbux).mean(axis=0))






example = [

  [ 1, 1, 0.3, 1 ],
  [ 1, 0.4, 0.5, 1 ],
  [ 1, 0.7, 0.8, 0 ]
]
#4 колонка: груша (1) или нет (0)

w = [ 0, 0, 0 ]

perfect = False

while not perfect:
  perfect = True

  for e in example:
    dot = e[0]*w[0] + e[1]*w[1] + e[2]*w[2]
    predict = int(dot > 0)
    target = e[3]

    if predict != target:
      perfect = False

      if predict == 0:
        w[0] = w[0] + e[0]
        w[1] = w[1] + e[1]
        w[2] = w[2] + e[2]
      else:
        w[0] = w[0] - e[0]
        w[1] = w[1] - e[1]
        w[2] = w[2] - e[2]
      print( w )





example = [[1, 1, 0.3, 1], [1, 0.4, 0.5, 1], [1, 0.7, 0.8, 0]]
w = [0, 0, 0]
perfect = False
step = 1
while not perfect:
  perfect = True
  for e in example:
    dot = e[0]*w[0] + e[1]*w[1] + e[2]*w[2]
    predict = int(dot > 0)
    target = e[3]
    if predict != target:
      perfect = False
      for i in range(len(w)):
        w[i] += (target-predict) * e[i]
    print( 'step ' + str(step) + ': ' + str(w) )
    step+=1







import numpy as np
import tensorflow as tf
import tensorflow.keras as keras

x = np.array([[0, 0], [0, 1], [1, 0], [1, 1]])
y = np.array([0 , 1, 1, 0])

model = keras.Sequential([
    keras.layers.Dense(2, activation=tf.math.sigmoid),
    keras.layers.Dense(1, activation=tf.math.sigmoid)
])


model.compile(optimizer=tf.train.GradientDescentOptimizer(1),
              loss='binary_crossentropy',
              metrics=['accuracy'])

model.fit(x, y, epochs=5000)

print(model.get_weights())












import urllib
from urllib import request
import numpy as np

fname =input() #"https://stepic.org/media/attachments/lesson/16462/boston_houses.csv"  # read file name from stdin
f = urllib.request.urlopen(fname)  # open file from URL
data = np.loadtxt(f, delimiter=',', skiprows=1)  # load data to work with

# here goes your solution
X = np.delete(data, 0, axis=1)
y = data[:,0]

n,m = X.shape
X0 = np.ones((n,1))
X = np.hstack((X0,X))

XT = X.transpose()
step_1 = np.dot(XT,X)
step_2 = np.linalg.inv(step_1)
step_3 = step_2.dot(X.transpose())
b = step_3.dot(y)

print(*b)







































































