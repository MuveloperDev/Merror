@echo off
set Data=%1

IF EXIST "\SaveData" (goto MakeDataFile)
else (goto MakeDir)

:MakeDir
mkdir SaveData

goto MakeDataFile

:MakeDataFile
cd SaveData 

goto WRITE 

:WRITE
echo %Data% > Data.txt


