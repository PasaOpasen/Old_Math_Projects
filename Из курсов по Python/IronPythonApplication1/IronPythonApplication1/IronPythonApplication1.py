


objects = [1, 2, 1, 2, 3]

print( 1 in objects)

lt=[]
a=0
for obj in objects: # доступная переменная objects
    if obj not in lt:
        lt.append(obj)
        a+=1

print(a)




