import platform
import subprocess
import sys
from pathlib import Path
if subprocess.call(['nasm', '-v'], stdout=subprocess.DEVNULL, stderr=subprocess.PIPE) != 0:
    print('nasm is not installed', file=sys.stderr)
    exit(-1)

if subprocess.call(['clang', '-v'], stdout=subprocess.DEVNULL, stderr=subprocess.PIPE) != 0:
    print('clang is not installed', file=sys.stderr)
    exit(-1)
cd = Path(__file__).parent
source_path = str(cd.joinpath('tetris.asm'))
obj_path = str(cd.joinpath('TetrisObject'))
file_format, output_file_path = '', ''
if platform.system().lower() == 'windows':
    file_format, output_file_path = 'win32', str(cd.joinpath('Tetris.exe'))
else:
    file_format, output_file_path = 'elf32', str(cd.joinpath('Tetris'))

subprocess.check_call(['nasm', '-f', file_format, '-o', obj_path, source_path], stdout=subprocess.DEVNULL, stderr=subprocess.PIPE)
subprocess.check_call(['clang', '-o', output_file_path, '-target', 'i386' , obj_path], stdout=subprocess.DEVNULL, stderr=subprocess.PIPE)

Path('TetrisObject').unlink()

print('Built successfully')
