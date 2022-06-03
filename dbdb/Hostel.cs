namespace dbdb
{
    public partial class Hostel
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public DateTime? DateRoomOccupied { get; set; }
        public DateTime? DateRoomFree { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
    }
}
