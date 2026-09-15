@echo off
set version=v3.4.23.1525
echo version=%version%

docker stop vgautodrill_external

docker rm vgautodrill_external

docker image rm vegagroup.tech/vgautodrill_external:%version%

docker load --input vgautodrill_external.tar.gz

docker run  -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_external  --network vega_network --link vgautodrill_admin_front -p 8005:8005 -v vol_external_conf:/app/conf -v vol_external_data:/app/data -v vol_external_logs:/app/logs vegagroup.tech/vgautodrill_external:%version%