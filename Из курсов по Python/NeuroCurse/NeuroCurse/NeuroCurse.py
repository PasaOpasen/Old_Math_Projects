
import numpy as np

mat=np.eye(3,4,0)*2+np.eye(3,4,1)
#print(mat)
mat=mat.reshape((3*4,1))
print(mat)

























