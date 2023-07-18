@echo off
set zip=YAL-Deck.zip
del /Q %zip%
cd ..\bin\Release
cmd /C 7z a ..\..\export\%zip%^
 Newtonsoft.Json.dll^
 Newtonsoft.Json.xml^
 YAL-Deck.exe^
 YAL-Deck.exe.config^
 YAL-Deck.pdb
cd ..\..
7z a export\YAL-Deck.zip DeckLightbox
echo Packed up! Continue to upload.
pause

cd export
set /p ver="Version?: "
echo %ver%>version.txt
cmd /C 7z a %zip% version.txt
pause
echo Uploading %ver%...
cmd /C itchio-butler push %zip% yellowafterlife/deck:windows --userversion=%ver%
pause