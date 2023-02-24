namespace DogAPI.Dtoes;

public record class UserReviewDto(string email, string name, string review, DateTime sent);