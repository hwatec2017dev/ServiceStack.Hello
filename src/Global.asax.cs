﻿using System;
using System.Runtime.Serialization;
using ServiceStack.Text;

namespace ServiceStack.Hello
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>    
    [DataContract(Namespace = "http://schemas.servicestack.net/types")]
    public class Hello : IReturn<HelloResponse>
    {
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>

    [DataContract(Namespace = "http://schemas.servicestack.net/types")]
    public class HelloResponse
    {
        [DataMember]
        public string Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack web service implementation.
    /// </summary>
    public class HelloService : IService
    {
        public object Any(Hello request)
        {
            //Looks strange when the name is null so we replace with a generic name.
            return new HelloResponse { Result = "Hello, " + (request.Name ?? "John Doe") };
        }
    }

    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Create your ServiceStack web service application with a singleton HelloAppHost.
        /// </summary>        
        public class HelloAppHost : AppHostBase
        {
            /// <summary>
            /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
            /// </summary>
            public HelloAppHost() : base("Hello Web Services", typeof(HelloService).Assembly) { }

            /// <summary>
            /// Configure the container with the necessary routes for your ServiceStack application.
            /// </summary>
            /// <param name="container">The built-in IoC used with ServiceStack.</param>
            public override void Configure(Funq.Container container)
            {
                Plugins.Add(new SoapFormat());

                //Register user-defined REST-ful urls. You can access the service at the url similar to the following.
                //http://localhost:62577/servicestack/hello or http://localhost:62577/servicestack/hello/John%20Doe
                //You can change /servicestack/ to a custom path in the web.config.
                Routes
                    .Add<Hello>("/hello")
                    .Add<Hello>("/hello/{Name}");
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            new HelloAppHost().Init();
        }
    }
}
