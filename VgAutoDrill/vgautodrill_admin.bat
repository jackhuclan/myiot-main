@echo off
set version=v1.10.17.1445
echo version=%version%

docker stop vgautodrill_admin

docker rm vgautodrill_admin

docker image rm vegagroup.tech/vgautodrill_admin:%version%

docker load --input vgautodrill_admin.tar.gz

docker run  -e TZ=Asia/Shanghai -d --restart always --name vgautodrill_admin  --network vega_network --link vgautodrill_admin_front -p 8003:8003 -v vol_admin_conf:/app/conf -v vol_admin_data:/app/data -v vol_admin_logs:/app/logs vegagroup.tech/vgautodrill_admin:%version%

pause