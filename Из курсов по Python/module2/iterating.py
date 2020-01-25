# x = input().split()
# xs = (int(i) for i in x)
#
#
# # def even(x):
# #     return x % 2 == 0
#
# evens = list(filter(lambda x: x % 2 == 0, xs))
# print(evens)

x = [
    ("Guido", "van", "Rossum"),
    ("Haskell", "Curry"),
    ("John", "Backus")
]

import operator as op
from functools import partial

sort_by_last = partial(list.sort, key=op.itemgetter(-1))
print(x)
sort_by_last(x)
print(x)

y = ["abc", "cba", "abb"]
sort_by_last(y)
print(y)