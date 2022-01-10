using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IyaElepoApp.Models.ConData
{
  [Table("ProductSupplyReportView", Schema = "dbo")]
  public partial class ProductSupplyReportView
  {
    public Int64 SupplyID
    {
      get;
      set;
    }
    public DateTime SupplyDate
    {
      get;
      set;
    }
    public string Supplier
    {
      get;
      set;
    }
    public string Product
    {
      get;
      set;
    }
    public decimal QuantityInLitres
    {
      get;
      set;
    }
  }
}
