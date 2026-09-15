::   10:31 2024/7/25

@echo off
 
echo=====Begin to push to git
 
 git pull
 git add .
 git commit -m "auto commit"
 git push

echo=====End to push to git


dotnet nuget push *.nupkg -k vega123 -s http://192.168.1.240:9001/v3/index.json --skip-duplicate
 

pause