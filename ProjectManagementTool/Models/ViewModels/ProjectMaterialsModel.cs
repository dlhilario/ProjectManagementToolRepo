using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models.ViewModels
{
    [Serializable]
    public class ProjectMaterialsModel
    {
        public List<Material> Materials { get; set; }
    }
    [Serializable]
    public class Material
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ItemQuantity { get; set; }
        public string InvoiceNumber { get; set; }
        public int Projects_ID { get; set; }
        public int ProjectID { get; set; }
    }

}