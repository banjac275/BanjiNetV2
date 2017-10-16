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

    public List<PrevEmp> PreviousEmployment { get; set; }

    public List<Guid> Friends { get; set; }

    public WorkersR()
    {
        Id = Guid.NewGuid();
    }
}

public class PrevEmp
{
    public Guid FirmId { get; set; }

    public Guid FormerEmployeeId { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}