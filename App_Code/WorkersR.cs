using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;


/// <summary>
/// Summary description for WorkersR
/// </summary>

public class WorkersR
{
    public Guid Id { get; set; }

    public string CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Checkbox { get; set; }

    public WorkersR()
    {
        Id = Guid.NewGuid();
    }
}