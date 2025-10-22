
namespace TasksBook.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceId) 
    : Exception($"{resourceType} with Id: {resourceId} dosen't exist")
{
}
