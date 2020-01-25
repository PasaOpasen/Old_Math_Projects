
# r (read) - открыть для чтения (по умолчанию)
# w (write) - открыть для записи, содержимое файла стирается
# a (append) - открыть для записи, запись ведется в конец
f = open("test.txt")
for line in f:
    line = line.rstrip()
    print(repr(line))
x = f.read()
print(repr(x))

f.close()
