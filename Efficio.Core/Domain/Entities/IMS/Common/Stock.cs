using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Efficio.Core.Domain.Entities.Base;
using Efficio.Core.Domain.Entities.Common;

namespace Efficio.Core.Domain.Entities.IMS.Common;

public class Stock : BaseDeletableEntity
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;
    
    public Stock? MainStock { get; set; }
    
    public ICollection<Stock> SubStocks { get; set; } = new List<Stock>();
    public ICollection<DepartmentStock> DepartmentStocks { get; set; } = new List<DepartmentStock>();
    
    [NotMapped]
    public ICollection<Department> Departments => DepartmentStocks.Select(x => x.Department).ToList();
}