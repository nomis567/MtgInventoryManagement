using System.ComponentModel.DataAnnotations;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;

public class Version
{
	[Key]
	public string version {get; set;} = string.Empty;
}
