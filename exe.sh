#!/bin/bash
docker build -t alfamyimage .
docker run -d -p 80:59722 alfamyimage