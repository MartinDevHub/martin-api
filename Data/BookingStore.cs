using Marcoff_API.Models.Dto;


namespace Marcoff_API.Data
{
    public static class BookingStore
    {
        public static List<BookingDto> BookingDtoList = new List<BookingDto>

        { new BookingDto{Id=1, Name="Jose", BedsOcuppied=3},
        new BookingDto{Id=2,Name="Susana", BedsOcuppied=4}

    };

}
}
