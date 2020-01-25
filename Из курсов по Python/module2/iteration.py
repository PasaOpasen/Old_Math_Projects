

lst = [1, 2, 3, 4, 5, 6]
book = {
    'title': 'The Langoliers',
    'author': 'Stephen King',
    'year_published': 1990
}
string = "Hello, World!"

for i in lst:
    print(i)

it = iter(lst)
while True:
    try:
        i = next(it)
        print(i)
    except StopIteration:
        break




