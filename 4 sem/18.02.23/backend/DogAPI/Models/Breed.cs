namespace DogAPI.Models
{
    public record class Breed(int id, string temperament, string origin, string name, int min_weight, int max_weight,
        int min_height, int max_height, TimeSpan life_span, string breed_for, string breed_group, string image_url);
}
