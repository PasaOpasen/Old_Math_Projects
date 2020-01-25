import os
import os.path
import shutil

shutil.copytree("tests", "tests/tests")

for current_dir, dirs, files in os.walk("."):
    print(current_dir, dirs, files)