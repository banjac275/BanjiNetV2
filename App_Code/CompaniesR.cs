using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CompaniesR
/// </summary>

public class CompaniesR
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; }

    public string Owner { get; set; }

    public string Type { get; set; }

    public string Location { get; set; }

    public List<Guid> Employees { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public CompaniesR()
    {
        Id = Guid.NewGuid();
    }
}