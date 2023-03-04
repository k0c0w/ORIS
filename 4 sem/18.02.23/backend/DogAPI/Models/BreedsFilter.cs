namespace DogAPI.Models;

public class BreedsFilter
{
    public float? min_lifespan { get; set; }
    public float? max_lifespan { get; set; }

    public float? min_height { get; set; }

    public float? max_height { get; set; }
    
    public float? min_weight { get;  set; }

    public float? max_weight { get;  set; }
    
    public string? breed_group { get;  set; }
    

    public void Normalize()
    {
        if (IsHeight)
        {
            if (max_height == null)
                max_height = min_height;
            else if (min_height == null)
                min_height = max_height;
        }

        if (IsWeight)
        {
            if (max_weight == null)
                max_weight = min_weight;
            else if(min_weight == null)
                min_weight = max_weight;
        }

        if (IsLifespan)
        {
            if (max_lifespan == null)
                max_lifespan = min_lifespan;
            else if(min_lifespan == null)
                min_lifespan = max_weight;
        }
    }

    public bool IsFilter => IsBreedGroup || IsLifespan || IsWeight || IsHeight;
    
    public bool IsBreedGroup => !string.IsNullOrEmpty(breed_group);
    
    public bool IsLifespan => min_lifespan != null || max_lifespan != null;
    public bool IsWeight => min_weight != null || max_weight != null;
    public bool IsHeight => min_height != null || max_height != null;
}