Monoscape
==========
Monoscape is an open source, Mono Platform as a Service (PaaS) Cloud solution. It has a complete software stack for deploying and managing applications in the cloud with automatic load balancing and scaling features.

Installation
-------------
1. Select an IaaS of your choice which supports EC2 API. Monoscape would recommend OpenStack.
2. Install an instance of the IaaS and create a new user account for monoscape to access the EC2 API.
3. Find EC2 authentication details (secret key and access key) for the above user from the IaaS dashboard. 
4. Update Monoscape Application Grid Controller configuraiton file with the above information.
5. Start Monoscape Load Balancer using start-load-balancer.sh shell script.
6. Start Monoscape Application Grid using start-application-grid.sh shell script.
7. Start Monoscape Dashboard using start-dashboard.sh shell script.
8. Login to Monoscape Dashboard using http://host-ip:8080 and verify IaaS authentication status.
9. Create a new key for a EC2 image of Ubuntu 12.04 server and start an instance.
10. Copy Monoscape Node Controller to the above VM instance.
11. Update Monoscape Node Controller configuration file with the IP address of the application grid.
12. Export the above instance of the VM and import it to the IaaS using the same key.
13. Login to Monoscape Dashboard and upload Mono applications to be deployed in the PaaS.
14. Start an application and do a sanity test to see whether its is working.

Applications Supported
-----------------------
As the name implies it supports applications written in Mono.


Managing & Monitoring the Cloud
--------------------------------
Monoscape has a built-in web interface for managing and monitoring the cloud.


Application Deployment Process
-------------------------------
Applications could be deployed to the cloud via the Monoscape web interface.


Web Servers Used
-----------------
Currently Monoscape uses Mono XSP web server (http://en.wikipedia.org/wiki/XSP_(software)), however support for more web servers which could run Mono applications will be added in the future.


IaaS Platforms Supported
-------------------------
Currently Monoscape has been tested on OpenStack and Eucalyptus. However it should work on any other IaaS platform which has EC2 API implemented.





