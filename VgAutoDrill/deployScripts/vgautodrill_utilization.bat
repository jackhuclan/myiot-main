@echo off
set version=v3.4.25.1539
echo version=%version%

docker stop vgautodrill_utilization_dashboard
docker rm vgautodrill_utilization_dashboard
docker image rm vegagroup.tech/utilization_dashboard:%version%

docker load --input vgautodrill_utilization.tar.gz

docker run  -e TZ=Asia/Shanghai -v utilization_dashboard_conf:/etc/nginx -d --restart always --name vgautodrill_utilization_dashboard  --network vega_network --link vgautodrill_admin -p 8089:80 vegagroup.tech/utilization_dashboard:%version%

