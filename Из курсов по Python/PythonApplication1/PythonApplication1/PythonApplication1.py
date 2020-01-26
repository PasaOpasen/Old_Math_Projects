x = [1,2,3,8,-10.2]





n = int(input())
s = 0
for _ in range(n):
  s = s + int(input())

print(s)



def s(a, *vs, b=10):
   res = a + b
   for v in vs:
       res += v
   return res




class MoneyBox:
    count = 0
    def __init__(self, capacity):
        self.cap = capacity
        #self.count=0

    def can_add(self, v):
        if self.count + v <= self.cap:
           return True
        return False

    def add(self, v):
        self.count += v





class Buffer:
    def __init__(self):
        self.ls = []
    def add(self, *a):
        # добавить следующую часть последовательности
        self.ls+=a
        while(len(self.ls) >= 5):
            print(sum(self.ls[slice(0,5)]))
            self.ls = self.ls[slice(5,len(self.ls))]

    def get_current_part(self):
        # вернуть сохраненные в текущий момент элементы последовательности в
        # порядке, в котором они были
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


graph = {}
ct = int(input())

for _ in range(ct):
    ip = input().split(' : ')
   # print(ip)
    if len(ip) != 1:
        graph[ip[0]] = list(ip[1].split(' '))
    else:
        graph[ip[0]] = []

graph2 = graph
for arg in graph:
    vs = graph[arg]
    for val in vs:
        if val not in graph.keys():
            graph2[val] = []
graph = graph2

for _ in range(3):
    for arg in graph:
        vs = graph[arg]
        for val in vs:
            graph[arg]+=graph[val]
            #graph[arg]=
for arg in graph:
    graph[arg]+=arg

