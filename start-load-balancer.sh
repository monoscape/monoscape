#!/bin/bash

# Start load balancer controller
cd Monoscape.LoadBalancerController/bin/Debug
./Monoscape.LoadBalancerController.exe
# Start load balancer web interface
cd Monoscape.LoadBalancerController.Web
xsp
