import glob
import shutil
import subprocess
from pathlib import Path
from zipfile import ZipFile


def dotnet(*args):
    cmd = ['dotnet', *args]
    ret = subprocess.check_call(cmd)

    if ret:
        raise Exception(f'{cmd} failed Ret={ret}')

PUB_FOLDER = 'bin/Release/Publish'

shutil.rmtree(PUB_FOLDER, ignore_errors=True)
dotnet('publish', '-c', 'Release', '-o', PUB_FOLDER)

with ZipFile(f'{PUB_FOLDER}/../DimMonitorMPVExtension.zip', 'w') as zipfile:
    for file in glob.glob(f'{PUB_FOLDER}/*'):
        basename = Path(file).name
        zipfile.write(file, f'DimMonitorMPVExtension/{basename}')