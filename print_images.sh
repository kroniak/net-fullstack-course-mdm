#!/bin/bash
docker images -a
#docker ps -a
#delete
#docker stop e97d897ef98b
docker rmi -f 970addaf76da 
docker rmi -f 17386de93ac4   
docker rmi -f 7203c8742ec7  
docker rmi -f 7cccd0d55335 

#docker rmi -f $(docker images -q)
#docker stop $(docker ps -a -q)
#docker rm $(docker ps -a -q)
#docker system prune