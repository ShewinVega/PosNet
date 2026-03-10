namespace PosNet.Domain.Constants
{
    public class Permissions
    {

        private const string View = nameof(View);
        private const string Create = nameof(Create);
        private const string Edit = nameof(Edit);
        private const string Delete = nameof(Delete);

        public const string UserView = $"user:{View}";
        public const string UserCreate = $"user:{Create}";
        public const string UserEdit = $"user:{Edit}";
        public const string UserDelete = $"user:{Delete}";

        public const string InventoryView = $"inventory:{View}";
        public const string InventoryCreate = $"inventory:{Create}";
        public const string InventoryEdit = $"inventory:{Edit}";
        public const string InventoryDelete = $"inventory:{Delete}";
        public const string InventoryStock = "inventory:ChargeStock"; // Charge the stock

        public const string SalesView = $"sales:{View}";
        public const string SalesVoid = "sales:void"; // Invalid Invoice
        public const string SalesDiscount = "sales:ApplyDiscount";

        public const string ReportsView = $"reports:{View}";
        public const string ReportsViewProfit = "reports:ViewProfit";

        public static string[] All()
        {
            return [
                UserView, UserCreate, UserEdit, UserDelete,
                InventoryView, InventoryCreate, InventoryEdit, InventoryDelete, InventoryStock,
                SalesView, SalesVoid, SalesDiscount, ReportsView, ReportsViewProfit
            ];
        }
    }
}
