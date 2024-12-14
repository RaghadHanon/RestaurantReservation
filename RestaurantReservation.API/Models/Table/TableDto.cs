namespace RestaurantReservation.API.ModelView.Table;
public class TableDto
{
    public int TableId { get; set; }
    public int RestaurantID { get; set; }
    public int Capacity { get; set; }
}