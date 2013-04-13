#!/bin/bash

#Start Monoscape PaaS cloud
./start-load-balancer.sh&
sleep 10
./start-application-grid.sh&
./start-dashboard.sh&

