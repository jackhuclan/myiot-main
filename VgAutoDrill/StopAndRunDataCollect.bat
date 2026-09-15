@echo off
set version=v1.0.5
echo version=%version%

docker stop vgautodrill_collect

docker rm vgautodrill_collect

docker image rm vegagroup.tech/vgautodrill_collect:%version%

docker load --input vgautodrill_collect.%version%.tar.gz

docker run  -e TZ=Asia/Shanghai -e ASPNETCORE_ENVIRONMENT=Development -d --restart always --name vgautodrill_collect  --network vega_network -p 8002:8002  -v vol_collect_conf:/app/conf -v vol_collect_data:/app/data -v vol_collect_logs:/app/logs vegagroup.tech/vgautodrill_collect:%version%

echo vgautodrill_collect %version% is updated to date
pause