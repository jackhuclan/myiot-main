@echo off
set version=v1.0.5
echo version=%version%

docker stop vgautodrill_admin_front
docker rm vgautodrill_admin_front
docker image rm vegagroup.tech/vgautodrill_admin_front:%version%

docker load --input vgautodrill_admin_front.%version%.tar.gz

docker run  -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_admin_front  --network vega_network --link vgautodrill_admin -p 8089:80 vegagroup.tech/vgautodrill_admin_front:%version%

echo vgautodrill_admin_front %version% is updated to date
pause