vt = []
ct = int(input())
for _ in range(ct):
    ans = input().split(' ')
    b = 'No'
    if ans[0] in graph[ans[1]]:
        b = 'Yes'
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
        self.append(a + b)

    def sub(self):
        a = self.pop()
        b = self.pop()
        self.append(a - b)

    def mul(self):
        a = self.pop()
        b = self.pop()
        self.append(a * b)

    def div(self):
        a = self.pop()
        b = self.pop()
        self.append(a // b)


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
        if object <= 0:
            raise NonPositiveError()
        else:
            super().append(object)




import datetime
import sys

(y,m,d) = [int(n) for n in input().split()]
dt = datetime.date(y,m,d)
td = datetime.timedelta(days=int(input()))

n = dt + td
print(n.year,n.month,n.day)




import simplecrypt

with open("encrypted.bin", "rb") as inp:
    encrypted = inp.read()
encrypted = b'sc\x00\x02\x96\x93^\xd7&1\x9f\xd0\x14\x02\x14\xd1\x92`\xeb\x1b\xdbulr\x0e\xeb\x0f\xf0D\xcf\x87\xf5\xd5\xf2oKA\x89b/\xaa\xa6y;\x8b)\x89\xbdl\x0f\x96\x144\x8e\xe2P\xa8\xcf\xc7T\xf6>.`m\xfbC/\xc1V\xd2>\xd0\xaf\xbb0%V\x14\xac\xf7\n\xcd'
password = open("passwords.txt", "r").read()
password = '9XB8nsIqRfYeswC\n4sEhUGLEZti9BiN\nbDjmT0NcIW8nzhb\nZN6QQoMOO1ZQLUY\nRVrF2qdMpoq6Lib\ntnnX7HH3vJ9Hiji\nC24TJYYkqekv40l\nB2ropluPaMAitzE\nDRezNUVnr2zC0CP\nXCNmpTvvZb1n3mX'.split('\n')

mess = decrypt(password,encrypted).decode('utf8')

for s in password:
    try:
        mess = decrypt(s,encrypted).decode('utf8')
    finally:
        print(mess)






def count_res(self, a):
    res = [f(a) for f in self.funcs]
    return res.count(True), res.count(False)
pos, neg = self.count_res(i)


class multifilter:
    """
    fsde
    """
    def judge_half(pos, neg):
        if pos >= neg:
            return True
        else:
            return False

    def judge_any(pos, neg):
        if pos > 0:
            return True
        else:
            return False

    def judge_all(pos, neg):
        if neg == 0:
            return True
        else:
            return False

    def count_res(self, a):
        res = [f(a) for f in self.funcs]
        return res.count(True), res.count(False)

    def __init__(self, iterable, *funcs, judge=judge_any):
        # iterable - исходная последовательность
        self.it = iterable
        # funcs - допускающие функции
        self.funcs = funcs
        # judge - решающая функция
        self.filt = judge

    def __iter__(self):
        # возвращает итератор по результирующей последовательности
        for el in self.it:
            if self.filt(*self.count_res(el)):
                yield el
            #raise StopIteration

def mul2(x):
    return x % 2 == 0

def mul3(x):
    return x % 3 == 0

def mul5(x):
    return x % 5 == 0


a = [i for i in range(31)] # [0, 1, 2, ...  , 30]
print(list(multifilter(a, mul2, mul3, mul5))) 
# [0, 2, 3, 4, 5, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 24, 25, 26, 27,
# 28, 30]
print(list(multifilter(a, mul2, mul3, mul5, judge=multifilter.judge_half))) 
# [0, 6, 10, 12, 15, 18, 20, 24, 30]
print(list(multifilter(a, mul2, mul3, mul5, judge=multifilter.judge_all))) 



def issimple(val):
    k = 0
    for i in range(1,val):
        if val % i == 0:
            k+=1
    return k <= 1
[(issimple(n), n) for n in range(1,15)]

def primes():
    a = 1
    while True:
        a += 1
        if issimple(a):
            yield a




lt = open("dataset_24465_4 (1).txt","r")
s = [p.strip() for p in lt.readlines()]
s.reverse()
lt.close()
f = open("dataset_24465_4(result).txt","w")
f.write('\n'.join(s))
f.close()



import os
import os.path
pth = "C:\\Users\\крендель\\Downloads\\main\\main"

t = []

for dir, dirs, files in os.walk(pth):
    if list(filter(lambda x: x.endswith('.py'), files)):
       dr = dir.replace("C:\\Users\\крендель\\Downloads\\main\\","")
       print(dr)
       t.append(dr.replace('\\', '/'))



def mod_checker(x, mod=0):
    return lambda y:y % x == mod





s="ababa"
a="a"
b="b"

s=input()
a=input()
b=input()
k=0
while a in s and k<=1000:
    s=s.replace(a,b)
    k+=1
if k>1000:
    print("Impossible")
else:
    print(k)




s = "abababa"
t = "aba"

s=input()
t=input()
i=0
k=0
while i<=len(s)-len(t):
    if s[i:].startswith(t):
        k+=1
    i+=1

print(k)





import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    count= len(re.findall("cat",line))
    if count>1:
        t.append(line)

print("\n".join(t))



import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    count= len(re.findall(r"\bcat\b",line))
    if count>0:
        t.append(line)

print("\n".join(t))




import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    count= len(re.findall(r"z.{3}z",line))
    if count>0:
        t.append(line)

print("\n".join(t))




import re
st=[k for k in input().split() if len(re.findall(r"\\",k))>0]


import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    count= len(re.findall(r"\\",line))
    if count>0:
        t.append(line)

print("\n".join(t))



import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    count= len(re.findall(r"\b().{2}\b",line))
    if count>0:
        t.append(line)

print("\n".join(t))


import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    t.append(re.sub("human","computer",line))

print("\n".join(t))



import sys
import re

t=[]
for line in sys.stdin:
    line = line.rstrip()
    t.append(re.sub("\b([aA]\w+)|([aA])\b","argh",line,1))

print("\n".join(t))












































































