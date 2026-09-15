version=v3.4.24.1110-kinwang
docker stop vgautodrill_central
docker rm vgautodrill_central
docker image rm vegagroup.tech/vgautodrill_central:$version

docker load --input vgautodrill_central.tar.gz

docker run -e ASPNETCORE_ENVIRONMENT=Production \
-e TZ=Asia/Shanghai -d --restart always --name vgautodrill_central  --network vega_network \
-p 8001:8001 -p 1883:1883 -v vol_central_conf:/app/conf -v vol_central_data:/app/data -v vol_central_logs:/app/logs vegagroup.tech/vgautodrill_central:$version