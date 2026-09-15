version=v3.5.30.0920
docker stop vgautodrill_admin_front
docker rm vgautodrill_admin_front
docker image rm vegagroup.tech/vgautodrill_admin_front:$version

docker load --input vgautodrill_admin_front.tar.gz

docker run  -e TZ=Asia/Shanghai -v adminfront_conf:/etc/nginx -d --restart always --name vgautodrill_admin_front  --network vega_network --link vgautodrill_admin -p 8080:80 vegagroup.tech/vgautodrill_admin_front:$version