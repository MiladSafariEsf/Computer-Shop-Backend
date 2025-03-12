using APICH.CORE.Entity;
using Microsoft.AspNetCore.SignalR;

namespace APICH.API.Models.Add
{
    public class ReviewModel
    {
        public int Rating { get; set; }
        public Guid ProductId { get; set; }
        public string Comment { get; set; }
    }
}
