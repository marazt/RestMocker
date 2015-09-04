RestMocker
==================

Version 1.0.6

Author marazt

Copyright marazt

License The MIT License (MIT)

Last updated 04 September 2015


About
-----------------

RestMocker is a simple server for mocking (simulating) REST APIs for your applications.
It can be run on local machine as console application on it can be deployed into the Azure Cloud.

Abilities
-----------------
+ Run as **standalone server on local machine** (Standalone & OWIN)
+ Run as **standalone server in Azure as Worker Role** (Standalone & OWIN)
+ Written in **.NET WebApi2**
+ Easy to run
+ Easy to configure
+ Configuration via **configuration file**
+ Configuration via **configuration resource**
+ View of the configured API as Swagger spec (**{app_address}/swagger**, e.g. http://localhost:8654/swagger/ui/index)
+ Mocking of the **resource**, **response code**, **json data**
+ Possibility to set **response delay (random, total)** of the resource
+ Possibility to specify **concrete** or **generic** request
+ Generation of the configuration file from the **Swagger 1.2** JSON spec
+ Running on multiple platforms: **Windows** (.NET 4.5.1 needed), on **MacOS** and **Linux** (Mono 3.2.8 needed)
+ ...


Versions
-----------------

**1.0.6 - 2015/09/07**

* Update of the IConfigurationService. Get rid of method for loading configuration to have better interface for other services, e.g. db, other API, etc. Not only configuration from file.



**1.0.5 - 2015/06/23**

* Added new configuration property "DoNotRespond". Use this property if you don't want to return request response. Default value is 'false'.
* Added model validation (WebApi ActionFilterAttribute)



**1.0.4 - 2015/06/16**

* Added new configuration property "RandomDelay". Total delay is not counted in followingf way: TotalDelay = MinDelay + RandomDelay
* Some little refactoring
* Added support of **Swagger API documentation** on URI yourAppp/swagger/, e.g. http://localhost:8654/swagger/ui/index



**1.0.3 - 2015/05/24**

* Fix of the IoC configuration.



**1.0.2 - 2015/05/21**

* Added Ninject IoC. I wanted to use Ninject OwinHost too, but there is a problem that after downloading these package, project needs to reference System.Web because of routing - and this is bad.



**1.0.1 - 2015/05/17**

* Added handling for Patch, Head and Options HTTP methods
* Refactoring of the controller handling



**1.0.0 - 2015/04/29**

* Initial version


Example Configuration
-----------------
Imagine you have an application consuming REST API and you want to establish integration tests of you app, but the API you are consuming is 3rd party app which randomly fails.
You are consuming following resources:

+ **http://www.someapp.org/api/shop/orders/{orderId}** (GET)
+ **http://www.someapp.org/api/shop/orders** (POST)
+ **http://www.someapp.org/api/shop/orders/{orderId}** (DELETE)

RestMocker configuration will be following:

```json
[
	{
		"Name": "orders-get-by-id",
		"Resource": "/api/shop/orders/{orderId}",
		"Method": "get",
		"MinDelay": 0,
		"RandomDelay": 1000,		
		"Response": {
			"Json": { "oderId": 3, "timestamp": "2025-03-02", "description": "some desc" },
			"Headers": {},
			"StatusCode": 200
		}
	},
	{
		"Name": "orders-create-new",
		"Resource": "/api/shop/orders",
		"Method": "post",
		"MinDelay": 1000,
		"RandomDelay": 5000,
		"Response": {
			"Json": { "oderId": 457, "timestamp": "2025-03-03", "description": "new item created" },
			"Headers": {},
			"StatusCode": 201
		}
	},
	{
		"Name": "orders-delete",
		"Resource": "/api/shop/orders/{orderId}",
		"Method": "delete",
		"MinDelay": 0,
		"RandomDelay": 0,		
		"Response": {
			"Json": {},
			"Headers": {},
			"StatusCode": 200
		}
	},
	{
		"Name": "orders-delete-fail",
		"Resource": "/api/shop/orders/346",
		"Method": "delete",
		"MinDelay": 2300,
		"RandomDelay": 0,		
		"Response": {
			"Json": {},
			"Headers": {"User-Agent":"Mozilla/5.0 (X11; Linux x86_64; rv:12.0)"},
			"StatusCode": 403
		}
	},
	 {
		 "Name": "orders-get-nothing",
		 "Resource": "/api/shop/orders/500",
		 "Method": "get",
		 "DoNotRespond": true
	 }
]
```
The configuration behaves as follows:

 1. Resource **orders-get-by-id** returns for every request on any **orderId** the same response as defined. It is because the resource contains '*generic*' **{orderId}**
 2. Resource **orders-create-new** returns for every request the same response with information that order was successfully created and as JSON data it sends new created order item. The minimal response of this request is set to 500 miliseconds.
 3. Resource **orders-delete** returns successful request for every orderId except **346** - it is because the fourth configuration **orders-delete-fail** handles deletion of the order with this **orderId**. It means that **orders-delete-fail** has priority before **orders-delete** request.
 4. Resource **orders-delete-fail** returns HTTP code 403 after at least 2300 ms
 5. Resource **orders-get-nothing** does not return anything

**Note 1:** *Name* of the resource must be unique

**Note 2:** There is registered resource **/restmocker/config**

Thats all <i class="icon-smile"></i>


Applying of the Configuration
-------------------------------
Configuration can be used in two ways:

+ Set it into configuration file **Configuration/config.json**
OR
+ Use special reserved resource **/restmocker/config (GET, POST)**
 + **/restmocker/config (GET)** returns actual configuration.
 + **/restmocker/config (POST)** accepts as data a configuration JSON. Call to this resource will replace actual configuration with the configuration sent in request.


<i class="icon-upload"></i> Running on the Local Machine
------------------------------
If you want to run **RestMocker** on your local machine, run **RestMocker.Console.exe** (mono RestMocker.Console.exe)

You can specify a concrete host and port in **RestMocker.Console.vshost.exe.config** - change values of keys host and port:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <startup>
	<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.1" />
  </startup>
  <appSettings>
	<add key="port" value="8654" />
	<add key="host" value="http://localhost" />
  </appSettings>
</configuration>
```

<i class="icon-upload"></i> Running on Azure
------------------------------
Just publish it via **RestMocker.CloudService -> Publish** from Visual Studio or Xamarin Studio.


Solution Structure
-----------------------------

+ **RestMocker.Model**
 + Project with model definitions
+ **RestMocker.Core**
 + Project common core classes
+ **RestMocker.Console**
 + Console application with self-hosted server for local machine usage
+ **RestMocker.WorkerRole**
 + Azure Cloud Worker Role project
+ **RestMocker.ConfigTransformer**
 + Helper console application for config file transformation

ConfigTransformer
-----------------------------
ConfigTransformer is a helper console application which can prepare a configuration from different API description format, e.g. *Swagger*.
now, it supports only **Swagger 1.2** format as an input.

Just run **RestMocker.ConfigTransformer.exe -v swagger1.2 -s your-swagger-spec.json -t out-config.json**

It will generate a file **out-config.json** with configuration of all resources defined in Swagger specification file. Then specify which JSON data, response code or delay time you want to return from the particular resources.