namespace CoverGo.Task.Api.DTOs;

public class AddInsuredGroupDto
{
    public int NumberOfEmployees { get; set; }
    public required string PlanId { get; set; }
}