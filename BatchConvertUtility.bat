@ECHO off
ECHO Please be careful that you select the correct paths!
ECHO I'm not taking any responsibility for any damage that might incure if you don't put the paths in correctly.

set /p OMDPath="Path to OMDs: " 
set /p EXEPath="Path to OMD2OBJ: "

echo Are these paths correct correct?
echo %OMDPath%
echo %EXEPath%

set /p ANSWER="Y/N: "

if NOT "%ANSWER%"=="Y" (goto ENDPROG)

:ProgRun
cd /D "%OMDPath%"

for /r %%f in (*.omd) do %EXEPath% %%f

:ENDPROG
echo Exiting...

