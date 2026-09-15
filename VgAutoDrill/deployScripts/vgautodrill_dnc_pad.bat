@echo off
set version=v3.5.30.1331-dnc-pad
echo version=%version%

docker stop vgautodrill_dnc_pad
docker rm vgautodrill_dnc_pad
docker image rm vegagroup.tech/vgautodrill_dnc_pad:%version%

docker load --input vgautodrill_dnc_pad.tar.gz

docker run  -e TZ=Asia/Shanghai -v vgautodrill_dnc_pad_conf:/etc/nginx -d --restart always --name vgautodrill_dnc_pad  --network vega_network -p 8090:8090 vegagroup.tech/vgautodrill_dnc_pad:%version%

