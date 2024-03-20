using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOS;

public class EventDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Event name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Event description is required")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Event date is required")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Event Date")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Event location is required")]
    public string Location { get; set; }

    [Required(ErrorMessage = "Total tickets available is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Total tickets must be greater than 0")]
    public int TotalTickets { get; set; }

    [Required(ErrorMessage = "Price per ticket is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}