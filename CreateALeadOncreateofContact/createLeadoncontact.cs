using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CreateALeadOncreateofContact
{
    public class createLeadoncontact : IPlugin

    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //tracing service essential for debugging
            ITracingService tracingservice = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingservice.Trace("Tracing Service Invoked");

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            tracingservice.Trace("Context Obtained Invoked");
            //Provides programmatic access to the metadata and data for an organization.
           

            int step = 1;

            if (context.InputParameters.Contains("Target") & context.InputParameters["Target"] is Entity)
            {

                Guid LeadID= Guid.Empty;

                Entity contact = (Entity)context.InputParameters["Target"];

                if (contact.LogicalName != "contact")

                    return;

                try
                {
                    string firstName = "";
                    string lastName = "";
                    string phone = "";
                    string email = "";


                    IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = organizationServiceFactory.CreateOrganizationService(context.UserId);
                    tracingservice.Trace("Org Service Invoked");

                    
                    if (contact.Attributes.Contains("firstname"))
                        firstName = contact["firstname"].ToString();
                        tracingservice.Trace("firstname: "+firstName);

                    if (contact.Attributes.Contains("lastname"))
                        lastName = contact["lastname"].ToString();
                        tracingservice.Trace("lastName: "+lastName);

                    if (contact.Attributes.Contains("mobilephone"))
                        phone = contact["mobilephone"].ToString();
                        tracingservice.Trace("phone: "+ phone);

                    if (contact.Attributes.Contains("emailaddress1"))
                        email = contact["emailaddress1"].ToString();
                        tracingservice.Trace("email: "+email);
                    
                    step = 2;

                    Entity lead = new Entity("lead");
                    tracingservice.Trace("Lead Creation Invoked");

                    lead["subject"] = "Lead Created from Contact";
                    lead["firstname"] = firstName;

                    lead["lastname"] = lastName;
                    lead["mobilephone"] = phone;
                    lead["emailaddress1"] = email;
                    lead["parentcontactid"] = new EntityReference("lead", contact.Id);
                    tracingservice.Trace("Leadparentid: " + lead["parentcontactid"]);
                  
                    LeadID = service.Create(lead);

                    tracingservice.Trace("Lead Created with GUID" + LeadID);

                }

                catch (Exception ex)
                {
                    tracingservice.Trace("{0}", ex.ToString());
                    throw;
                }

            }

        }
        
    }
}
 