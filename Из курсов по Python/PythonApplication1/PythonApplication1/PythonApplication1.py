x=[1,2,3,8,-10.2]





n = int(input())
s=0
for _ in range(n):
  s = s+int(input())

print(s)



def s(a, *vs, b=10):
   res = a + b
   for v in vs:
       res += v
   return res




class MoneyBox:
    count=0
    def __init__(self, capacity):
        self.cap=capacity
        #self.count=0

    def can_add(self, v):
        if self.count+v <= self.cap:
           return True
        return False

    def add(self, v):
        self.count += v





class Buffer:
    def __init__(self):
        self.ls=[]
    def add(self, *a):
        # добавить следующую часть последовательности
        self.ls+=a
        while(len(self.ls)>=5):
            print(sum(self.ls[slice(0,5)]))
            self.ls=self.ls[slice(5,len(self.ls))]

    def get_current_part(self):
        # вернуть сохраненные в текущий момент элементы последовательности в порядке, в котором они были     
        # добавлены
        return self.ls

buf = Buffer()
buf.add(1, 2, 3)
buf.get_current_part() # вернуть [1, 2, 3]
buf.add(4, 5, 6) # print(15) – вывод суммы первой пятерки элементов
buf.get_current_part() # вернуть [6]
buf.add(7, 8, 9, 10) # print(40) – вывод суммы второй пятерки элементов
buf.get_current_part() # вернуть []
buf.add(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1) # print(5), print(5) – вывод сумм третьей и четвертой пятерки
buf.get_current_part() # вернуть [1]





graph = {'A': ['B', 'C'],

             'B': ['C', 'D'],

             'C': ['D'],

             'D': ['C'],

             'E': ['F'],

             'F': ['C']}


graph={}
ct=int(input())

for _ in range(ct):
    ip=input().split(' : ')
   # print(ip)
    if len(ip)!=1:
        graph[ip[0]]=list(ip[1].split(' '))
    else:
        graph[ip[0]]=[]

graph2=graph
for arg in graph:
    vs=graph[arg]
    for val in vs:
        if val not in graph.keys():
            graph2[val]=[]
graph=graph2

for _ in range(3):
    for arg in graph:
        vs=graph[arg]
        for val in vs:
            graph[arg]+=graph[val]
            #graph[arg]=

for arg in graph:
    graph[arg]+=arg

vt=[]
ct=int(input())
for _ in range(ct):
    ans=input().split(' ')
    b = 'No'
    if ans[0] in graph[ans[1]]:
        b='Yes'
    vt.append(b)

for i in range(ct): print(vt[i])

"4"
"A"
"B : A"
"C : A"
"D : B C"
"4"
"A B"
"B D"
"C D"
"D A"



class ExtendedStack(list):

    def sum(self):
        a = self.pop()
        b = self.pop()
        self.append(a+b)

    def sub(self):
        a = self.pop()
        b = self.pop()
        self.append(a-b)

    def mul(self):
        a = self.pop()
        b = self.pop()
        self.append(a*b)

    def div(self):
        a = self.pop()
        b = self.pop()
        self.append(a//b)


import time

class Loggable:
    def log(self, msg):
        print(str(time.ctime()) + ": " + str(msg))

class LoggableList(list,Loggable):
    def append(self,elem):
        list.append(self, elem)
        Loggable.log(self,elem)




try:
    newClass.foo()
except ZeroDivisionError:
    print("ZeroDivisionError")
except AssertionError:
    print("AssertionError")
except ArithmeticError:
    print("ArithmeticError")





class NonPositiveError(Exception):pass
class PositiveList(list):
    def append(self, object):
        if object<=0:
            raise NonPositiveError()
        else:
            super().append(object)





















